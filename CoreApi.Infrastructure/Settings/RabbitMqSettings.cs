namespace CoreApi.Infrastructure.Settings
{
    public class RabbitMqSettings
    {
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string PendingTreatmentRoutingKey { get; set; }
        public string DoneTreatmentRoutingKey { get; set; }
        public string TreatmentExchange { get; set; }
    }
}