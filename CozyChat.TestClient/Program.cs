using System;
using CozyChat.TestClient.CozyChatProxy;

namespace CozyChat.TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var proxy = new CozyChatServiceClient())
            {
                proxy.RegisterUserAsync("ieaglle", "password")
                    .ContinueWith(res =>
                {
                    var a = res.Result;
                });

                Console.Read();
            }
        }
    }
}
