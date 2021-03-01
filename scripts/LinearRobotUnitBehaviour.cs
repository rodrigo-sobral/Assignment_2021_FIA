using System;
using System.Collections;
using UnityEngine;

//este script está referenced ao Body
public class LinearRobotUnitBehaviour : RobotUnit {
    //LinearRobotUnitBehaviour extends RobotUnit 
    public float weightResource; //está definido a 1 no unity
    public float resourceValue;
    public float resouceAngle;
    public float wallValue;
    public float wallAngle;
   
    
    void Update() {
        // get sensor data
        resouceAngle = resourcesDetector.GetAngleToClosestResource();
        resourceValue = weightResource * resourcesDetector.GetLinearOuput();
        Debug.Log("resource_sensor_input:");
        Debug.Log(resouceAngle);
        Debug.Log(resourceValue);
        applyForce(resouceAngle, resourceValue); // go towards // apply to the ball

        wallAngle = blockDetector.GetAngleToClosestWall();
        resourceValue = 1 * blockDetector.GetLinearOuput();
        Debug.Log("wall_sensor_input:");
        Debug.Log(wallAngle);
        Debug.Log(wallValue);
        applyForce(wallAngle, resourceValue);
        //TODO: obter os dados do sensor dos blocos para obter o vetor a mandar para o "applyForce"

        
        //applyForce adds (angle,strength) tupple to array of resources to later calculate force vector (movement vector) of ball
    }
}