namespace ZHello.MQ
{
    public interface IXSubClient : ISocket
    {
        bool Subscribe(string prefix, out string error);

        string RecvData(out string error);
    }

    public interface IXPubServer : ISocket
    {
        bool Publish(string str, out string error);
    }
}