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
        //private readonly string[] _scopes = { DriveService.Scope.Drive }; // Scope completo para poder borrar/crear
        private readonly string[] _scopes = { DriveService.Scope.DriveFile };

        public GoogleDriveService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var jsonPath = _config["GoogleDrive:CredentialsPath"];
            GoogleCredential credential;

            using (var stream = new FileStream(jsonPath, FileMode.Open, FileAccess.Read))
            {
                // _scopes debe incluir "www.googleapis.com"
                credential = GoogleCredential.FromStream(stream!).CreateScoped(_scopes);
            }

            // Solicita el token de acceso de forma asíncrona
            var token = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();

            if (string.IsNullOrEmpty(token))
            {
                // Fuerza la obtención del token si no está en caché
                token = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();
            }

            return token;
        }

        private DriveService GetService()
        {
            var jsonPath = _config["GoogleDrive:CredentialsPath"];
            GoogleCredential? credential;
            using (var stream = new FileStream(jsonPath!, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(_scopes);
            }

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
                MimeType = "application/vnd.google-apps.folder", // MimeType especial de carpetas
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

            // Preparamos la consulta de búsqueda (query)
            var query = $"name = '{folderName}' and mimeType = 'application/vnd.google-apps.folder' and '{parentId}' in parents and trashed = false";

            var request = service.Files.List();
            request.Q = query;
            request.Fields = "files(id, name)"; // Solicitamos solo el ID y el nombre de los archivos encontrados
            request.SupportsAllDrives = true;
            request.IncludeItemsFromAllDrives = true; // Importante para Shared Drives

            var result = await request.ExecuteAsync();

            // Verificamos si la lista contiene algún archivo/carpeta
            if (result.Files != null && result.Files.Any())
            {
                // Devuelve el ID de la primera carpeta encontrada que coincida con el nombre exacto.
                // Aunque la query ya es exacta, esto añade una capa de seguridad.
                var existingFolder = result.Files.FirstOrDefault(f => f.Name == folderName);
                return existingFolder?.Id;
            }

            return null; // La carpeta no existe
        }

        public async Task TrashPdfFilesInFolderAsync(string folderId)
        {
            try
            {
                var service = GetService();
                string? pageToken = null;

                do
                {
                    // 1. Configurar la búsqueda de archivos PDF dentro de la carpeta específica
                    var listRequest = service.Files.List();
                    listRequest.Q = $"'{folderId}' in parents and mimeType = 'application/pdf' and trashed = false";
                    listRequest.Spaces = "drive";
                    listRequest.Fields = "nextPageToken, files(id, name)";
                    listRequest.PageToken = pageToken;

                    // Habilitar soporte para Shared Drives (Unidades Compartidas)
                    listRequest.SupportsAllDrives = true;
                    listRequest.IncludeItemsFromAllDrives = true;

                    // 2. Ejecutar la búsqueda
                    var result = await listRequest.ExecuteAsync();
                    var files = result.Files;

                    if (files != null && files.Count > 0)
                    {
                        foreach (var file in files)
                        {
                            // 3. Preparar la actualización para marcar como "Trashed"
                            var fileMetadata = new Google.Apis.Drive.v3.Data.File() { Trashed = true };
                            var updateRequest = service.Files.Update(fileMetadata, file.Id);
                            updateRequest.SupportsAllDrives = true;

                            await updateRequest.ExecuteAsync();
                            Console.WriteLine($"Enviado a papelera: {file.Name} ({file.Id})");
                        }
                    }

                    // Actualizar el token para la siguiente página de resultados
                    pageToken = result.NextPageToken;

                } while (pageToken != null);

                Console.WriteLine("Proceso de limpieza completado.");
            }
            catch (Exception ex)
            {
                // Documentación oficial sobre errores: https://developers.google.com
                Console.WriteLine($"Error al procesar la carpeta: {ex.Message}");
            }
        }

        public async Task TrashPdfFileByIdAsync(string fileId)
        {
            try
            {
                var service = GetService();

                // 1. Preparar la actualización para marcar el archivo como "Trashed"
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Trashed = true
                };

                // 2. Configurar la solicitud de actualización para el archivo específico
                var updateRequest = service.Files.Update(fileMetadata, fileId);

                // Habilitar soporte para Shared Drives (Unidades Compartidas)
                updateRequest.SupportsAllDrives = true;

                // 3. Ejecutar la actualización
                var updatedFile = await updateRequest.ExecuteAsync();

                Console.WriteLine($"Archivo enviado a la papelera correctamente. ID: {fileId}");
            }
            catch (Exception ex)
            {
                // Documentación oficial sobre errores: https://developers.google.com
                Console.WriteLine($"Error al enviar el archivo a la papelera: {ex.Message}");
            }
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, string? folderId = null)
        {
            var service = GetService();
            // Si no especifican carpeta, usa la raíz de la Unidad Compartida
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
            if (progress.Status != UploadStatus.Completed) throw new Exception("Error subiendo archivo");

            return request.ResponseBody.Id;
        }

        public async Task<string> GetFileAsync(string fileId)
        {
            var service = GetService();

            var request = service.Files.Get(fileId);
            var stream = new MemoryStream();
            await request.DownloadAsync(stream);
            stream.Position = 0;

            // Convertir a Base64 para mostrarlo en el componente
            var base64 = Convert.ToBase64String(stream.ToArray());

            return base64;
        }
    }
}
