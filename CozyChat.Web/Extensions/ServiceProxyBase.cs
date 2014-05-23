using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace CozyChat.Web.Extensions
{
    public class ServiceProxyBase<T> : IDisposable where T : class
    {
        private readonly string _endpointUri;
        private readonly object _sync = new object();
        private IChannelFactory<T> _channelFactory;
        private bool _disposed;

        private T _channel;
        public T Channel
        {
            get
            {
                Initialize();
                return _channel;
            }
        }

        protected ServiceProxyBase(string endpointUri)
        {
            _endpointUri = endpointUri;
        }

        protected void CloseChannel()
        {
            if (_channel != null)
                ((ICommunicationObject)_channel).Close();
        }

        private void Initialize()
        {
            lock (_sync)
            {
                if (_channel != null) return;

                _channelFactory = new ChannelFactory<T>(new NetTcpBinding());
                _channel = _channelFactory.CreateChannel(new EndpointAddress(_endpointUri));
            }
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposeManaged)
        {
            if (_disposed) return;

            if (disposeManaged)
            {
                lock (_sync)
                {
                    CloseChannel();

                    if (_channelFactory != null)
                        ((IDisposable)_channelFactory).Dispose();

                    _channel = null;
                    _channelFactory = null;
                }
            }

            _disposed = true;
        }

        #endregion
    }
}