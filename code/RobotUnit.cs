using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotUnit : MonoBehaviour {
    public int resourcesGathered;
    protected Rigidbody rb;
    public float speed = 1.0f;
    public Text countText;
    public float startTime;
    public float timeElapsed = 0.0f;
    public ResourceDetectorScript resourcesDetector;
    public BlockDetectorScript blockDetector;
    private List<Tuple<float, float>> listAngleStr;
    public bool debugMode = true;
    protected int maxObjects = 0;

    // Start is called before the first frame update
    void Start() {
        //strength = 0.0f;
        maxObjects = GameObject.FindGameObjectsWithTag("Pickup").Length;
        resourcesGathered = 0;
        rb = GetComponent<Rigidbody>();
        listAngleStr = new List<Tuple<float, float>>();
        this.startTime = Time.time;
        timeElapsed = Time.time - startTime;
        SetCountText();
    }

    void FixedUpdate() {
        int i = 0;
        foreach(Tuple<float,float> tmp in listAngleStr){
            //
            float angle = tmp.Item1;
            float strength = tmp.Item2;
            angle *= Mathf.Deg2Rad;
            float xComponent = Mathf.Cos(angle);
            float zComponent = Mathf.Sin(angle);
            Vector3 forceDirection = new Vector3(xComponent, 0, zComponent);
            if (debugMode) {
                Debug.DrawRay(this.transform.position, (forceDirection * strength * speed) , i == 0 ? Color.black :Color.magenta );
            }
            rb.AddForce(forceDirection * strength * speed);
            i++;
        }
        listAngleStr.Clear(); // cleanup
    }

    private void LateUpdate() {
        SetCountText();
    }

    //  WRITE INFORMATION TEXT
    void SetCountText() {
        if(resourcesGathered < maxObjects) {
            this.timeElapsed = Time.time - this.startTime;
        }
        
        string minutes = ((int)(timeElapsed / 60)).ToString();
        string seconds = (timeElapsed % 60).ToString("f0");
        countText.text = "Resources Gathered: " + resourcesGathered.ToString() + "/" + maxObjects + "\nTime Elapsed: " + minutes + ":" + seconds; //start
    }

    public void applyForce(float angle, float strength) {
        listAngleStr.Add(new Tuple<float, float>(angle, strength));
    }


    //  ROBOT TRIGGERS (IF THE ROBOT CATCHED A CUBE OR DIED)
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Pickup")) {
            other.gameObject.SetActive(false);
            resourcesGathered++;
        }
        else
        {
            if (other.gameObject.CompareTag("Deadly"))
            {
                Debug.Log("Destroyed!");
                this.gameObject.transform.parent.gameObject.SetActive(false);
            }
        }

    }


}