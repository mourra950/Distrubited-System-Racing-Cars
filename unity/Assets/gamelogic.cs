using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamelogic : MonoBehaviour
{
    public GameObject gamestate;
    public GameObject carwithcontroller;
    public GameObject camerawithfollow;

	public GameObject test;

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
        test=(GameObject)Instantiate(carwithcontroller, new Vector3(5.2f, 0.1f, -3), Quaternion.identity);
        Instantiate(camerawithfollow, new Vector3(5.2f, 0.1f+0.2f, -3+2), Quaternion.identity);
        test.name="Omar";

    }

    // Update is called once per frame
    void Update()
    {

    }
}
