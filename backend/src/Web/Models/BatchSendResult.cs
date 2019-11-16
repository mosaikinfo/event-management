namespace EventManagement.WebApp.Models
{
    public class BatchSendResult
    {
        public bool DryRun { get; set; }
        public int MailsSent { get; set; }
        public int TicketsWithoutEmailAddress { get; set; }
    }
}