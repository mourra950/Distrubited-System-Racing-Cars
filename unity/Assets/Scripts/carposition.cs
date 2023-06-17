using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carposition : MonoBehaviour
{
    public string tempcoord;
    public float smooth = 2f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (tempcoord != "")
        {
            string[] tempvalues = tempcoord.Split(',', 2)[1].Split(',');
            this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(float.Parse(tempvalues[0]), float.Parse(tempvalues[1]), float.Parse(tempvalues[2])), smooth * Time.deltaTime);
            Quaternion rotation = Quaternion.Euler(float.Parse(tempvalues[3]), float.Parse(tempvalues[4]), float.Parse(tempvalues[5]));
            this.transform.rotation = rotation;
        }
    }
}
