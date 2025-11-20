using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject ball;
    public Vector3 offset;
    public float zRotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        zRotation = Mathf.Clamp(zRotation, -90.0f, 90.0f);
        //Quaternion.SetFromToRotation
        //Vector3.RotateTowards


        //this.transform.position = ball.transform.position + Quaternion.AngleAxis(zRotation, new Vector3(0.0f, 1.0f, 0.0f)) * offset;
        //this.transform.rotation = Quaternion.LookRotation(ball.transform.position - transform.position);


        this.transform.position = ball.transform.position + offset;
        this.transform.rotation = Quaternion.LookRotation(ball.transform.position - transform.position);


        //this.transform.position = new Vector3(
        //ball.transform.position.x - offset,
        //ball.transform.position.y,
        //ball.transform.position.z);
    }
}
