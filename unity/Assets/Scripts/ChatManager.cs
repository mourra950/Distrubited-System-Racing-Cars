using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using UnityEngine.SceneManagement;
using System.Net.Sockets;
using UnityEngine;
using System.Threading.Tasks;
using TMPro;
using System.Threading;

public class ChatManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject gamestate;
    gameManager gameManager;
    public TMP_InputField RoomID;
    private const string serverAddress = "127.0.0.1";
    private const int serverPort = 3004;
    NetworkStream stream;
    TcpClient client = new TcpClient();
    // Start is called before the first frame update
    async void Awake()
    {
        gameManager = gamestate.GetComponent<gameManager>();

        Debug.Log("Looking for a server");
        while (true)
        {
            try
            {
                await client.ConnectAsync(serverAddress, serverPort);
                break;
            }
            catch (Exception e)
            {
                Debug.LogError("Error connecting to the server: " + e.Message);
            }
        }
        stream = client.GetStream();
        DontDestroyOnLoad(this);
        Thread backgroundThread = new Thread(new ThreadStart(receiveMessages));
        backgroundThread.Start();
    }
    public void sendata(String responseData)
    {
        Byte[] data = new Byte[1024];
        try
        {
            byte[] messageBytes = System.Text.Encoding.ASCII.GetBytes(responseData);
            stream.Write(messageBytes, 0, messageBytes.Length);
            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Debug.Log(responseData);

        }
        catch (Exception e)
        {
            Debug.LogError("Error connecting to the server: " + e.Message);
        }

    }
    public void receiveMessages()
    {
        String responseData = String.Empty;
        Byte[] data = new Byte[1024];
        while (true)
        {
            try
            {
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Debug.Log(responseData);
                byte[] messageBytes = System.Text.Encoding.ASCII.GetBytes("OK");
                stream.Write(messageBytes, 0, messageBytes.Length);
            }
            catch (Exception e)
            {
                Debug.LogError("Error connecting to the server: " + e.Message);
            }
        }
    }
}