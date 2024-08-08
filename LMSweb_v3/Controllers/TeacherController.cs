using LMSweb_v3.Services;
using LMSweb_v3.ViewModels.Course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMSweb_v3.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        private readonly CourseService _context;

        public TeacherController(CourseService context)
        {
            _context = context;
        }

        public IActionResult Home()
        {
            var teacher = User.Claims.FirstOrDefault(x => x.Type == "UID");   //抓出當初記載Claims陣列中的UID
            if (teacher == null)
            {
                return Unauthorized();
            }
            var tid = teacher.Value;
            var courses = _context.GetCourses(tid);
            return View(courses);
        }
    }
}
