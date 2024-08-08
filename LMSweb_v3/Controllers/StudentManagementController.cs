using LMSweb_v3.Services;
using LMSweb_v3.ViewModels.StudentManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMSweb_v3.Controllers;

[Authorize(Roles = "Teacher")]
public class StudentManagementController : Controller
{
    private readonly FileProcessService _fileUploadService;
    private readonly StudentManagementSercices _studentManagementSercices;

    public StudentManagementController(FileProcessService fileUpload, StudentManagementSercices studentManagement)
    {
        _fileUploadService = fileUpload;
        _studentManagementSercices = studentManagement;
    }

    public IActionResult Index(string cid, bool firstCreate = false)
    {
        if (cid == null)
        {
            return NotFound();
        }

        var relatedData = _studentManagementSercices.GetStudents(cid);
        if (relatedData == null)
        {
            return RedirectToAction("Home", "Teacher");
        }

        return View(relatedData);
    }

    [HttpPost]
    public async Task<IActionResult> Upload(string cid, IFormFile file)
    {
        var enrolledStudents = await _fileUploadService.ProcessingStudentData(file);

        if (enrolledStudents == null || !enrolledStudents.EnrolledStudents.Any())
        {
            return BadRequest(enrolledStudents?.Message);
        }
        _studentManagementSercices.AddStudents(enrolledStudents.EnrolledStudents.ToList(), cid);

        return Json(new { success = true });
    }

    // GET: CreateStudent
    public IActionResult CreateStudent(string cid)
    {
        var data = _studentManagementSercices.GetStudents(cid);
        if (data == null)
        {
            return RedirectToAction("Home", "Teacher");
        }
        var vm = new StudentCreateViewModel
        {
            CourseId = data.CourseId,
            CourseName = data.CourseName,
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateStudent(StudentCreateViewModel vm)
    {
        if (vm != null && vm.Student != null)
        {
            bool isExist = _studentManagementSercices.IsStudentExist(vm.Student.StudentId);
            if (!isExist)
            {
                List<EnrolledStudent> enrolledStudents = [vm.Student];
                _studentManagementSercices.AddStudents(enrolledStudents, vm.CourseId);
                return RedirectToAction("Index", "StudentManagement", new { cid = vm.CourseId });
            }
            ModelState.AddModelError("Student.StudentId", "學號已存在");
            return View(vm);
        }
        return View(vm);
    }

    // GET: StudentEdit
    public IActionResult EditStudent(string sid, string cid)
    {
        var student_data = _studentManagementSercices.GetStudent(cid, sid);

        if (student_data == null)
        {
            return RedirectToAction("Index", "StudentManagement", new { cid });
        }

        return View(student_data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult EditStudent(string sid, string cid, StudentEditViewModel student_data)
    {
        if (string.IsNullOrEmpty(sid))
        {
            throw new ArgumentException($"'{nameof(sid)}' 不可為 Null 或空白。", nameof(sid));
        }

        if (ModelState.IsValid)
        {
            student_data.CourseId = cid;
            student_data.StudentId = sid;
            _studentManagementSercices.UpdateStudent(student_data);
            return RedirectToAction("Index", "StudentManagement", new { cid });
        }
        return View(student_data);
    }

    // 刪除學生頁面 DeleteStudent
    public IActionResult DeleteStudent(string sid, string cid)
    {
        _studentManagementSercices.DeleteStudent(sid, cid);
        return RedirectToAction("Index", "StudentManagement", new { cid });
    }
}
