﻿using System;
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
    public float angle; //to closest wall
    public int numObjects;
    public bool debug_mode;

    //activation functions limits
    public float strengthLimitLow = 0.0f; //limite inferior p/ eixo x
    public float strengthLimitSup = 1.0f; //limite superior p/ eixo x
    public float outLimitLow = 0.0f; //limite inferior p/ eixo y
    public float outLimitSup = 1.0f; //limite superior p/ eixo y

    //gaussian function parameters:
    public float gaussMicro = 0.5f; //from enunciado
    public float gaussSigma = 0.12f; //from enunciado


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
            strength = 1.0f / (wall.distance +1); //está a ser calculada c/ mesma formula q o resource 
        }
    }

    public float GetAngleToClosestWall()
    {
        return angle;
    }

    public float GetLinearOuput()
    {
        if (strength < strengthLimitLow) //limite inferior strenght
        {
            strength = strengthLimitLow;
        }
        else if (strength > strengthLimitSup) //limite superior strenght
        {
            strength = strengthLimitSup;
        }
        if (strength < outLimitLow)
        {
            return outLimitLow;
        }
        else if (strength > outLimitSup)
        {
            return outLimitSup;
        }
        return strength;
    }

    public virtual float GetGaussianOutput() {
        float y;
        if (strength < strengthLimitLow) //limite inferior strenght
        {
            strength = strengthLimitLow;
        }
        else if (strength > strengthLimitSup) //limite superior strenght
        {
            strength = strengthLimitSup;
        }
        y = (float)((1 / (gaussSigma * Math.Sqrt(2 * Math.PI))) * Math.Exp(-0.5 * (((strength - gaussMicro) * (strength - gaussMicro)) / (gaussSigma * gaussSigma))));

        if (y < outLimitLow)
        {
            y = outLimitLow;
        }
        else if (y > outLimitSup)
        {
            y = outLimitSup;
        }
        return y;
    }

    public virtual float GetLogaritmicOutput() {
        float y;
        if (strength < strengthLimitLow) //limite inferior strenght
        {
            strength = strengthLimitLow;
        }
        else if (strength > strengthLimitSup) //limite superior strenght
        {
            strength = strengthLimitSup;
        }
        y = (float)-Math.Log(strength); //negative natural (base e) logarithm!

        if (y < outLimitLow)
        {
            y = outLimitLow;
        }
        else if (y > outLimitSup)
        {
            y = outLimitSup;
        }
        return y;
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
                        Debug.DrawRay(this.transform.position, Quaternion.AngleAxis((-angleOfSensors * i), initialTransformUp) * initialTransformFwd * hit.distance, Color.green);
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


