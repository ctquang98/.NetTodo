namespace TodoProject.Models.DTOs
{
    public class FilterParams
    {
        public string FilterFieldName { get; set; } = "";
        public string FilterFieldValue { get; set; } = "";
        public Boolean SortAsc { get; set; } = true;
        public string SortFieldName { get; set; } = "";
        public int Page {  get; set; } = 1;
        public int PageSize { get; set; } = 5;

    }
}
