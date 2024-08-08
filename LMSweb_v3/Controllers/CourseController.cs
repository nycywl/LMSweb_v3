using LMSweb_v3.Services;
using LMSweb_v3.ViewModels.Course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMSweb_v3.Controllers;

[Authorize(Roles = "Teacher")]
public class CourseController : Controller
{
    private readonly CourseService _courseService;

    public CourseController(CourseService course)
    {
        _courseService = course;
    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(CourseCreateViewModel courseViewModel)
    {
        /*
         * 新增課程到資料庫
         */

        var UID = User.Claims.FirstOrDefault(x => x.Type == "UID");   //抓出當初記載Claims陣列中的TID
        if (UID == null)
        {
            return Unauthorized();
        }

        if (ModelState.IsValid)
        {
            var courseId = _courseService.CreateCourse(UID.Value, courseViewModel.Name);
            return RedirectToAction("Index", "StudentManagement", new { cid = courseId, firstCreate = true });
        }
        return View(courseViewModel);
    }

    public ActionResult Edit(string cid)
    {
        /*
         * 透過id找到課程，並將資料傳到 View                   
         */

        var course = _courseService.GetCourseEditViewModel(cid);
        return View(course);
    }

    [HttpPost]
    public ActionResult Edit(string cid, CourseEditViewModel courseViewModel)
    {
        /*
         *  透過id找到課程，並將資料更新到資料庫
         */
        if (ModelState.IsValid)
        {
            _courseService.EditCourse(cid, courseViewModel);
            return RedirectToAction("Home", "Teacher");
        }
        return View(courseViewModel);
    }

    public ActionResult Delete(string cid)
    {
        /*
         * 透過id找到課程，並將資料傳到 View                   
         */

        var teacherId = User.Claims.FirstOrDefault(x => x.Type == "UID");
        if (teacherId == null)
        {
            return Unauthorized();
        }

       var delsteViewModel = _courseService.GetWillBeDeleteCourse(cid, teacherId.Value);
        return View(delsteViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(CourseDeleteViewModel deletedCourse)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Home", "Teacher");
        }
        var courseId = deletedCourse.CourseID ?? "";
        _courseService.DeleteCourse(courseId);
        return RedirectToAction("Home", "Teacher");
    }
}
