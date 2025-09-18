using FeedbackApi.Data;
using FeedbackApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeedbackApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly ILogger<MessagesController> _logger;

    public MessagesController(AppDbContext db, ILogger<MessagesController> logger)
    {
        _db = db;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<MessageResponseDto>> Create([FromBody] MessageCreateDto dto)
    {
        // Нормализация данных (удаление пробелов по краям)
        dto.name = dto.name?.Trim() ?? string.Empty;
        dto.email = dto.email?.Trim() ?? string.Empty;
        dto.phone = dto.phone?.Trim() ?? string.Empty;
        dto.text = dto.text?.Trim() ?? string.Empty;

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Поиск или создание контакта по почте и телефону
        var contact = await _db.Contacts
            .FirstOrDefaultAsync(c => c.email == dto.email && c.phone == dto.phone);

        if (contact == null)
        {
            contact = new Contact
            {
                name = dto.name,
                email = dto.email,
                phone = dto.phone
            };
            _db.Contacts.Add(contact);
            await _db.SaveChangesAsync();
        }

        // Проверка темы сообщения
        var topicExists = await _db.MessageTopics.AnyAsync(t => t.id == dto.topic_id);
        if (!topicExists)
        {
            return BadRequest(new { error = "Invalid topic" });
        }

        var message = new Message
        {
            contact_id = contact.id,
            topic_id = dto.topic_id,
            text = dto.text,
            created_at = DateTime.UtcNow
        };
        _db.Messages.Add(message);
        await _db.SaveChangesAsync();

        var response = new MessageResponseDto
        {
            id = message.id,
            contact_id = contact.id,
            topic_id = message.topic_id,
            text = message.text,
            created_at = message.created_at
        };

        return CreatedAtAction(nameof(Create), new { id = response.id }, response);
    }
}








