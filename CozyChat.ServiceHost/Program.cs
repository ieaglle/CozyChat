using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using CozyChat.Service;

namespace CozyChat.ServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new System.ServiceModel.ServiceHost(typeof(CozyChatService), 
                new Uri("net.tcp://localhost:3939/CozyChat")))
            {
                var binding = new NetTcpBinding();
                host.AddServiceEndpoint(typeof (ICozyChatService), binding, "");

                var metadata = new ServiceMetadataBehavior
                {
                    HttpGetEnabled = false,
                    MetadataExporter = { PolicyVersion = PolicyVersion.Policy15 }
                };

                host.Description.Behaviors.Add(metadata);

                host.AddServiceEndpoint(
                    ServiceMetadataBehavior.MexContractName,
                    MetadataExchangeBindings.CreateMexTcpBinding(),
                    "mex");

                host.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
                host.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

                host.Open();

                Console.WriteLine("Started");

                Console.Read();
            }
        }
    }
}
