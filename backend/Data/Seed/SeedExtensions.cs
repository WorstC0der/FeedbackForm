using FeedbackApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FeedbackApi.Data.Seed;

public static class SeedExtensions
{
    public static void SeedInitial(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MessageTopic>().HasData(
            new MessageTopic { id = 1, name = "Техподдержка" },
            new MessageTopic { id = 2, name = "Продажи" },
            new MessageTopic { id = 3, name = "Другое" },
            new MessageTopic { id = 4, name = "Еще один пункт" }
        );
    }
}






