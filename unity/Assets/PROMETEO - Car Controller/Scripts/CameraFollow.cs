using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public GameObject carwithcontrols;
    public Transform carTransform;
    [Range(1, 10)]
    public float followSpeed = 2;
    [Range(1, 10)]
    public float lookSpeed = 5;
    Vector3 initialCameraPosition;
    Vector3 initialCarPosition;
    Vector3 absoluteInitCameraPosition;

    public Vector3 elevation;
    public float cardistance;

    void Start()
    {
        carwithcontrols = GameObject.Find("Omar");
        carTransform = carwithcontrols.GetComponent<Transform>();


        initialCameraPosition = gameObject.transform.position;

        initialCarPosition = carTransform.position;
        absoluteInitCameraPosition = initialCameraPosition - initialCarPosition;
    }

    void FixedUpdate()
    {
        //Look at car
        Vector3 _lookDirection = (new Vector3(carTransform.position.x, carTransform.position.y, carTransform.position.z)) - transform.position;
        Quaternion _rot = Quaternion.LookRotation(_lookDirection, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, _rot, lookSpeed * Time.deltaTime);

        //Move to car
        Vector3 _targetPos = carTransform.transform.position;
        _targetPos = _targetPos - cardistance * carTransform.transform.forward;
        _targetPos += elevation;
        transform.position = Vector3.MoveTowards(transform.position, _targetPos, followSpeed * Time.deltaTime);

    }

}
/*
public class Follow : MonoBehaviour
{
   	public GameObject Target = null;
	public GameObject T = null;
	public float speed = 1.5f;

    void Start()
    {
		Target = GameObject.FindGameObjectWithTag("Player");
		T = GameObject.FindGameObjectWithTag("Target");
    }

  
    void FixedUpdate()
    {
		this.transform.LookAt(Target.transform);
		float car_Move = Mathf.Abs(Vector3.Distance(this.transform.position, T.transform.position) * speed);
		this.transform.position = Vector3.MoveTowards(this.transform.position, T.transform.position, car_Move * Time.deltaTime);
    }

}
*/