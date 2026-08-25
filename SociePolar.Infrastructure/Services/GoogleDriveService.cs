using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Microsoft.Extensions.Configuration;

namespace SociePolar.Infrastructure.Services
{
    public class GoogleDriveService
    {
        private readonly IConfiguration _config;
        private readonly string[] _scopes = { DriveService.Scope.DriveFile };

        public GoogleDriveService(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Instancia centralizada de la credencial de la Cuenta de Servicio
        /// impersonando al Usuario Bot del dominio (@empresaspolar.com).
        /// </summary>
        private GoogleCredential GetCredential()
        {
            var jsonPath = _config["GoogleDrive:CredentialsPath"];
            var botUserEmail = _config["GoogleDrive:ImpersonatedUserEmail"]; // Ej: bot.societario@empresaspolar.com

            if (string.IsNullOrEmpty(jsonPath) || !File.Exists(jsonPath))
            {
                throw new FileNotFoundException($"No se encontró el archivo de credenciales de Google Drive en: {jsonPath}");
            }

            if (string.IsNullOrEmpty(botUserEmail))
            {
                throw new ArgumentNullException("GoogleDrive:ImpersonatedUserEmail", "Debe configurar el correo del usuario bot en appsettings.json");
            }

            using (var stream = new FileStream(jsonPath, FileMode.Open, FileAccess.Read))
            {
                return GoogleCredential.FromStream(stream)
                    .CreateScoped(_scopes)
                    .CreateWithUser(botUserEmail); // <--- AQUÍ OCURRE LA MAGIA DE LA IMPERSONACIÓN
            }
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var credential = GetCredential();

            // Solicita el token de acceso de forma asíncrona actuando como el usuario Bot
            var token = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

            if (string.IsNullOrEmpty(token))
            {
                token = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();
            }

            return token;
        }

        private DriveService GetService()
        {
            var credential = GetCredential();

            return new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = _config["GoogleDrive:ApplicationName"],
            });
        }

        public async Task<string> CreateFolderAsync(string folderName, string? parentFolderId = null)
        {
            string? folderId = await GetFolderIdByNameAsync(folderName, parentFolderId);
            if (!string.IsNullOrEmpty(folderId))
            {
                return folderId;
            }

            var service = GetService();
            var parentId = parentFolderId ?? _config["GoogleDrive:SharedDriveId"];

            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = folderName,
                MimeType = "application/vnd.google-apps.folder",
                Parents = new List<string> { parentId! }
            };

            var request = service.Files.Create(fileMetadata);
            request.Fields = "id";
            request.SupportsAllDrives = true;

            var file = await request.ExecuteAsync();
            return file.Id;
        }

        public async Task<string?> GetFolderIdByNameAsync(string folderName, string? parentFolderId = null)
        {
            var service = GetService();
            var parentId = parentFolderId ?? _config["GoogleDrive:SharedDriveId"];

            var query = $"name = '{folderName}' and mimeType = 'application/vnd.google-apps.folder' and '{parentId}' in parents and trashed = false";

            var request = service.Files.List();
            request.Q = query;
            request.Fields = "files(id, name)";
            request.SupportsAllDrives = true;
            request.IncludeItemsFromAllDrives = true;

            var result = await request.ExecuteAsync();

            if (result.Files != null && result.Files.Any())
            {
                var existingFolder = result.Files.FirstOrDefault(f => f.Name == folderName);
                return existingFolder?.Id;
            }

            return null;
        }

        public async Task TrashPdfFilesInFolderAsync(string folderId)
        {
            try
            {
                var service = GetService();
                string? pageToken = null;

                do
                {
                    var listRequest = service.Files.List();
                    listRequest.Q = $"'{folderId}' in parents and mimeType = 'application/pdf' and trashed = false";
                    listRequest.Spaces = "drive";
                    listRequest.Fields = "nextPageToken, files(id, name)";
                    listRequest.PageToken = pageToken;
                    listRequest.SupportsAllDrives = true;
                    listRequest.IncludeItemsFromAllDrives = true;

                    var result = await listRequest.ExecuteAsync();
                    var files = result.Files;

                    if (files != null && files.Count > 0)
                    {
                        foreach (var file in files)
                        {
                            var fileMetadata = new Google.Apis.Drive.v3.Data.File() { Trashed = true };
                            var updateRequest = service.Files.Update(fileMetadata, file.Id);
                            updateRequest.SupportsAllDrives = true;

                            await updateRequest.ExecuteAsync();
                        }
                    }

                    pageToken = result.NextPageToken;

                } while (pageToken != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al procesar la carpeta: {ex.Message}");
            }
        }

        public async Task TrashPdfFileByIdAsync(string fileId)
        {
            try
            {
                var service = GetService();
                var fileMetadata = new Google.Apis.Drive.v3.Data.File() { Trashed = true };

                var updateRequest = service.Files.Update(fileMetadata, fileId);
                updateRequest.SupportsAllDrives = true;

                await updateRequest.ExecuteAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar el archivo a la papelera: {ex.Message}");
            }
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, string? folderId = null)
        {
            var service = GetService();
            var parentId = folderId ?? _config["GoogleDrive:SharedDriveId"];

            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = fileName,
                Parents = new List<string> { parentId! }
            };

            var request = service.Files.Create(fileMetadata, fileStream, contentType);
            request.Fields = "id";
            request.SupportsAllDrives = true;

            var progress = await request.UploadAsync();
            if (progress.Status != UploadStatus.Completed)
            {
                throw new Exception($"Error subiendo archivo a Google Drive: {progress.Exception?.Message}");
            }

            return request.ResponseBody.Id;
        }

        public async Task<string> GetFileAsync(string fileId)
        {
            var service = GetService();

            var request = service.Files.Get(fileId);
            request.SupportsAllDrives = true; // Recomendado para Unidades Compartidas

            using var stream = new MemoryStream();
            await request.DownloadAsync(stream);
            stream.Position = 0;

            return Convert.ToBase64String(stream.ToArray());
        }
    }
}