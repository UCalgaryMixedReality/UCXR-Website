using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.PackageManager;

public class TC : MonoBehaviour
{
    private TcpListener listener;
    private TcpClient client;
    private bool isRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        StartServer(12345);
    }

    // Update is called once per frame
    void Update()
    {

    }

    async void StartServer(int port)
    {
        try
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            isRunning = true;
            Debug.Log("Server listening on port " + port);

            while (isRunning)
            {
                client = await listener.AcceptTcpClientAsync();
                Debug.Log("Connection from " + ((IPEndPoint)client.Client.RemoteEndPoint).Address);

                NetworkStream stream = client.GetStream();
                _ = Task.Run(() => HandleClient(stream));
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
    }

    async void HandleClient(NetworkStream stream)
    {
            byte[] data = new byte[1024];
            while (isRunning)
            {
                try
                {
                    int bytesRead = await stream.ReadAsync(data, 0, data.Length);

                    if (bytesRead == 0)
                    {
                        Debug.Log("Client closed the connection.");
                        break;
                    }

                    string message = Encoding.ASCII.GetString(data, 0, bytesRead);

                    // Parse the received message based on its structure
                    if (message.Contains("."))
                    {
                        // If the message contains a ".", it's likely a decimal number (distance between hands)
                        double distance = double.Parse(message);
                        Debug.Log("Current Gesture: Zoom");
                        Debug.Log("Distance between hands: " + distance);
                    }
                    else if (message.StartsWith("["))
                    {
                        // If the message starts with "[", it's likely a list of coordinates
                        // Remove "[" and "]" characters and split the message by ","
                        string[] coordinates = message.Trim('[', ']').Split(',');

                        int[] parsedCoordinates = Array.ConvertAll(coordinates, int.Parse);

                        if (parsedCoordinates.Length == 4)
                        {
                            // If there are 4 coordinates, it represents two pointer fingers
                            Debug.Log("Current Gesture: Point");
                            Debug.Log("Coordinates of two pointer fingers: (" + parsedCoordinates[0] + ", " + parsedCoordinates[1] + "), (" + parsedCoordinates[2] + ", " + parsedCoordinates[3] + ")");
                        }
                        else if (parsedCoordinates.Length == 2)
                        {
                            // If there are 2 coordinates, it represents one pointer finger
                            Debug.Log("Current Gesture: Point");
                            Debug.Log("Coordinates of one pointer finger: (" + parsedCoordinates[0] + ", " + parsedCoordinates[1] + ")");
                        }
                    }
                    else
                    {
                        // Handle other types of messages or unknown formats
                        Debug.Log("Unknown message format: " + message);
                    }
                }

                catch (Exception e)
            {
                Console.WriteLine("Caught error: " + e);
                continue;
            }
            }
        }
    private void OnDestroy()
    {
        StopServer();
    }

    void StopServer()
    {
        isRunning = false;
        if (client != null)
        {
            client.Close();
        }
        if (listener != null)
        {
            listener.Stop();
        }
    }
}