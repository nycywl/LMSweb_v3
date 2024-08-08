using LMSweb_v3.Services;
using LMSweb_v3.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace LMSweb_v3.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class PromptManageController : Controller
    {
        private readonly CourseService _context;
        private readonly ChatService _chatService;

        public PromptManageController(CourseService context, ChatService chat)
        {
            _context = context;
            _chatService = chat;
        }

        public IActionResult Edit(string cid)
        {
            var data = _context.GetCourseDefaultPrompt(cid);
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string cid, PromptManageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (Regex.IsMatch(model.Greeting, @"<!reference_data>|<!question>|<!context>"))
            {
                ModelState.AddModelError("Greeting", "不可以包含系統內定的替換符號!!");
                return View(model);
            }
            var isUpdated = _context.UpdateDefaultPrompt(model);
            if (!isUpdated)
            {
                ViewBag.isError = true;
                ViewBag.Message = "無法更新";
                return View(model);
            }
            return RedirectToAction("Index", "Material", new { cid = model.CourseId });
        }

        public IActionResult Export(string cid)
        {
            var data = _chatService.GetChatHistory(cid);
            var fileName = $"chat_history_{cid}.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            using var stream = new MemoryStream();
            data.SaveAs(stream);
            var content = stream.ToArray();
            stream.Flush();
            data.Dispose();
            return File(content, contentType, fileName);
        }
    }
}
