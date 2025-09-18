namespace FeedbackApi.Models;

public class Contact
{
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public string phone { get; set; } = string.Empty;
}

public class MessageTopic
{
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
}

public class Message
{
    public int id { get; set; }
    public int contact_id { get; set; }
    public Contact? contact { get; set; }

    public int topic_id { get; set; }
    public MessageTopic? topic { get; set; }

    public string text { get; set; } = string.Empty;
    public DateTime created_at { get; set; }
}








