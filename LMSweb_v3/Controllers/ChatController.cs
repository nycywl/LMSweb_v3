using LMSweb_v3.Services;
using LMSweb_v3.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LMSweb_v3.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly AzureOpenAIService _service;

    public ChatController(AzureOpenAIService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] QuestionViewModel chat)
    {
        if (string.IsNullOrEmpty(chat.Question) || string.IsNullOrWhiteSpace(chat.Question))
        {
            return BadRequest();
        }
        var result = await _service.GetAnswer(chat);
        //需增加檔案名稱
        var response = new ChatResultViewModel
        {
            Answer = result
        };

        return Ok(response);
    }
}
