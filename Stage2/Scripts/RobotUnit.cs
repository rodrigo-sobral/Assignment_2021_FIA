using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotUnit : MonoBehaviour
{
   

    public int hitTheBall;
    public int hitTheWall;
    public Rigidbody rb;
    public float speed;
    public float startTime;
    public float timeElapsed = 0.0f;
    public DetectorScript objectsDetector;
    public bool debugMode = true;
    
    
    // Start is called before the first frame update
    void Start()
    {

        hitTheBall = 0;
        rb = GetComponent<Rigidbody>();
        this.startTime = Time.time;
        timeElapsed = Time.time - startTime;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag.Equals("ball"))
        {
            hitTheBall++;

        }
        else if (collision.collider.tag.Equals("Wall"))
        {
            hitTheWall++;
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag.Equals("ball"))
        {
            hitTheBall++;

        }
        else if (collision.collider.tag.Equals("Wall"))
        {
            hitTheWall++;
        }
    }


}