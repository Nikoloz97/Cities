namespace Cities.API.Services
{
    public class CloudMailService : IMailService
    {
        private readonly string _mailTo = string.Empty;
        private readonly string _mailFrom = string.Empty;

        // Assign class attributes to configuration (see appsettings.json)
        public CloudMailService(IConfiguration configuration)
        {
            _mailTo = configuration["mailSettings:mailToAddress"];
            _mailFrom = configuration["mailSettings:mailFromAddress"];

        }

        public void Send(string subject, string message)
        {
            // Pretend to send mail - output to console
            Console.WriteLine($"Mail from {_mailFrom} to {_mailTo}, " + $" with {nameof(CloudMailService)}.");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }

    }
}
