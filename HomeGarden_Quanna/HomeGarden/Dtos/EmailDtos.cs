namespace HomeGarden.Dtos
{
    public class EmailTemplateDto
    {
        public int TemplateId { get; set; }
        public string Code { get; set; }
        public string Subject { get; set; }
        public string? Description { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class EmailSendDto
    {
        public long UserId { get; set; }
        public string Subject { get; set; } = "";
        public string Content { get; set; } = "";
    }
}
