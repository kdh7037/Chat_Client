using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

namespace Chat_Client
{
    class Program
    {
        public class StateObject{
            public Socket workSocket = null;
            public const int BufferSize = 256;
            public byte[] buffer = new byte[BufferSize];
            public StringBuilder sb = new StringBuilder();
        }

        public class AsynClient{
            private const int port = 9999;

            private static ManualResetEvent connectDone = new ManualResetEvent(false);  
            private static ManualResetEvent sendDone = new ManualResetEvent(false);  
            private static ManualResetEvent receiveDone = new ManualResetEvent(false); 
            private static String response = String.Empty;

            private static void StartClient(){
                
            }

            private static void ConnectCallback(IAsyncResult ar){

            }
        }
        

    }
}
