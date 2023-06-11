using System;
using UnityEngine.SceneManagement;
using System.Net.Sockets;
using UnityEngine;
using System.Threading.Tasks;
public class CarSocket : MonoBehaviour
{
    private const string serverAddress = "127.0.0.1";
    private const int serverPort = 3002;
    NetworkStream stream;
    TcpClient client;
    public int counter;
    // Start is called before the first frame update
    void Awake()
    {
        try
        {
            client = new TcpClient(serverAddress, serverPort);
            stream = client.GetStream();
            Debug.Log("Connected to the server");

        }
        catch (Exception e)
        {
            Debug.LogError("Error connecting to the server: " + e.Message);
        }
        DontDestroyOnLoad(this);

    }

    // Update is called once per frame
    public void Update()
    {

        string coords = String.Format("{0:0.00}", this.transform.position.x) + "," + String.Format("{0:0.00}", this.transform.position.z);

        byte[] messageBytes = System.Text.Encoding.ASCII.GetBytes(coords);
        try
        {
            stream.Write(messageBytes, 0, messageBytes.Length);
        }
        catch (Exception e)
        {
            Debug.LogError("Error connecting to the server: " + e.Message);
        }


        // Send the message to the server
    }

    public void gotolobby()
    {

        SceneManager.LoadScene(1, LoadSceneMode.Single);

    }

    private async Task WaitOneSecondAsync()
    {
        await Task.Delay(TimeSpan.FromMilliseconds(2000));
    }
}
