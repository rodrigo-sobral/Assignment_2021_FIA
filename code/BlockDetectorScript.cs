using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDetectorScript : MonoBehaviour {
    public float angleOfSensors = 10f;
    public float rangeOfSensors = 10f;
    protected Vector3 initialTransformUp;
    protected Vector3 initialTransformFwd;
    public float strength;
    public float angleToClosestObj;
    public int numObjects;
    public bool debug_mode;
    
    // Start is called before the first frame update
    void Start() {
        initialTransformUp = this.transform.up;
        initialTransformFwd = this.transform.forward;
    }

    // Update is called once per frame
    void FixedUpdate() {
        // YOUR CODE HERE (COPIED FROM RESOURCEDECTECTORSCRIPT)
        ObjectInfo anObject;
        //  IT GETS THE NEAREST WALL TODO:
        anObject = GetClosestWall();
        if (anObject != null) {
            angleToClosestObj = anObject.angle;
            strength = 1.0f / (anObject.distance + 1.0f);
        }
    }

    public float GetAngleToClosestObstacle()
    {
        return angleToClosestObj;
    }

    public float GetLinearOuput()
    {
        return strength;
    }

    public virtual float GetGaussianOutput() {
        // YOUR CODE HERE
        throw new NotImplementedException();
    }

    public virtual float GetLogaritmicOutput() {
        // YOUR CODE HERE
        throw new NotImplementedException();
    }

    //  VISIBLE DEPENDING ON THE VISIBLE RAIO
    public ObjectInfo[] GetVisibleWalls() {
        return (ObjectInfo[]) GetVisibleObjects("Wall").ToArray();
    }

    public ObjectInfo GetClosestWall() {
        ObjectInfo [] a = (ObjectInfo[])GetVisibleObjects("Wall").ToArray();
        if(a.Length == 0) return null;
        return a[a.Length-1];
    }

    //Raio de visão (os sensores do objeto detetam objetos num determinado 'raio')
    public List<ObjectInfo> GetVisibleObjects(string objectTag) {
        RaycastHit hit;
        List<ObjectInfo> objectsInformation = new List<ObjectInfo>();

        for (int i = 0; i * angleOfSensors < 360f; i++) {
            if (Physics.Raycast(this.transform.position, Quaternion.AngleAxis(-angleOfSensors * i, initialTransformUp) * initialTransformFwd, out hit, rangeOfSensors)) {

                if (hit.transform.gameObject.CompareTag(objectTag)) {
                    if (debug_mode) {
                        Debug.DrawRay(this.transform.position, Quaternion.AngleAxis((-angleOfSensors * i), initialTransformUp) * initialTransformFwd * hit.distance, Color.red);
                    }
                    ObjectInfo info = new ObjectInfo(hit.distance, angleOfSensors * i + 90);
                    objectsInformation.Add(info);
                }
            }
        }

        objectsInformation.Sort();

        return objectsInformation;
    }
}
