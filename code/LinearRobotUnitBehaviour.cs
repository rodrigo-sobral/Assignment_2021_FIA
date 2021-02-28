using System;
using System.Collections;
using UnityEngine;

public class LinearRobotUnitBehaviour : RobotUnit {
    //LinearRobotUnitBehaviour extends RobotUnit 
    public float weightResource;
    public float resourceValue;
    public float resouceAngle;
    //public float blockValue;
    //public float blockAngle;
    
    void Update() {
        // get sensor data
        resouceAngle = resourcesDetector.GetAngleToClosestResource();
        resourceValue = weightResource * resourcesDetector.GetLinearOuput();

        
        //TODO: obter os dados do sensor dos blocos para obter o vetor a mandar para o "applyForce"


        // apply to the ball
        applyForce(resouceAngle, resourceValue); // go towards
        //applyForce adds (angle,strength) tupple to array of resources to later calculate force vector (movement vector) of ball
    }
}