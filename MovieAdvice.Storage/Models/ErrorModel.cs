namespace MovieAdvice.Storage.Models
{
    public class Error
    {
        public Error()
        {
        }
        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }
        public Error(string code, string message, string data)
        {
            Code = code;
            Message = message;
            Data = data;
        }
        public string Code { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }
    }
}
