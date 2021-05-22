using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class ScoreKeeper : MonoBehaviour {

    //the score.
    public int[] score;

    public string[] inds;

    //the game's UI element 
    public Text text; 

	// Use this for initialization
	void Start () {
        score = new int[2];
        //inds = new string[2];
        //display the score to the screen
        UpdateScoreText();

	}

    void UpdateScoreText()
    {
        string toWrite = "Blue "+inds[0] + ": " + score[0] + " \tRed " + inds[1] +": " + score[1];
        text.text = toWrite;
    }

    //public method to be called from the Goal script
    public void ScoreGoal(int whichgoal)
    {
        //add one to the score
        score[whichgoal]++;
        //display the updated score
        UpdateScoreText();
    }

    public void setIds(string indexIndBlue, string indexIndRed)
    {
        inds = new string[2];
        inds[0] = ""+indexIndBlue;
        inds[1] = ""+indexIndRed;
    }
}
