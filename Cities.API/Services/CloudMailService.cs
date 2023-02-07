namespace Cities.API.Services
{
    public class CloudMailService : IMailService
    {
        // Dummy property information
        private string _mailTo = "admin@mycompany.com";
        private string _mailFrom = "noreply@mycompany.com";


        public void Send(string subject, string message)
        {
            // Pretend to send mail - output to console
            Console.WriteLine($"Mail from {_mailFrom} to {_mailTo}, " + $" with {nameof(CloudMailService)}.");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }

    }
}
