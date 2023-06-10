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
    public string gameID = "123412s";
    private const string serverAddress = "127.0.0.1";
    private const int serverPort = 3003;
    NetworkStream Receivestream;
    NetworkStream Sendstream;

    bool connectionSuccess = false;
    TcpClient unitySend = new TcpClient();
    TcpClient unityReceive = new TcpClient();

    async void Awake()
    {

        Debug.Log("Looking for a server");
        while (true)
        {
            try
            {
                await unityReceive.ConnectAsync(serverAddress, 3002);
                break;
            }
            catch (Exception e)
            {
                Debug.LogError("Error connecting to the receive sender: " + e.Message);
            }
        }
        while (true)
        {
            try
            {
                await unitySend.ConnectAsync(serverAddress, 3003);
                break;
            }
            catch (Exception e)
            {
                Debug.LogError("Error connecting to the Send server: " + e.Message);
            }
        }

        Debug.Log("Connected to the two servers python success");
        connectionSuccess = true;
        Receivestream = unityReceive.GetStream();
        Sendstream = unitySend.GetStream();

        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    public void Update()
    {
        /*if (connectionSuccess)
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
        */
        // Send the message to the server
    }

    public void CreateGame()
    {
        string responseData;
        Byte[] data = new Byte[1024];


        try
        {
            Debug.Log("sending data from unity to python");
            byte[] messageBytes = System.Text.Encoding.ASCII.GetBytes("/Create,a");
            Sendstream.Write(messageBytes, 0, messageBytes.Length);
        }
        catch (Exception e)
        {
            Debug.LogError("unable to send");
        }

        try
        {
            Int32 bytes = Receivestream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Debug.Log("Created a game with code" + responseData);
            gameID = responseData;
        }
        catch (Exception e)
        {
            Debug.Log("error in receiving");
        }

        SceneManager.LoadScene(1, LoadSceneMode.Single);


    }

    public void JoinGame()
    {
        Debug.Log("sending data from unity to python");

        string responseData;
        Byte[] data = new Byte[1024];
        try
        {
            Debug.Log("sending data from unity to python");
            responseData = "/Join," + RoomID.text.Trim();
            byte[] messageBytes = System.Text.Encoding.ASCII.GetBytes(responseData);
            Debug.Log(responseData);

            if (RoomID.text == "")
            {
                return;
            }
            Sendstream.Write(messageBytes, 0, messageBytes.Length);
        }
        catch (Exception e)
        {
            Debug.LogError("unable to send");
        }

        try
        {
            Int32 bytes = Receivestream.Read(data, 0, data.Length);
            Debug.Log("receiv data from unity to python to join room");

            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            if (responseData == "false")
            {
                return;
            }
            bytes = Receivestream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            gameID = responseData;
        }
        catch (Exception e)
        {
            Debug.Log("error in receiving");
        }

        Debug.Log(RoomID.text);
        SceneManager.LoadScene(1, LoadSceneMode.Single);

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
            Sendstream.Write(messageBytes, 0, messageBytes.Length);
            Int32 bytes = Receivestream.Read(data, 0, data.Length);
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
            Int32 bytes = Receivestream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Debug.Log(responseData);
            byte[] messageBytes = System.Text.Encoding.ASCII.GetBytes("OK");
            Sendstream.Write(messageBytes, 0, messageBytes.Length);
        }
        catch (Exception e)
        {
            Debug.LogError("Error connecting to the server: " + e.Message);
        }

    }
}
