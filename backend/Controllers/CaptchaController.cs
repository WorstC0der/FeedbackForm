using CaptchaGen;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Imaging;

namespace FeedbackApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CaptchaController : ControllerBase
{
    private const string SessionKey = "CaptchaCode";

    [HttpGet]
    public IActionResult Get()
    {
        var code = Random.Shared.Next(10000, 99999).ToString();
        HttpContext.Session.SetString(SessionKey, code);

        // цифры, высота, ширина, размер шрифта, искажение
        var bytes = ImageFactory.GenerateImage(code, 50, 150, 24, 2);
        return File(bytes, "image/png");
    }

    public record CaptchaValidateRequest(string Code);

    [HttpPost("validate")]
    public IActionResult Validate([FromBody] CaptchaValidateRequest body)
    {
        var stored = HttpContext.Session.GetString(SessionKey);
        if (string.IsNullOrWhiteSpace(stored) || string.IsNullOrWhiteSpace(body.Code))
        {
            return BadRequest(new { ok = false, error = "CAPTCHA_INVALID" });
        }

        var ok = string.Equals(stored, body.Code.Trim(), StringComparison.OrdinalIgnoreCase);
        if (!ok)
        {
            return BadRequest(new { ok = false, error = "CAPTCHA_INVALID" });
        }

        HttpContext.Session.Remove(SessionKey);
        return Ok(new { ok = true });
    }
}


