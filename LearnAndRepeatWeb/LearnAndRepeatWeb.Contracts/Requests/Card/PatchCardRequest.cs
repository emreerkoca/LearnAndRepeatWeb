namespace LearnAndRepeatWeb.Contracts.Requests.Card
{
    public class PatchCardRequest
    {
        public long Id { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public string Tag { get; set; }
    }
}
