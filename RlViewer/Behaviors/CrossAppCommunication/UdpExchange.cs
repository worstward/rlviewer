using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace RlViewer.Behaviors.CrossAppCommunication
{

    /// <summary>
    /// Provides cross app event based communication
    /// </summary>
    public class UdpExchange
    {
        public UdpExchange(IPEndPoint endPoint, CommunicationType type)
        {
            _endPoint = endPoint;
            SetCommunicationTypeType(endPoint, type);
        }

        public virtual event GotDataEventHandler DataReceived = delegate { };

        private IPEndPoint _endPoint;
        private UdpClient _udpClient;
        public UdpClient UdpClient
        {
            get { return _udpClient; }
        }

        public void ListenAndNotify()
        {
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    var data = await GetDataAsync();
                    DataReceived(null, new GotDataEventArgs(data));
                }
            });
        }


        public async Task<byte[]> GetDataAsync()
        {
            return (await _udpClient.ReceiveAsync()).Buffer;
        }

        public byte[] GetData(IPEndPoint ipEp)
        {
            return _udpClient.Receive(ref ipEp);
        }

        public async void SendDataAsync(byte[] dataBytes)
        {
            await _udpClient.SendAsync(dataBytes, dataBytes.Length, _endPoint);
        }

        public void SendData(byte[] dataBytes)
        {
            _udpClient.Send(dataBytes, dataBytes.Length, _endPoint);
        }

        private void SetCommunicationTypeType(IPEndPoint ep, CommunicationType type)
        {
            switch (type)
            {
                case CommunicationType.Multicast:
                    StartAsMulticast(ep);
                    break;
                default:
                    throw new NotSupportedException("CommunicationType");
            }
        }


        private void StartAsMulticast(IPEndPoint multicastEp)
        {
            _udpClient = GetMulticastClient(multicastEp);
        }


        private UdpClient GetMulticastClient(IPEndPoint multicastEp)
        {
            var uc = new UdpClient();

            uc.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 2);
            uc.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            uc.ExclusiveAddressUse = false;
            uc.MulticastLoopback = true;
            uc.JoinMulticastGroup(_endPoint.Address);
            uc.Client.Bind(new IPEndPoint(IPAddress.Any, multicastEp.Port));

            return uc;
        }

    }

   


}
