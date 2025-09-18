using FeedbackApi.Data;
using FeedbackApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FeedbackApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TopicsController : ControllerBase
{
    private readonly AppDbContext _db;
    public TopicsController(AppDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MessageTopic>>> Get()
    {
        var topics = await _db.MessageTopics.AsNoTracking().OrderBy(t => t.id).ToListAsync();
        return Ok(topics);
    }
}
