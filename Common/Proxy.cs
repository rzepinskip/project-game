using System;
using Common.Interfaces;

namespace Common
{
    public class Proxy : IProxy
    {
        private IClient Client { get; }
        public event Action<IMessage> MessageReceivedEvent;

        public Proxy(IClient client)
        {
            Client = client;
            //Client.SetupClient(HandleStringMessage);
        }

        public void Send(IMessage message)
        {
            string content = "";// = message.SerializeToString();
            //Client.Send(content);
        }

        public void HandleStringMessage(string message)
        {
            //receives string message
            //proccess it
            //activate an event

            //MessageReceivedEvent?.Invoke(new MoveRequest());
        }

        public void SubscribeProxy(Action<IMessage> messageHandler)
        {
            //Player or GM can pass its handle message function
            MessageReceivedEvent += messageHandler;
        }


    }
}
