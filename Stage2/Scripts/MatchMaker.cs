using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class MatchMaker : MonoBehaviour {

	// instances
	public static MatchMaker instance = null;
    [HideInInspector]
    public Text infoText;
    [HideInInspector]
    public bool simulating = false;
    public string PathRedPlayer;
    public string PathBluePlayer;
    public GameObject simulationPrefab;
	private SimulationInfo bestSimulation;
	private NeuralNetwork BlueController;
    private NeuralNetwork RedController;
    [HideInInspector]
    public int TheTimeScale = 1;
    //protected string folder = "Assets/Logs/";
    public float MatchTimeInSecs;
    [Header("Scenario Conditions")]
    public bool randomRedPlayerPosition = false;
    public bool randomBluePlayerPosition = false;
    public bool randomBallPosition = false;
    public bool MovingBall = false;

    

	void Awake(){
		// deal with the singleton part
		if (instance == null) {
			instance = this;
		}
		else if (instance != this) {
			DestroyImmediate (gameObject);    
		}
		DontDestroyOnLoad(gameObject);
		loadPlayers ();
		simulating = false;

	}

	void loadPlayers() {
        Debug.Log("Tyring to Load Blue:" + PathBluePlayer);
        if (File.Exists(PathBluePlayer))
		{
            
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open((PathBluePlayer).Trim(), FileMode.Open);
			this.BlueController = (NeuralNetwork) bf.Deserialize(file);
			file.Close();
		}
        else
        {
            Debug.Log("Path to Red Player does not exist");
        }
        Debug.Log("Tyring to Load Red:" + PathRedPlayer);
        if (File.Exists(PathRedPlayer))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open((PathRedPlayer).Trim(), FileMode.Open);
            this.RedController = (NeuralNetwork)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            Debug.Log("Path to Red Player does not exist");
        }

    }

	private SimulationInfo createSimulation(int sim_i, Rect location)
	{
        D31NeuralControler bluePlayerScript = null;
        D31NeuralControler redPlayerScript = null;
        GameObject sim = Instantiate(simulationPrefab, transform.position + new Vector3(0, (sim_i * 250), 0), Quaternion.identity);
        sim.GetComponentInChildren<Camera>().rect = location;
        if (sim.transform.Find("D31-red") != null)
            redPlayerScript = sim.transform.Find("D31-red").gameObject.transform.Find("Body").gameObject.GetComponent<D31NeuralControler>();
        if (sim.transform.Find("D31-blue") != null)
            bluePlayerScript = sim.transform.Find("D31-blue").gameObject.transform.Find("Body").gameObject.GetComponent<D31NeuralControler>();
        sim.GetComponentInChildren<Camera> ().rect = location;
		sim.transform.Find("Scoring System").gameObject.GetComponent<ScoreKeeper>().setIds(PathBluePlayer, PathRedPlayer);

		if (bluePlayerScript != null &&  bluePlayerScript.enabled)
        {// BluePlayer Controller
            bluePlayerScript.neuralController = BlueController;
            bluePlayerScript.maxSimulTime = this.MatchTimeInSecs;
            bluePlayerScript.running = true;
        }
        if (redPlayerScript != null && redPlayerScript.enabled || PathRedPlayer.Length != 0)
        {// RedController Controller
            redPlayerScript.enabled = true;
            redPlayerScript.neuralController = RedController;
            redPlayerScript.maxSimulTime = this.MatchTimeInSecs;
            redPlayerScript.running = true;
		}

        return new SimulationInfo (sim, redPlayerScript,bluePlayerScript, 0,0);
	}

	void Update () {
        infoText.text = "Playing a match for " + this.MatchTimeInSecs +" secs";
		// show best.. in loop
		if (!simulating) {
            // x values: -20, 20
            // z values: -25, 25
            Vector3 redPlayerStartPosition = new Vector3(Random.Range(-20, 20), 0, Random.Range(0, 20));
            Vector3 ballStartPosition = new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 0));



            bestSimulation = createSimulation (0, new Rect (0.0f, 0.0f, 1f, 1f));

            if (randomRedPlayerPosition)
            {
                GameObject p = bestSimulation.sim.transform.Find("D31-red").gameObject;
                redPlayerStartPosition.y = p.transform.localPosition.y;
                p.transform.localPosition = redPlayerStartPosition;
            }

            if (randomBallPosition)
            {
                GameObject p = bestSimulation.sim.transform.Find("Ball").gameObject;
                ballStartPosition.y = p.transform.localPosition.y;
                p.transform.localPosition = ballStartPosition;
            }

            if (MovingBall)
            {
                Goal goal = bestSimulation.sim.transform.Find("Field").transform.Find("RedGoal").GetComponent<Goal>();
                GameObject p = bestSimulation.sim.transform.Find("Ball").gameObject;
                if (randomBallPosition)
                {
                    goal.initalBallPosition = ballStartPosition;
                }
                else
                {
                    goal.initalBallPosition = p.transform.position;
                }



                goal.ShootTheBallInMyDirection();

            }

            Time.timeScale = TheTimeScale;
			simulating = true;

		} else if (simulating) {
			if (!bestSimulation.playerRed.running && bestSimulation.playerRed.gameOver) {
                Debug.Log("Red score (according to current GetScoreRed fitness function): " + bestSimulation.playerRed.GetScoreRed());
                if(bestSimulation.playerBlue != null)
                    Debug.Log("Blue score (according to current GetScoreBlue fitness function): " + bestSimulation.playerBlue.GetScoreBlue());
                simulating = false;
				DestroyImmediate (bestSimulation.sim);
			}
		}
	}
	}




