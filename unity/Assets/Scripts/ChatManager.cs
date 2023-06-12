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
    Thread backgroundThread;
    public bool newchat = true;
    public TMP_InputField chatInput;
    public TMP_InputField chatOutput;
    List<String> chatList = new List<string>();
    // Start is called before the first frame update
    public GameObject gamestate;
    gameManager gameManager;
    public TMP_Text RoomID;
    private const string serverAddress = "127.0.0.1";
    private const int serverPort = 3004;
    NetworkStream stream;
    TcpClient client = new TcpClient();
    // Start is called before the first frame update
    async void Awake()
    {

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
        backgroundThread = new Thread(new ThreadStart(receiveMessages));
        backgroundThread.Start();

    }

    void OnApplicationQuit()
    {
        backgroundThread.Abort();
    }
    public void Start()
    {
        gamestate = GameObject.Find("sceneManager");
        gameManager = gamestate.GetComponent<gameManager>();
        try
        {
            RoomID.SetText("Room ID = " + gameManager.gameID);
        }
        catch (Exception e)
        {
            Debug.LogError("Error connecting to the server: " + e.Message);

        }
        Debug.Log("start function");

    }
    public void Update()
    {
        if (newchat)
        {
            Showmessages();
            newchat = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (chatInput.isFocused == false)
            {
                chatInput.ActivateInputField();
                chatInput.Select();
                chatList.Add("ahmed mohamed");
            }
            else
            {
                string senddata = "/Message," + chatInput.text.Trim();
                if (chatInput.text.Trim() != "")
                {
                    sendata(senddata);
                    gameManager.chat.Add("YOU :: " + chatInput.text.Trim());

                }
                chatInput.text = "";
                chatInput.DeactivateInputField();
            }
        }

    }
    public void sendata(String responseData)
    {
        Byte[] data = new Byte[1024];
        try
        {
            Debug.Log("sending data from unity to python");
            byte[] messageBytes = System.Text.Encoding.ASCII.GetBytes(responseData);
            stream.Write(messageBytes, 0, messageBytes.Length);
            newchat = true;
        }
        catch (Exception e)
        {
            Debug.LogError("Error connecting to the server: " + e.Message);
        }

    }
    public void Showmessages()
    {
        string allchat = string.Empty;
        string [] tempchat= gameManager.chat.ToArray();
        for(int i = tempchat.Length-1;i>=0;i--)
            allchat += tempchat[i] + "\n";
        chatOutput.text = allchat;
    }

    public void startgame()
    {
        gameManager.startgame();
    }
    public void receiveMessages()
    {
        int counter = 0;
        String responseData = String.Empty;
        Byte[] data = new Byte[1024];
        while (true)
        {
            try
            {
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                if (responseData.Split(',', 2)[0] == "/Msg")
                {
                    Debug.Log("Message received from another server" + responseData);
                    gameManager.chat.Add(responseData.Split(',', 2)[1]);
                    newchat = true;
                }

            }
            catch (Exception e)
            {
                counter++;
                Debug.LogError("Error connecting to the server: " + e.Message);
                if (counter > 2000)
                    break;
            }

        }
    }
}
