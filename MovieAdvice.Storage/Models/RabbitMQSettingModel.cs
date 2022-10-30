namespace MovieAdvice.Storage.Models
{
    public class RabbitMQSettingModel
    {
        public bool Enabled { get; set; }
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
