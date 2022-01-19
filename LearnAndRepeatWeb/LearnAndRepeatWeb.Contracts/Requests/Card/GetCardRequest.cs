namespace LearnAndRepeatWeb.Contracts.Requests.Card
{
    public class GetCardRequest
    {
        public long? Id { get; set; }
        public string Header { get; set; }
        public string Tag { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
