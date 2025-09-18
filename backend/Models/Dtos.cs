using System.ComponentModel.DataAnnotations;

namespace FeedbackApi.Models;

public class MessageCreateDto
{
    private const string EmailPattern = @"^[A-Za-z0-9._-]+@[a-z0-9-]+\.[a-z]{2,}$";
    private const string PhonePattern = @"^\+7 \(\d{3}\) \d{3}-\d{2}-\d{2}$";

    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    public string name { get; set; } = string.Empty;

    [Required]
    [MaxLength(254)]
    [RegularExpression(EmailPattern, ErrorMessage = "Неверный формат email")] 
    public string email { get; set; } = string.Empty;

    [Required]
    [RegularExpression(PhonePattern, ErrorMessage = "Неверный формат телефона")] 
    public string phone { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int topic_id { get; set; }

    [Required]
    [MinLength(5)]
    [MaxLength(4000)]
    public string text { get; set; } = string.Empty;

    public string? captchaToken { get; set; }
}

public class MessageResponseDto
{
    public int id { get; set; }
    public int contact_id { get; set; }
    public int topic_id { get; set; }
    public string text { get; set; } = string.Empty;
    public DateTime created_at { get; set; }
}








