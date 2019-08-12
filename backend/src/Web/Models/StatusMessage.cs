namespace EventManagement.WebApp.Models
{
    public class StatusMessage
    {
        public string Message { get; set; }

        public string BackgroundCssClass { get; set; } = "bg-success";

        public string IconCssClass { get; set; } = "far fa-check-square";
    }
}
