using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamelogic : MonoBehaviour
{
    public GameObject gamestate;
    public GameObject carwithcontroller;
    public GameObject camerawithfollow;

    public GameObject mycar;
    public GameObject tempcar;


    public GameObject carwithoutcontroller;
    gameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {

        gamestate = GameObject.Find("sceneManager");
        gameManager = gamestate.GetComponent<gameManager>();


        mycar = (GameObject)Instantiate(carwithcontroller, new Vector3(5.2f, 0.2f, -3), Quaternion.identity);
        Instantiate(camerawithfollow, new Vector3(5.2f, 0.1f + 0.2f, -3 + 2), Quaternion.identity);
        mycar.name = gameManager.UserID;

        string[] temparray = gameManager.playertestlist.ToArray();
        //for loop for instantiating
        for (int i = 0; i < 2; i++)
        {
            if (gameManager.UserID != temparray[i])
            {
                tempcar = (GameObject)Instantiate(carwithoutcontroller, new Vector3(5.2f - (i * 0.5f), 0.2f, -3 - (i * 0.2f)), Quaternion.identity);
                tempcar.name = temparray[i];
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        //sending car coordinates
        string coordmessage = "/Coord," + mycar.transform.position.x.ToString("0.00") + "," + mycar.transform.position.y.ToString("0.00") + "," + mycar.transform.position.z.ToString("0.00") + "," + mycar.transform.rotation.eulerAngles.x.ToString("0.00") + "," + mycar.transform.rotation.eulerAngles.y.ToString("0.00") + "," + mycar.transform.rotation.eulerAngles.z.ToString("0.00");
        Debug.Log(coordmessage);
        // gameManager.sendata("/Coord,"+);





        // gameManager.receivedata();

    }
}
