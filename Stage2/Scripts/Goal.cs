using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    public GameObject ballSpawner;
    public GameObject ball;
    public ScoreKeeper scoreKeeper;

    public enum WhichGoal{
        Blue,
        Red,

    }
    protected bool defenseTask = false;
    public WhichGoal whichGoal;
    public Vector3 initalBallPosition;



    public void ShootTheBallInMyDirection()
    {
        //if this method is called we assume that we are in the defense task
        defenseTask = true;
        Vector3 shoot = (this.transform.localPosition - ball.transform.localPosition).normalized;
        ball.GetComponent<Rigidbody>().AddForce(shoot * 800.0f);

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ball")
        {
            //cancel out the ball's velocity
            ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
            //cancel out the ball's angular velocity
            ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            //call the ScoreGoal() method to add a poin
            scoreKeeper.ScoreGoal((int)whichGoal);

            if (defenseTask)
            {

                ball.transform.localPosition = initalBallPosition;
                this.ShootTheBallInMyDirection();
            }
            else
            {
                //set the ball's position equal to the ball spawner's position; i.e., reset the ball position
                ball.transform.position = ballSpawner.transform.position;
            }



        }
    }

}
