using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using UnityEngine.SceneManagement;
using System.Net.Sockets;
using UnityEngine;
using System.Threading.Tasks;
using TMPro;

public class gameManager : MonoBehaviour
{
    public TMP_InputField RoomID;
    private const string serverAddress = "127.0.0.1";
    private const int serverPort = 3003;
    NetworkStream stream;
    bool connectionSuccess = false;
    TcpClient client = new TcpClient();
    public int counter;
    // Start is called before the first frame update
    async void Awake()
    {

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
        Debug.Log("Connected to the server");
        connectionSuccess = true;
        stream = client.GetStream();
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    public void Update()
    {
        if (connectionSuccess)
        {
            String responseData = String.Empty;
            Byte[] data = new Byte[256];
            try
            {
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                byte[] messageBytes = System.Text.Encoding.ASCII.GetBytes("OK");
                stream.Write(messageBytes, 0, messageBytes.Length);
                Debug.Log(responseData);

            }
            catch (Exception e)
            {
                Debug.LogError("Error connecting to the server: " + e.Message);
            }

        }
        // Send the message to the server
    }

    public void CreateGame()
    {

        SceneManager.LoadScene(1, LoadSceneMode.Single);

    }

    public void JoinGame()
    {
        Debug.Log(RoomID.text);

        // SceneManager.LoadScene(1, LoadSceneMode.Single);

    }
    private async Task WaitOneSecondAsync()
    {
        await Task.Delay(TimeSpan.FromMilliseconds(2000));
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
    public void receivedata(String responseData)
    {
        Byte[] data = new Byte[1024];
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
