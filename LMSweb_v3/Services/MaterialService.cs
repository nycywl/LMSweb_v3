using LMSweb_v3.ViewModels;
using LMSweb_v3.ViewModels.Material;
using LMSwebDB.Models;
using LMSwebDB.Repositories;
using System.Data;
using System.Net;

namespace LMSweb_v3.Services;

public class MaterialService
{
    private readonly FileOperationService _fileOperation;
    private readonly FileProcessService _fileUpload;
    private readonly LMSRepository _context;
    public MaterialService(LMSRepository repo, FileProcessService fileProcess,
        FileOperationService fileOperation)
    {
        _context = repo;
        _fileOperation = fileOperation;
        _fileUpload = fileProcess;
    }

    /// <summary>
    /// 取得課程底下的所有教材
    /// </summary>
    /// <param name="courseId">課程編號</param>
    /// <returns>教材清單</returns>
    public MaterialIndexViewModel? GetMaterials(string courseId)
    {
        // 找出在每一門課底下所有的教材，至少須包含課程名稱、教材檔案名稱、教材檔案上傳時間
        var course = _context.Query<Course>().FirstOrDefault(x => x.CourseId == courseId);
        if (course == null)
        {
            return null;
        }

        var result = _context.Query<Material>().Where(x => x.CourseID == courseId)
                            .Select(x => new MaterialItem
                            {
                                MaterialID = x.MaterialId,
                                FileName = x.FileName,
                                UploadTime = x.UploadTime.ToString("yyyy-MM-dd HH:mm:ss"),
                                IsTrainingMaterial = false
                            }).ToList();
        foreach (var item in result)
        {
            if (_context.Query<QnA>().Any(x => x.MaterialId == item.MaterialID))
            {
                item.IsTrainingMaterial = true;
            }
        }


        var viewModel = new MaterialIndexViewModel
        {
            CourseID = courseId,
            CourseName = course.CourseName,
            Materials = result
        };
        return viewModel;
    }

    /// <summary>
    /// 新增教材
    /// </summary>
    /// <param name="courseId">課程編號</param>
    /// <param name="fileName">檔名</param>
    public void AddMaterialToCourse(string courseId, string fileName)
    {
        var material = new Material
        {
            MaterialId = Guid.NewGuid().ToString(),
            CourseID = courseId,
            FileName = fileName,
            UploadTime = DateTime.Now
        };
        _context.Create(material);
        _context.SaveChanges();
    }

    /// <summary>
    /// 取得教材路徑
    /// </summary>
    /// <param name="materialId">教材編號</param>
    /// <returns>教材路徑</returns>
    public string? GetMaterialPath(string materialId)
    {
        var material = _context.Query<Material>().FirstOrDefault(x => x.MaterialId == materialId);
        if (material == null)
        {
            return null;
        }
        return _fileOperation.GetFilePath(material.FileName, material.CourseID);
    }

    /// <summary>
    /// 刪除教材
    /// </summary>
    /// <param name="materialId">教材編號</param>
    /// <returns>刪除結果</returns>
    public DeleteResultViewModel DeleteMaterial(string materialId)
    {
        var material = _context.Query<Material>().FirstOrDefault(x => x.MaterialId == materialId);
        var qnas = _context.Query<QnA>().Where(x => x.MaterialId == materialId).ToList();
        if (material == null)
        {
            return new DeleteResultViewModel
            {
                Success = false,
                Message = "教材刪除失敗"
            };
        }
        _context.Delete(material);
        _context.DeleteMany(qnas);        
        var isDeleted = _fileOperation.RemoveFile(material.FileName, material.CourseID);
        if (!isDeleted)
        {
            return new DeleteResultViewModel
            {
                Success = false,
                Message = "教材刪除失敗"
            };
        }
        _context.SaveChanges();
        return new DeleteResultViewModel
        {
            Success = true,
            CourseId = material.CourseID
        };
    }

    /// <summary>
    /// 訓練教材
    /// </summary>
    /// <param name="materialId">教材編號</param>
    /// <returns>訓練結果</returns>
    public async Task<TrainingResultViewModel> TrainingMaterialAsync(string materialId)
    {
        var material = _context.Query<Material>().FirstOrDefault(x => x.MaterialId == materialId);
        if (material == null)
        {
            return new TrainingResultViewModel
            {
                Status = (int)HttpStatusCode.BadRequest,
                Success = false,
                ErrorMessage = "找不到教材"
            };
        }
        await _fileUpload.ProcessingFileAsync(material.FileName, materialId, material.CourseID);
        return new TrainingResultViewModel
        {
            Status = (int)HttpStatusCode.OK,
            Success = true,
            ErrorMessage = "訓練完成"
        };
    }
}
