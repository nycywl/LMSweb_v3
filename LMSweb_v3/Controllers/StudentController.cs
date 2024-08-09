using LMSweb_v3.Services;
using LMSweb_v3.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMSweb_v3.Controllers;

[Authorize(Roles = "Student")]
public class StudentController : Controller
{
    private readonly StudentManagementSercices _studentManagement;

    public StudentController(StudentManagementSercices studentManagement)
    {
        _studentManagement = studentManagement;
    }

    public IActionResult Home()
    {
        var sid = User.Claims.FirstOrDefault(x => x.Type == "UID");   //抓出當初記載Claims陣列中的SID

        if (sid == null)
        {
            return Unauthorized();
        }

        var data = _studentManagement.GetCoursesForStudent(sid.Value);

        if (data == null || !data.Any())
        {
            ViewBag.isError = true;
            return View();
        }

        return View(data);
    }


    public IActionResult AskAIAssistent(string cid)
    {
        var sid = User.Claims.SingleOrDefault(x => x.Type == "UID")?.Value;
        if (sid == null)
        {
            return Unauthorized();
        }
        var students = _studentManagement.GetStudent(cid, sid);

        if (students == null)
        {
            return RedirectToAction("Home");
        }
        var data = new AskAIAssistentViewModel
        {
            CourseId = students.CourseId ?? "",
            CourseName = students.CourseName ?? "",
            UserId = sid,
            Greeting = students.Greeting ?? ""
        };
        return View(data);
    }
}
