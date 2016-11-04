using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace RlViewer.Behaviors.CrossAppCommunication.PointSharer
{
    public sealed class MulticastPointSharer : IDisposable
    {

        /// <summary>
        /// Creates MulticastPointSharer class instance
        /// </summary>
        /// <param name="multicastEp">Multicast ip endpoint</param>
        /// <param name="guid">Application id</param>
        /// <param name="shiftX">Image shift by X axis</param>
        /// <param name="shiftY">Image shift by Y axis</param>
        /// <param name="triggeredAction">Action to perform when event triggered</param>
        public MulticastPointSharer(ICrossAppExchange server, int guid, int shiftX, int shiftY)
        {
            server.SetUp();
            _server = server;

            _shiftX = shiftX;
            _shiftY = shiftY;
            _guid = guid;
        }


        private ICrossAppExchange _server;
        private int _guid;
        private int _shiftX;
        private int _shiftY;

        public event GotDataEventHandler DataReceived
        {
            add
            {
                _server.DataReceived += value;
            }
            remove
            {
                _server.DataReceived -= value;
            }
        }


        public void ProcessMessage(byte[] receivedMessage, Action<System.Drawing.Point> callBack)
        {
            var messageStruct = Behaviors.Converters.StructIO.ReadStruct<PointSharerMessage>(receivedMessage);
            if (messageStruct.Guid != _guid)
            {
                var shared = new System.Drawing.Point(messageStruct.X - _shiftX, messageStruct.Y - _shiftY);
                callBack(shared);
            }
        }


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
            _server.Dispose();
        }


        private struct PointSharerMessage
        {
            public PointSharerMessage(int guid, int x, int y)
            {
                _guid = guid;
                _x = x;
                _y = y;
            }

            private int _guid;
            public int Guid
            {
                get { return _guid; }
                private set { _guid = value; }
            }

            private int _x;
            public int X
            {
                get { return _x; }
                private set { _x = value; }
            }

            private int _y;
            public int Y
            {
                get { return _y; }
                private set { _y = value; }
            }

        }


    }
}
