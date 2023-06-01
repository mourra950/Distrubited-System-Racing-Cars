using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Threading.Tasks;
public class CarSocket : MonoBehaviour
{
    private const string serverAddress = "127.0.0.1";
    private const int serverPort = 3002;
    NetworkStream stream;
    TcpClient client;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            client = new TcpClient(serverAddress, serverPort);

            // Get the network stream for sending data
            stream = client.GetStream();

            // Convert the message to bytes


            // Clean up
            //stream.Close();
            // client.Close();
        }
        catch (Exception e)
        {
            Debug.LogError("Error connecting to the server: " + e.Message);
        }
    }

    // Update is called once per frame
    async void Update()
    {
        string coords = this.transform.position.x + "," + this.transform.position.z;

        byte[] messageBytes = System.Text.Encoding.ASCII.GetBytes(coords);
        await WaitOneSecondAsync();
        try
        {
            stream.Write(messageBytes, 0, messageBytes.Length);
        }
        catch (Exception e)
        {
            Debug.LogError("Error connecting to the server: " + e.Message);
        }
        // Send the message to the server
        Debug.Log(DateTime.Now.ToString("HH:mm:ss"));


    }

    private async Task WaitOneSecondAsync()
    {
        await Task.Delay(TimeSpan.FromMilliseconds(500));
    }
}
