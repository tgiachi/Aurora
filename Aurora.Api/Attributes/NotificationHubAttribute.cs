namespace Aurora.Api.Attributes
{
    public enum NotificationHubType
    {
        Email,
        WhatsApp
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class NotificationHubAttribute : Attribute
    {
        public NotificationHubAttribute(NotificationHubType hubType)
        {
            HubType = hubType;
        }

        public NotificationHubType HubType { get; set; }
    }
}
