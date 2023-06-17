
using UnityEngine;
using System.Threading;
public class gamelogic : MonoBehaviour
{
    public GameObject gamestate;
    public GameObject carwithcontroller;
    public GameObject camerawithfollow;
    public GameObject cameraspectate;


    public GameObject mycar;
    public GameObject tempcar;

    bool sleepbool = false;
    public GameObject carwithoutcontroller;
    gameManager gameManager;
    Thread sleepthread;

    // Start is called before the first frame update
    void Start()
    {

        gamestate = GameObject.Find("sceneManager");
        gameManager = gamestate.GetComponent<gameManager>();


        if (gameManager._isplayer == true)
        {
            mycar = (GameObject)Instantiate(carwithcontroller, new Vector3(5.33f, 0.5f, 2.7f), Quaternion.identity);
            mycar.name = gameManager.UserID;
            Instantiate(camerawithfollow, new Vector3(5.8f, 0.5f, 2.7f), Quaternion.identity);
        }
        else
        {
            Instantiate(cameraspectate, new Vector3(0, 11.4f, 1.4f), Quaternion.identity);
        }
        string[] temparray = gameManager.playertestlist.ToArray();
        //for loop for instantiating
        for (int i = 0; i < temparray.Length; i++)
        {
            Debug.Log(i + " attempt");
            if (!(temparray[i].Contains(gameManager.UserID)))
            {
                tempcar = (GameObject)Instantiate(carwithoutcontroller, new Vector3(5.33f, 0.5f, 2.7f), Quaternion.identity);
                tempcar.name = temparray[i];
                gameManager.playerReference.Add(tempcar);
            }


        }
        sleepthread = new Thread(sleepcounter);
        sleepthread.Start();


    }

    void Update()
    {
        if (gameManager._isplayer == true)
        {
            if (sleepbool == true)
            {
                string coordmessage = "/Coord," + mycar.transform.position.x.ToString("0.00") + "," + mycar.transform.position.y.ToString("0.00") + "," + mycar.transform.position.z.ToString("0.00") + "," + mycar.transform.rotation.eulerAngles.x.ToString("0.00") + "," + mycar.transform.rotation.eulerAngles.y.ToString("0.00") + "," + mycar.transform.rotation.eulerAngles.z.ToString("0.00");
                gameManager.sendata(coordmessage);
                sleepbool = false;
            }
        }



    }
    void sleepcounter()
    {
        while (true)
        {
            if (sleepbool == false)
            {
                Thread.Sleep(300);
                sleepbool = true;
            }
        }

    }
}
