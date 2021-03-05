using System;
using System.Collections;
using UnityEngine;

//este script está referenced ao Body
public class LinearRobotUnitBehaviour : RobotUnit {
    //LinearRobotUnitBehaviour extends RobotUnit 
    public float weightResource=2.0f; //prioridade
    public float resourceValue;
    public float resouceAngle;

    public float weightWall = -0.5f; //prioridade
    public float wallValue;
    public float wallAngle;

    public bool resourceLinear=true; 
    public bool resourceGaussian = false;
    public bool resourceLogaritmic = false;

    public bool wallLinear = true;
    public bool wallGaussian = false;
    public bool wallLogaritmic = false;


    void Update() {
        // get sensor data
        resouceAngle = resourcesDetector.GetAngleToClosestResource();
        if (resourceGaussian)
        {
            resourceValue = weightResource * resourcesDetector.GetGaussianOutput();
        }
        else if (resourceLogaritmic)
        {
            resourceValue = weightResource * resourcesDetector.GetLogaritmicOutput();
        }
        else
        {
            resourceValue = weightResource * resourcesDetector.GetLinearOuput();
        }
        
        Debug.Log("resource_sensor_input:");
        Debug.Log(resouceAngle);
        Debug.Log(resourceValue);
        applyForce(resouceAngle, resourceValue); // go towards // apply to the ball
        
        wallAngle = blockDetector.GetAngleToClosestWall();
        
        if (wallGaussian)
        {
            wallValue = weightWall * blockDetector.GetGaussianOutput();
        }
        else if (wallLogaritmic)
        {
            wallValue = weightWall * blockDetector.GetLogaritmicOutput();
        }
        else
        {
            wallValue = weightWall * blockDetector.GetLinearOuput();
            Debug.Log(blockDetector.GetLinearOuput());
        }
        
        Debug.Log("wall_sensor_input:");
        Debug.Log(wallAngle);
        Debug.Log(wallValue);
        applyForce(wallAngle, wallValue); 
        //TODO: obter os dados do sensor dos blocos para obter o vetor a mandar para o "applyForce"

        
        //applyForce adds (angle,strength) tupple to array of resources to later calculate force vector (movement vector) of ball
    }
}