using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Behaviors.CrossAppCommunication
{
    public interface ICrossAppExchange
    {
        void SetUp();

        byte[] ReceiveData(System.Net.IPEndPoint ipEp);
        Task<byte[]> ReceiveDataAsync();

        void SendData(byte[] msg);
        void SendDataAsync(byte[] msg);

        event GotDataEventHandler DataReceived;

        void Dispose();
    }
}
