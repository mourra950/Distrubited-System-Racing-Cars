using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using System.Net.Sockets;
using UnityEngine;
using TMPro;
using System.Threading;

public class gameManager : MonoBehaviour
{

    public List<string> playertestlist = new List<string>();
    public List<GameObject> playerReference = new List<GameObject>();
    public List<playercustomclass> playerlist = new List<playercustomclass>();
    public List<string> chat = new List<string>();
    public TMP_InputField RoomID;
    public TMP_InputField countField;

    public string gameID = "123412s";
    public string UserID;
    public bool countChanged = true;
    public bool scenechanged = false;
    bool connectionSuccess = false;
    bool gamestarted = false;
    bool receivedcoord = false;
    public bool _isplayer = true;


    string tempcoord;


    TcpClient unitySend = new TcpClient();
    TcpClient unityReceive = new TcpClient();
    NetworkStream Receivestream;
    NetworkStream Sendstream;

    private const string serverAddress = "127.0.0.1";
    Thread backgroundreceiveThread;

    public void Start()
    {
        RoomID = GameObject.Find("RoomID").GetComponent<TMP_InputField>();
    }
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
        if (gamestarted)
        {
            SceneManager.LoadScene(2, LoadSceneMode.Single);
            scenechanged = true;
            gamestarted = false;

        }
        if (receivedcoord == true)
        {
            Debug.Log("Finally");
            string tempuserID = tempcoord.Split(',', 2)[0].Trim();
            string[] temparray = playertestlist.ToArray();
            if (_isplayer == true)
            {
                for (int i = 0; i < temparray.Length - 1; i++)
                {

                    if (playerReference[i].name == tempuserID)
                    {

                        carposition tempref = playerReference[i].GetComponent<carposition>();
                        tempref.tempcoord = tempcoord;

                    }
                }
                receivedcoord = false;
            }
            else
            {

                for (int i = 0; i < temparray.Length; i++)
                {

                    if (playerReference[i].name == tempuserID)
                    {

                        carposition tempref = playerReference[i].GetComponent<carposition>();
                        tempref.tempcoord = tempcoord;

                    }
                }
                receivedcoord = false;
            }
        }
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
                Debug.Log("Created a game with code " + responseData);
                gameID = array[1].Split(',', 2)[0];
                UserID = array[1].Split(',', 2)[1];

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
            responseData = "/Join," + RoomID.text.Trim() + "," + "true";
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

            if (responseData.Split(',', 2)[0] == "false")
            {
                return;
            }
            else
            {
                UserID = responseData.Split(',', 2)[1].Split(',', 2)[1];

                gameID = responseData.Split(',', 2)[1].Split(',', 2)[0];
            }

        }
        catch (Exception e)
        {
            Debug.Log("error in receiving " + e);
        }
        Debug.Log(RoomID.text);
        backgroundreceiveThread.Start();
        SceneManager.LoadScene(3, LoadSceneMode.Single);
    }

    async public void sendata(String responseData)
    {
        Byte[] data = new Byte[1024];
        try
        {
            byte[] messageBytes = System.Text.Encoding.ASCII.GetBytes(responseData);
            await Sendstream.WriteAsync(messageBytes, 0, messageBytes.Length);
            // Debug.Log(responseData);
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
                    countChanged = true;


                }
                else if (responseData.Split(',', 2)[0] == "/NCoord")
                {
                    if (receivedcoord == false)
                    {
                        tempcoord = responseData.Split(',', 2)[1];
                        // Debug.Log("inside receive");
                        Debug.Log(tempcoord);

                        receivedcoord = true;
                    }

                }
                else if ((responseData.Split(',', 2)[0] == "/Startgame"))
                {
                    gamestarted = true;
                }

                Debug.Log(responseData);
            }
            catch (Exception e)
            {
                Debug.LogError("Error connecting to the server: " + e.Message);
            }
        }
    }

    public void startgame()
    {
        string msg = "/Start," + gameID;
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
    public void SpectateGame()
    {

        string responseData;
        Byte[] data = new Byte[1024];
        try
        {
            responseData = "/Join," + RoomID.text.Trim() + "," + "false";
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

            if (responseData.Split(',', 2)[0] == "false")
            {
                return;
            }
            else
            {
                UserID = responseData.Split(',', 2)[1].Split(',', 2)[1];

                gameID = responseData.Split(',', 2)[1].Split(',', 2)[0];
            }

        }
        catch (Exception e)
        {
            Debug.Log("error in receiving " + e);
        }
        _isplayer = false;
        Debug.Log(RoomID.text);
        backgroundreceiveThread.Start();
        SceneManager.LoadScene(3, LoadSceneMode.Single);
        // SceneManager.LoadScene(1, LoadSceneMode.Single);

    }
    void OnApplicationQuit()
    {
        backgroundreceiveThread.Abort();
    }
}
