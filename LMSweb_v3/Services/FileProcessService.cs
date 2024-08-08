using LMSwebDB.Repositories;
using LMSweb_v3.ViewModels.StudentManagement;
using LMSwebDB.Models;
using LMSweb_v3.ViewModels;
using Docnet.Core;
using Docnet.Core.Models;
using DocumentFormat.OpenXml.Wordprocessing;

namespace LMSweb_v3.Services;

public class FileProcessService
{
    private readonly FileOperationService _fileOperationService;
    private readonly AzureOpenAIService _azureOpenAIService;
    private readonly LMSRepository _context;
    public FileProcessService(FileOperationService fileOperationService,
        AzureOpenAIService azureOpenAIService, LMSRepository context)
    {
        _fileOperationService = fileOperationService;
        _azureOpenAIService = azureOpenAIService;
        _context = context;
    }

    /// <summary>
    /// 處理上傳的學生資料檔案，檢查是否有重複的學生資料、學生性別是否為男或女
    /// </summary>
    /// <param name="uploadedFile">上傳的檔案</param>
    /// <returns>學生資料</returns>
    public async Task<ProcessingResultViewModel?> ProcessingStudentData(IFormFile uploadedFile)
    {
        if (uploadedFile == null || uploadedFile.Length == 0)
        {
            return null;
        }
        var result = new ProcessingResultViewModel();
        string pathFile = await _fileOperationService.SaveFile(uploadedFile, "uploadfiles");
        var data = ReadDataFromFile(pathFile).Skip(1).ToList();
        _fileOperationService.RemoveFile(pathFile);
        if (data.Count() == 0)
        {
            result.Message = "檔案內容為空";
            return result;
        }

        // 檢查學生性別是否為男或女
        if (data.Where(x => x.StudentSex != "男" && x.StudentSex != "女").Count() > 0)
        {
            result.Message = "學生性別只能為男或女";
            return result;
        }

        result.EnrolledStudents = data;

        return result;
    }

    /// <summary>
    /// 讀取上傳的學生資料檔案
    /// </summary>
    /// <param name="filepath">檔案路徑</param>
    /// <returns>修課學生清單</returns>
    private static IEnumerable<EnrolledStudent> ReadDataFromFile(string filepath)
    {
        using var reader = new StreamReader(filepath);
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (line == null) break;
            var values = line.Split(',');
            if (!values.All(string.IsNullOrEmpty))
            {
                yield return new EnrolledStudent
                {
                    StudentId = values[0],
                    StudentName = values[1],
                    StudentSex = values[2]
                };
            }
            else break;
        }
    }

    /// <summary>
    /// 處理上傳的PDF檔案內容，將每頁的文字內容轉換成向量後存入資料庫
    /// </summary>
    /// <param name="filepath">檔案路徑</param>
    public async Task ProcessingFileAsync(string filepath, string mid, string cid)
    {
        var pdf_file = _fileOperationService.GetFilePath(filepath, cid) ?? throw new FileNotFoundException("找不到檔案");
        List<QnA> qnas = [];
        using (var pdfReader = DocLib.Instance.GetDocReader(pdf_file, new PageDimensions()))
        {
            for (int i = 0; i < pdfReader.GetPageCount(); i++)
            {
                using (var pageReader = pdfReader.GetPageReader(i))
                {
                    string thispage = pageReader.GetText();
                    if (!string.IsNullOrEmpty(thispage))
                    {
                        thispage = thispage.Replace(" ", "").Replace("\n", "");
                        var vector = await _azureOpenAIService.GetEmbeddingAsync(thispage);
                        var qna = new QnA
                        {
                            QnAId = Guid.NewGuid().ToString(),
                            CourseId = cid,
                            MaterialId = mid,
                            Embeddings = string.Join(",", vector),
                            Answer = thispage,
                        };
                        qnas.Add(qna);
                    }
                }
            }
        }
        _context.CreateMany(qnas);
        _context.SaveChanges();
    }
}
