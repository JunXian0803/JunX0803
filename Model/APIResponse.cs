namespace Account.Model
{
    public class APIResponse
    {

        public string? Status { get; set; }

        public int Code { get; set; }

        public string? Message { get; set; }

        public string? Data { get; set; }

        public string? Errors { get; set; }
    }
}
