using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace RlViewer.Behaviors.CrossAppCommunication.PointSharer
{
    public class MulticastPointSharer : IDisposable
    {

        /// <summary>
        /// Creates MulticastPointSharer class instance
        /// </summary>
        /// <param name="multicastEp">Multicast ip endpoint</param>
        /// <param name="guid">Application id</param>
        /// <param name="shiftX">Image shift by X axis</param>
        /// <param name="shiftY">Image shift by Y axis</param>
        /// <param name="triggeredAction">Action to perform when event triggered</param>
        public MulticastPointSharer(IPEndPoint multicastEp, int guid, int shiftX, int shiftY,
            Action<System.Drawing.Point> triggeredAction)
        {
            _triggeredAction = triggeredAction;
            _server = new UdpExchange(multicastEp, CommunicationType.Multicast);
            _server.ListenAndNotify();

            _shiftX = shiftX;
            _shiftY = shiftY;
            _guid = guid;

            _server.DataReceived += (s, e) =>
            {
                var receivedMessage = Converters.StructIO.ReadStruct<PointSharerMessage>(e.Data);
                if (receivedMessage.guid != guid)
                {
                    SharedPoint = new System.Drawing.Point(receivedMessage.x - _shiftX, receivedMessage.y - _shiftY);
                    triggeredAction(SharedPoint);
                }
            };

        }


        

        private UdpExchange _server;
        private int _guid;
        private int _shiftX;
        private int _shiftY;
        private Action<System.Drawing.Point> _triggeredAction;


        public System.Drawing.Point SharedPoint
        {
            get;
            private set;
        }


        /// <summary>
        /// Sends point on image to remote host
        /// </summary>
        public void SendPoint(System.Drawing.Point pointToSend)
        {
            var msg = new PointSharerMessage(_guid, _shiftX + pointToSend.X, _shiftY + pointToSend.Y);
            var msgBytes = Behaviors.Converters.StructIO.WriteStruct<PointSharerMessage>(msg);
            _server.SendDataAsync(msgBytes);
        }

        public void Dispose()
        {
            _server.UdpClient.Close();
        }


        private struct PointSharerMessage
        {
            public PointSharerMessage(int guid, int x, int y)
            {
                this.guid = guid;
                this.x = x;
                this.y = y;
            }

            public int guid;
            public int x;
            public int y;
        }


    }
}
