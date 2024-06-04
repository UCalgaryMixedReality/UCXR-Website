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

            if (bytesRead == 0) {
                Console.WriteLine("Client closed the connection.");
                break;
            }

            string message = Encoding.ASCII.GetString(data, 0, bytesRead);
            
            // Parse the received message based on its structure
            if (message.Contains(".")) {
                // If the message contains a ".", it's likely a decimal number (distance between hands)
                double distance = double.Parse(message);
                Console.WriteLine("Current Gesture: Zoom");
                Console.WriteLine("Distance between hands: " + distance);
            } else if (message.StartsWith("[")) {
                // If the message starts with "[", it's likely a list of coordinates
                // Remove "[" and "]" characters and split the message by ","
                
                string[] coordinates = message.Trim('[', ']').Split(',');
                
                int[] parsedCoordinates = Array.ConvertAll(coordinates, int.Parse);
                
                if (parsedCoordinates.Length == 4) {
                    // If there are 4 coordinates, it represents two pointer fingers
                    Console.WriteLine("Current Gesture: Point");
                    Console.WriteLine("Coordinates of two pointer fingers: (" + parsedCoordinates[0] + ", " + parsedCoordinates[1] + "), (" + parsedCoordinates[2] + ", " + parsedCoordinates[3] + ")");
                } else if (parsedCoordinates.Length == 2) {
                    // If there are 2 coordinates, it represents one pointer finger
                    Console.WriteLine("Current Gesture: Point");
                    Console.WriteLine("Coordinates of one pointer finger: (" + parsedCoordinates[0] + ", " + parsedCoordinates[1] + ")");
                } 
            } 
            
            else {
                // Handle other types of messages or unknown formats
                Console.WriteLine("Unknown message format: " + message);
            }
        }

        client.Close();
        listener.Stop();
    }
}
