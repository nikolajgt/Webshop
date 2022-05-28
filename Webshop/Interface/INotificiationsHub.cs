namespace Webshop.Interface
{
    public interface INotificiationsHub
    {
        Task SendNotificationsOut(string message);
        Task SendUserinformationOut(string connectionid, string username);
    }
}
