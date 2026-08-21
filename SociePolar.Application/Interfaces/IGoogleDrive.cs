namespace SociePolar.Application.Interfaces
{
    public interface IGoogleDrive
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, string? folderId = null);
        Task<string> CreateFolderAsync(string folderName, string? parentFolderId = null);
        Task RenameFileAsync(string fileId, string newName);
        Task DeleteFileAsync(string fileId);
    }
}
