using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDetectorScript : MonoBehaviour {
    public float angleOfSensors = 10f; //TODO: fazer variar este parametro para obter o melhor agente
    /*parametros a variar
    /*definir 10 neste par^ametro
(valor por defeito no sensor fornecido) implica que os 360 graus em volta
do sensor sejam divididos em raios a cada 10 graus (36 raios igualmente
espacados em redor do sensor).*/
    public float rangeOfSensors = 30f; //TODO: fazer variar este parametro para obter o melhor agente
    protected Vector3 initialTransformUp;
    protected Vector3 initialTransformFwd;
    public float strength; //to closest wall
    public float varying_strenght=2.2f; //Increase this parameter value para atenuar a energia com q o boneco se afasta das paredes
    //nota: o varying_streght do boneco em relação aos resources é = 1, logo o varying_strenght para as paredes deve ser superior!
    public float angle; //to closest wall
    public int numObjects;
    public bool debug_mode;

    
    // Start is called before the first frame update
    void Start() {
        initialTransformUp = this.transform.up;
        initialTransformFwd = this.transform.forward;
    }

    // Update is called once per frame
    void FixedUpdate() {
        ObjectInfo wall; 
        wall = GetClosestWall(); //closest wall

        if (wall != null) {
            angle = wall.angle;
            strength = -(1.0f / (wall.distance +varying_strenght)); //está a ser calculada c/ mesma formula q o resource (mas negativa)
        }
    }

    public float GetAngleToClosestWall()
    {
        return angle;
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
        //para utilizar no calculo da streght na próxima meta
        throw new NotImplementedException();
    }

    //  VISIBLE DEPENDING ON THE VISIBLE RAIO
    public ObjectInfo[] GetVisibleWalls() {
        //Debug.Log("OLA");
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



