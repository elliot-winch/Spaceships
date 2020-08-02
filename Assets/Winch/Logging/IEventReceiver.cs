public interface IEventReceiver : IIdentifiable
{
    void ReceiveEvent(Event evt);
}
