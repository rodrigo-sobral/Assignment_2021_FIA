using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorScript : MonoBehaviour
{
    protected Vector3 initialTransformUp;
    protected Vector3 initialTransformFwd;
    public bool debug_mode;
    public string AdversaryTag = "";
    public string AdversaryGoal = "";
    public string MyGoal = "";
    public Dictionary<string, ObjectInfo> objectsInformation;


    // Start is called before the first frame update
    void Start()
    {
   
        initialTransformUp = this.transform.up;
        initialTransformFwd = this.transform.forward;
    }


    public Dictionary<string, ObjectInfo> GetVisibleObjects()
    {


        //GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>();
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 80);
        objectsInformation = new Dictionary<string, ObjectInfo>();

        foreach (Collider col in hitColliders)
        {
            Vector2 sensorPos = new Vector2(this.transform.position.x, this.transform.position.z);

            Vector3 temp = col.ClosestPointOnBounds(this.transform.position);

            Vector2 objectPos = new Vector2(temp.x, temp.z);
            Vector2 objectLocalPos = new Vector2(col.gameObject.transform.localPosition.x, col.gameObject.transform.localPosition.z); //Normalisation: 40 is the max xcoordinates 25 is the z max

            Vector2 dir = sensorPos - objectPos;
            dir = this.transform.InverseTransformDirection(dir);
            float angle = Mathf.Round(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 180); //Normalisation: 360 is the max angle we have
            float dist = Mathf.Round(Vector2.Distance(sensorPos, objectPos));// Normalization: 95 is the max value of distance 


            ObjectInfo info = new ObjectInfo(dist, angle, objectPos);

            if (col.tag.Equals(AdversaryGoal)) {
                if (!objectsInformation.ContainsKey("AdversaryGoal") || info.distance < objectsInformation["AdversaryGoal"].distance)
                {
                    objectsInformation["AdversaryGoal"] = info;
                }

            }
            else if (col.tag.Equals(MyGoal))
            {
                if (!objectsInformation.ContainsKey("MyGoal") || info.distance < objectsInformation["MyGoal"].distance)
                {
                    objectsInformation["MyGoal"] = info;
                }
            }
            else if (col.tag.Equals("ball"))
            {
                if (!objectsInformation.ContainsKey("DistanceToBall") || info.distance < objectsInformation["DistanceToBall"].distance)
                {
                    objectsInformation["DistanceToBall"] = info;
                }
               
            }
            else if (col.tag.Equals("Wall"))
            {
                if (!objectsInformation.ContainsKey("Wall") || info.distance < objectsInformation["Wall"].distance)
                {
                    objectsInformation["Wall"] = info;
                }

            }
            else if (col.tag.Equals(AdversaryTag))
            {

                if (!objectsInformation.ContainsKey("Adversary") || info.distance < objectsInformation["Adversary"].distance)
                {
                    objectsInformation["Adversary"] = info;
                }

            }

        }

        return objectsInformation;
    }



}
