using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamelogic : MonoBehaviour
{
    public GameObject gamestate;
    public GameObject carwithcontroller;
    public GameObject camerawithfollow;

    public GameObject mycar;

    public GameObject carwithoutcontroller;
    gameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        /*
                gamestate = GameObject.Find("sceneManager");
                gameManager = gamestate.GetComponent<gameManager>();
                gameManager.playerlist.Add(new gameManager.playercustomclass());
                gameManager.playerlist[0].carcolor = new Color(0.5f, 1, 1);
          */
        //for loop for instantiating
        mycar = (GameObject)Instantiate(carwithcontroller, new Vector3(5.2f, 0.1f, -3), Quaternion.identity);
        Instantiate(camerawithfollow, new Vector3(5.2f, 0.1f + 0.2f, -3 + 2), Quaternion.identity);
        mycar.name = "Omar";

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
