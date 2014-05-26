using System.Configuration;
using CozyChat.Service;

namespace CozyChat.Web.Extensions
{
    public class CozyChatProxy : ServiceProxyBase<ICozyChatService>
    {
        public CozyChatProxy()
            : base(ConfigurationManager.AppSettings["uri"])
        {
        }
    }
}