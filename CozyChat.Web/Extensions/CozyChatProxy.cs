using CozyChat.Service;

namespace CozyChat.Web.Extensions
{
    public class CozyChatProxy : ServiceProxyBase<ICozyChatService>
    {
        public CozyChatProxy()
            : base("net.tcp://localhost:3939/CozyChat")
        {
        }
    }
}