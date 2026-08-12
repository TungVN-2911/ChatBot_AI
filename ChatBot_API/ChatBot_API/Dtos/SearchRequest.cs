namespace ChatBot_API.Dtos
{
    public class SearchRequest
    {
        public string Question { get; set; } = string.Empty;
        public int TopK { get; set; } = 5;
    }
}
