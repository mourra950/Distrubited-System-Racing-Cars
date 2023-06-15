using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Net;
using System.Threading.Tasks;
public class gamelogic : MonoBehaviour
{
    public GameObject gamestate;
    public GameObject carwithcontroller;
    public GameObject camerawithfollow;

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




        string[] temparray = gameManager.playertestlist.ToArray();
        //for loop for instantiating
        for (int i = 0; i < temparray.Length; i++)
        {
            Debug.Log(i + " attempt");
            if (!(temparray[i].Contains(gameManager.UserID)))
            {
                tempcar = (GameObject)Instantiate(carwithoutcontroller, new Vector3(5.33f, 0.2f, 2.7f), Quaternion.identity);
                tempcar.name = temparray[i];
                gameManager.playerReference.Add(tempcar);
            }
            else
            {
                mycar = (GameObject)Instantiate(carwithcontroller, new Vector3(5.33f, 0.2f, 2.7f), Quaternion.identity);
                Instantiate(camerawithfollow, new Vector3(5.8f, 0.5f, 2.7f), Quaternion.identity);
                mycar.name = gameManager.UserID;
            }

        }
        sleepthread = new Thread(sleepcounter);
        sleepthread.Start();


    }

    // Update is called once per frame
    void Update()
    {
        //sending car coordinates
        if (sleepbool == true)
        {
            string coordmessage = "/Coord," + mycar.transform.position.x.ToString("0.00") + "," + mycar.transform.position.y.ToString("0.00") + "," + mycar.transform.position.z.ToString("0.00") + "," + mycar.transform.rotation.eulerAngles.x.ToString("0.00") + "," + mycar.transform.rotation.eulerAngles.y.ToString("0.00") + "," + mycar.transform.rotation.eulerAngles.z.ToString("0.00");
            gameManager.sendata(coordmessage);
            sleepbool = false;
            // Debug.Log(coordmessage);
            // gameManager.sendata("/Coord,"+);

        }



        // gameManager.receivedata();

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
