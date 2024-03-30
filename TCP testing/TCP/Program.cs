using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class TCPServer {
    static void Main(string[] args) {
        int port = 12345;

        TcpListener listener = new TcpListener(IPAddress.Any, port);
        listener.Start();

        Console.WriteLine("Server listening on port " + port);

        TcpClient client = listener.AcceptTcpClient();
        Console.WriteLine("Connection from " + ((IPEndPoint)client.Client.RemoteEndPoint).Address);

        NetworkStream stream = client.GetStream();

        while (true) {
            byte[] data = new byte[1024];
            int bytesRead = stream.Read(data, 0, data.Length);

            // Check if no data is received (indicating client has closed the connection)
            if (bytesRead == 0) {
                Console.WriteLine("Client closed the connection.");
                break;
            }

            string message = Encoding.ASCII.GetString(data, 0, bytesRead);
            Console.WriteLine("Received: " + message);
        }

        // Close the connection
        client.Close();
        listener.Stop();
    }
}
