using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Chat_Client
{
    class Chat_Client2
    {
        public class AsyncObject{
            public Byte[] Buffer;
            public Socket WorkingSocket;
            public AsyncObject(Int32 bufferSize){
                this.Buffer = new byte[bufferSize];
            }
        }

        private Boolean g_Connected;
        private Socket m_ClientSocket = null;
        private AsyncCallback m_fnReceiveHandler;
        private AsyncCallback m_fnSendHandler;

        public Chat_Client(){
            m_fnReceiveHandler = new AsyncCallback(handleDataReceive);
            m_fnSentHandler = new AsyncCallback(handleDataSend);
        }

        public Boolean Connected {
            get {
                return g_Connected;
            }
        }

        public void ConnectToServer(String hostName, UInt16 hostPort){
            m_ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            Boolean isConnected = false;
            try{
                m_ClientSocket.Connect(hostName, hostPort);
                isConnected = true;
            } catch {
                isConnected = false;
            }
            g_Connected = isConnected;

            if(isConnected){
                AsyncObject ao = new AsyncObject(4096);
                ao.WorkingSocket = m_ClientSocket;
                m_ClientSocket.BeginReceive(ao.Buffer.Length, SocketFlags.None, m_fnReceiveHandler, ao);
                Console.WriteLine("연결됨");
            }
            else{
                Console.WriteLine("연결안됨");
            }
        }
        public void StopClient() {
            m_ClientSocket.Close();
        }

        public void SendMessage(String message){
            AsyncObject ao = new AsyncObject(1);
            ao.Buffer = Encoding.Unicode.GetBytes(message);
            ao.WorkingSocket = m_ClientSocket;

            try{
                m_ClientSocket.BeginSend(ao.Buffer, 0, ao.Buffer.Length, SocketFlags.None, m_fnSendHandler, ao);
            }catch(Exception.ex){
                Console.WriteLine("전송중에러 :: 메세지{0}", ex.Message);
            }
        }

        private void handleDataReceive(IAsyncResult ar){
            AsyncObject ao = (AsyncObject) ar.AsyncState;
            Int32 recvBytes; 
            
            try {
                recvBytes = ao.WorkingSocket.EndReceive(ar);
            } catch {
                return;
            }
         
            if ( recvBytes > 0 ) {
                Byte[] msgByte = new Byte[recvBytes];
                Array.Copy(ao.Buffer, msgByte, recvBytes);
                
                Console.WriteLine("메세지 받음: {0}", Encoding.Unicode.GetString(msgByte));
            }
         
            try {
                ao.WorkingSocket.BeginReceive(ao.Buffer, 0, ao.Buffer.Length, SocketFlags.None, m_fnReceiveHandler, ao);
            } catch (Exception ex) {
                Console.WriteLine("자료 수신 대기 도중 오류 발생! 메세지: {0}", ex.Message);
                return;
            }
        }

        private void handleDataSend(IAsyncResult ar) {
            AsyncObject ao = (AsyncObject) ar.AsyncState;
            Int32 sentBytes;
            
            try {
                sentBytes = ao.WorkingSocket.EndSend(ar);
            } catch (Exception ex) {
                Console.WriteLine("자료 송신 도중 오류 발생! 메세지: {0}", ex.Message);
                return;
            }
         
            if ( sentBytes > 0 ) {
                Byte[] msgByte = new Byte[sentBytes];
                Array.Copy(ao.Buffer, msgByte, sentBytes);
                
                Console.WriteLine("메세지 보냄: {0}", Encoding.Unicode.GetString(msgByte));
            }
        }
    }
}
