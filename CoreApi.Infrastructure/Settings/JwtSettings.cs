namespace CoreApi.Infrastructure.Settings
{
    public class JwtSettings
    {
        public string? Key { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }

        public int? MinutesDuration { get; set; }
        
        public string? Algorithm { get; set; }
    }
}