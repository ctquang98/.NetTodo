namespace TodoProject.Models.DTOs
{
    public class CardAddLabelRequest
    {
        public string LabelId { get; set; }
        public int Priority { get; set; } = 1;
    }
}
