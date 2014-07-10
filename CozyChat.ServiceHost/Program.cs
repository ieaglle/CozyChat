using System;
using System.Configuration;
using System.ServiceModel;
using CozyChat.Service;

namespace CozyChat.ServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            //NOTE: Manual way
            //using (var host = new System.ServiceModel.ServiceHost(typeof(CozyChatService),
            //    new Uri("net.tcp://localhost:3939/CozyChat")))
            //{
            //    var binding = new NetTcpBinding();
            //    host.AddServiceEndpoint(typeof(ICozyChatService), binding, "");

            //    host.Open();

            //    Console.WriteLine("Started");
            //    Console.Read();
            //}

            //NOTE: Configuration file way 
            using (var host = new System.ServiceModel.ServiceHost(typeof(CozyChatService)))
            {
                host.Open();

                Console.WriteLine("Started");
                Console.Read();
            }
        }
    }
}
