using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.Fonts;
using System.IO;

namespace FeedbackApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableCors("AllowFrontend")]
public class CaptchaController : ControllerBase
{
    private const string SessionKey = "CaptchaCode";

    [HttpGet]
    public IActionResult Get()
    {
        var code = Random.Shared.Next(10000, 99999).ToString();
        HttpContext.Session.SetString(SessionKey, code);

        using var image = new Image<Rgba32>(150, 50);

        // Заливаем фон белым
        image.Mutate(ctx => ctx.Fill(Color.White));

        // Шрифт
        using var fontStream = System.IO.File.OpenRead("fonts/arial.ttf");
        var collection = new FontCollection();
        var family = collection.Add(fontStream);
        var font = family.CreateFont(24);


        // Рисуем текст
        image.Mutate(ctx => ctx.DrawText(code, font, Color.Black, new PointF(10, 10)));

        using var ms = new MemoryStream();
        image.SaveAsPng(ms);
        ms.Seek(0, SeekOrigin.Begin);

        return File(ms.ToArray(), "image/png");
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
