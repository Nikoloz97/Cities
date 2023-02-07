namespace Cities.API.Services
{
    public class LocalMailService : IMailService
    {

        private readonly string _mailTo = string.Empty;
        private readonly string _mailFrom = string.Empty;

        // Assign configuration (see appsettings.json)
        public LocalMailService(IConfiguration configuration)
        {
            // Pass through the key of JSON object
            _mailTo = configuration["mailSettings:mailToAddress"];
            _mailFrom = configuration["mailSettings:mailFromAddress"];

        }

        public void Send(string subject, string message)
        {
            // Pretend to send mail - output to console
            Console.WriteLine($"Mail from {_mailFrom} to {_mailTo}, " + $" with {nameof(LocalMailService)}.");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }
    }
}
