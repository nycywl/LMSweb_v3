namespace LMSweb_v3.Services;

public class FileOperationService
{
    private readonly string _rootDirectory;


    public FileOperationService()
    {
        _rootDirectory = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot");
    }

    /// <summary>
    /// 取得檔案路徑
    /// </summary>
    /// <param name="filename">檔名</param>
    /// <param name="directory">資料夾名稱，預設為UploadFiles</param>
    /// <returns></returns>
    public string? GetFilePath(string filename, string directory = "UploadFiles")
    {
        if (!directory.Equals("uploadfiles", StringComparison.CurrentCultureIgnoreCase) &&
            !directory.Equals("prompt_template", StringComparison.CurrentCultureIgnoreCase))
        {
            directory = Path.Combine("UploadFiles", directory);
        }
        string pathFile = Path.Combine(_rootDirectory, directory, filename);
        if (File.Exists(pathFile))
        {
            return pathFile;
        }
        return null;
    }

    /// <summary>
    /// 產生資料夾
    /// </summary>
    /// <param name="directory">資料夾名稱</param>
    public void CreateDirectory(string directory)
    {
        string path = Path.Combine(_rootDirectory, directory);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    /// <summary>
    /// 刪除資料夾
    /// </summary>
    /// <param name="directory">資料夾名稱</param>
    public void RemoveDirectory(string directory)
    {
        if (!directory.Equals("uploadfiles", StringComparison.CurrentCultureIgnoreCase))
        {
            directory = Path.Combine("UploadFiles", directory);
            string path = Path.Combine(_rootDirectory, directory);
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }        
    }

    /// <summary>
    /// 儲存檔案
    /// </summary>
    /// <param name="uploadedFile">上傳的檔案</param>
    /// <param name="directory">要儲存的資料夾，預設為UploadFiles</param>
    /// <returns>儲存的檔案路徑</returns>
    public async Task<string> SaveFile(IFormFile uploadedFile, string? directory)
    {
        if (string.IsNullOrWhiteSpace(directory) ||
            directory.Equals("uploadfiles", StringComparison.CurrentCultureIgnoreCase))
        {
            directory = "UploadFiles";
        }
        else
        {
            directory = Path.Combine("UploadFiles", directory);
            CreateDirectory(directory);
        }
        var fileName = uploadedFile.FileName;
        return await SaveFile(uploadedFile, directory, fileName);
    }

    /// <summary>
    /// 儲存檔案
    /// </summary>
    /// <param name="uploadedFile">上傳的檔案</param>
    /// <param name="directory">要儲存的資料夾</param>
    /// <param name="fileName">檔名</param>
    /// <returns>儲存的檔案路徑</returns>
    private async Task<string> SaveFile(IFormFile uploadedFile, string directory, string fileName)
    {
        string pathFile = Path.Combine(_rootDirectory, directory, fileName);
        using (var stream = new FileStream(pathFile, FileMode.Create))
        {
            await uploadedFile.CopyToAsync(stream);
        }

        return pathFile;
    }

    /// <summary>
    /// 刪除檔案
    /// </summary>
    /// <param name="filename">檔名</param>
    /// <param name="directory">資料夾名稱，預設為UploadFiles</param>
    /// <returns>True/False</returns>
    public bool RemoveFile(string filename, string directory = "UploadFiles")
    {
        if (!directory.Equals("uploadfiles", StringComparison.CurrentCultureIgnoreCase))
        {
            directory = Path.Combine("UploadFiles", directory);
        }
        string pathFile = Path.Combine(_rootDirectory, directory, filename);
        if (File.Exists(pathFile))
        {
            File.Delete(pathFile);
            return true;
        }
        return false;
    }
}
