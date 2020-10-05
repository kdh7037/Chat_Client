using System.net.Sockets;
using System.Net;

static void Main(string[] args){
    Socket transferSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
    transferSock.Connect(new IPEndPoint(IPAddress.Loopback, 10801));

    transferSock.BeginReceive(receiveBytes, 0, 11, SocketFlags.None, new AsyncCallback(receiveStr), transferSock);
}

static void receiveStr(IAsyncResult ar){
    Socket transferSock = (Socket)ar.AsyncState;
    int strLength = transferSock.EndReceive(ar);
    Console.WriteLine(Encoding.Default.GetString(receiveBytes));
}