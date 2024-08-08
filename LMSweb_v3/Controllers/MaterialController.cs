using LMSweb_v3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMSweb_v3.Controllers;

[Authorize]
public class MaterialController : Controller
{
    private readonly FileOperationService _fileOperation;
    private readonly MaterialService _materialService;

    public MaterialController(MaterialService material, FileOperationService service)
    {
        _fileOperation = service;
        _materialService = material;
    }

    public IActionResult Index(string cid)
    {
        var role = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
        if (cid == null)
        {
            return RedirectToAction("Home", role?.Value);
        }
        var viewModel = _materialService.GetMaterials(cid);
        if (viewModel == null)
        {
            return RedirectToAction("Home", role?.Value);
        }
        return View(viewModel);
    }

    [HttpPost]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Upload(string cid, IFormFile post_file)
    {
        if (post_file == null || post_file.Length == 0)
        {
            return Json(new { success = false, message = "檔案上傳失敗" });
        }

        var result = await _fileOperation.SaveFile(post_file, cid);
        var file_name = result.Split("\\").LastOrDefault();

        if (string.IsNullOrEmpty(file_name))
        {
            return Json(new { success = false, message = "檔案上傳失敗" });
        }

        _materialService.AddMaterialToCourse(cid, file_name);
        return Json(new { success = true });
    }

    public async Task<IActionResult> Download(string mid)
    {
        var contentType = "application/octet-stream";
        var path = _materialService.GetMaterialPath(mid);
        if (string.IsNullOrEmpty(path))
        {
            return RedirectToAction("Home", "Teacher");
        }
        var memory = new MemoryStream();
        using (var stream = new FileStream(path, FileMode.Open))
        {
            await stream.CopyToAsync(memory);
        }
        memory.Position = 0;
        return File(memory, contentType, Path.GetFileName(path));
    }

    [Authorize(Roles = "Teacher")]
    public IActionResult Delete(string mid)
    {
        var deletedResult = _materialService.DeleteMaterial(mid);
        if (!deletedResult.Success)
        {
            return RedirectToAction("Home", "Teacher");
        }
        return RedirectToAction("Index", new { cid = deletedResult.CourseId });
    }

    [Authorize(Roles = "Teacher")]
    [ActionName("Training")]
    [HttpPost]
    public async Task<IActionResult> TrainingAsync(string mid)
    {
        var result = await _materialService.TrainingMaterialAsync(mid);
        return Json(result);
    }
}
