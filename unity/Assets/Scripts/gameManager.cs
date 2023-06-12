using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using System.Net.Sockets;
using UnityEngine;
using TMPro;
using System.Threading;
using System.Net;
using System.Threading.Tasks;

public class gameManager : MonoBehaviour
{
    public List<string> playertestlist = new List<string>();
    public List<playercustomclass> playerlist = new List<playercustomclass>();
    public List<string> chat = new List<string>();
    public TMP_InputField RoomID;
    public string gameID = "123412s";
    private const string serverAddress = "127.0.0.1";
    private const int serverPort = 3003;
    NetworkStream Receivestream;
    NetworkStream Sendstream;
    Thread backgroundreceiveThread;

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
        backgroundreceiveThread = new Thread(new ThreadStart(receivedata));
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
        string[] array;
        string responseData;
        Byte[] data = new Byte[1024];


        try
        {
            Debug.Log("sending data from unity to python");
            responseData = "/Create," + RoomID.text.Trim();
            byte[] messageBytes = System.Text.Encoding.ASCII.GetBytes(responseData);
            Sendstream.Write(messageBytes, 0, messageBytes.Length);
        }
        catch (Exception e)
        {
            Debug.LogError("unable to send" + e);
        }

        try
        {
            Int32 bytes = Receivestream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            array = responseData.Split(',', 2);
            if (array[0] == "true")
            {
                Debug.Log("Created a game with code" + responseData);
                gameID = array[1];
            }
            else if (array[0] == "false")
            {
                return;
            }
        }
        catch (Exception e)
        {
            Debug.Log("error in receiving" + e);
        }
        backgroundreceiveThread.Start();

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
            Debug.LogError("unable to send" + e);
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
            Debug.Log("error in receiving " + e);
        }

        Debug.Log(RoomID.text);
        backgroundreceiveThread.Start();
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        // SceneManager.LoadScene(1, LoadSceneMode.Single);

    }

    async public void sendata(String responseData)
    {
        Byte[] data = new Byte[1024];
        try
        {
            byte[] messageBytes = System.Text.Encoding.ASCII.GetBytes(responseData);
            await Sendstream.WriteAsync(messageBytes, 0, messageBytes.Length);
            Debug.Log(responseData);
        }
        catch (Exception e)
        {
            Debug.LogError("Error connecting to the server: " + e.Message);
        }

    }
    public void receivedata()
    {
        while (true)
        {
            string responseData = string.Empty;
            Byte[] data = new Byte[1024];
            try
            {
                Receivestream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, data.Length);
                if (responseData.Split(',', 2)[0] == "/Joined")
                {
                    string[] players = responseData.Split(',', 2)[1].Split(',');
                    playerlist.Clear();
                    for (int i = 0; i < players.Length; i++)
                    {
                        playerlist.Add(new gameManager.playercustomclass(players[i]));
                        playertestlist.Add(players[i]);

                    }
                }
                else if((responseData.Split(',', 2)[0] == "/Startgame"))
                {
                
                    SceneManager.LoadScene(2, LoadSceneMode.Single);

                }

                Debug.Log(responseData);
            }
            catch (Exception e)
            {
                Debug.LogError("Error connecting to the server: " + e.Message);
            }
        }
    }

    void startgame()
    {
        string msg="/Start,"+gameID;
        sendata(msg);
    }
    public class playercustomclass
    {
        public string playername;
        public bool isadmin;

        public Color carcolor;

        public playercustomclass(string name)
        {
            playername = name; // Set the initial value for model
        }



    }

    void OnApplicationQuit()
    {
        backgroundreceiveThread.Abort();
    }
}
