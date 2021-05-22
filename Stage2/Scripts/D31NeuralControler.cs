using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


public class D31NeuralControler : MonoBehaviour
{
    public RobotUnit agent; // the agent controller we want to use
    public int player;
    public GameObject ball;
    public GameObject MyGoal;
    public GameObject AdversaryGoal;
    public GameObject Adversary;
    public GameObject ScoreSystem;

    //BY US
    public enum TipoFuncaoAptidao { Atacante0, Atacante2, GuardaRedes }; //by us <---------
    public TipoFuncaoAptidao redPopulationAptidaoMethod = TipoFuncaoAptidao.Atacante0; //população RED //by us
    public TipoFuncaoAptidao bluePopulationAptidaoMethod = TipoFuncaoAptidao.GuardaRedes; //populacao BLUE //by us

    
    public int numberOfInputSensores { get; private set; }
    public float[] sensorsInput;


    // Available Information 
    [Header("Environment  Information")]
    public List<float> distanceToBall;
    public List<float> distanceToMyGoal;
    public List<float> distanceToAdversaryGoal;
    public List<float> distanceToAdversary;
    public List<float> distancefromBallToAdversaryGoal;
    public List<float> distancefromBallToMyGoal;
    public List<float> distanceToClosestWall;
    // 
    public List<float> agentSpeed;
    public List<float> ballSpeed;
    public List<float> advSpeed;
    private float FIELD_SIZE =95.0f;
    //
    public float simulationTime = 0;
    public float distanceTravelled = 0.0f;
    public float avgSpeed = 0.0f;
    public float maxSpeed = 0.0f;
    public float currentSpeed = 0.0f;
    public float currentDistance = 0.0f;
    public int hitTheBall;
    public int hitTheWall;
    public int fixedUpdateCalls = 0;
    //
    public float maxSimulTime = 30;
    public bool GameFieldDebugMode = false;
    public bool gameOver = false;
    public bool running = false;

    private Vector3 startPos;
    private Vector3 previousPos;
    private Vector3 ballStartPos;
    private Vector3 ballPreviousPos;
    private Vector3 advPreviousPos;
    private int SampleRate = 1;
    private int countFrames = 0;
    public int GoalsOnAdversaryGoal;
    public int GoalsOnMyGoal;
    public float[] result;



    public NeuralNetwork neuralController;

    private void Awake()
    {
        // get the unit controller
        agent = GetComponent<RobotUnit>();
        numberOfInputSensores = 18;
        sensorsInput = new float[numberOfInputSensores];

        startPos = agent.transform.localPosition;
        previousPos = startPos;
        // 2021
        ballPreviousPos = ball.transform.localPosition;
        if (Adversary !=null) { 
            advPreviousPos = Adversary.transform.localPosition;
        }
        
        //Debug.Log(this.neuralController);
        if (GameFieldDebugMode && this.neuralController.weights == null)
        {
            Debug.Log("creating nn..!! ONLY IN GameFieldDebug SCENE THIS SHOULD BE USED!");
            int[] top = { 12, 4, 2 };
            this.neuralController = new NeuralNetwork(top, 0);

        }
        distanceToBall = new List<float>();
        distanceToMyGoal = new List<float>();
        distanceToAdversaryGoal = new List<float>();
        distanceToAdversary = new List<float>();
        distancefromBallToAdversaryGoal = new List<float>();
        distancefromBallToMyGoal = new List<float>();
        distanceToClosestWall = new List<float>();
        agentSpeed = new List<float>();
        ballSpeed = new List<float>();
        advSpeed = new List<float>();
    }


    private void FixedUpdate()
    {
        if(countFrames == 0 && ball != null)
        {
            ballStartPos = ball.transform.localPosition;
            ballPreviousPos = ballStartPos;
        }


        simulationTime += Time.deltaTime;
        if (running && fixedUpdateCalls % 10 == 0)
        {
            // updating sensors
            SensorHandling();
            // move
            result = this.neuralController.process(sensorsInput);
            float angle = result[0] * 180;
            float strength = result[1];            
            Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up;
            dir.z = dir.y;
            dir.y = 0;



            /** DEBUG **
            // debug raycast for the force and angle being applied on the agent
            Vector3 rayDirection = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up;
            rayDirection.z = rayDirection.y;
            rayDirection.y = 0;
            
            if (strength > 0)
            {
                Debug.DrawRay(this.transform.position, -rayDirection.normalized * 5, Color.black);
            }
            else
            {
                Debug.DrawRay(this.transform.position, rayDirection.normalized * 5, Color.black);
            }*/

            agent.rb.AddForce(dir * strength * agent.speed); 
            

            // updating game status
            updateGameStatus();

            
            // check method
            if (endSimulationConditions())
            {
                wrapUp();
            }
            countFrames++;
        }
        fixedUpdateCalls++;
    }

    // The ambient variables are created here!
    public void SensorHandling()
    {

        Dictionary<string, ObjectInfo> objects = agent.objectsDetector.GetVisibleObjects();
        sensorsInput[0] = objects["DistanceToBall"].distance / FIELD_SIZE;
        sensorsInput[1] = objects["DistanceToBall"].angle / 360.0f;
        sensorsInput[2] = objects["MyGoal"].distance / FIELD_SIZE;
        sensorsInput[3] = objects["MyGoal"].angle / 360.0f;
        sensorsInput[4] = objects["AdversaryGoal"].distance / FIELD_SIZE;
        sensorsInput[5] = objects["AdversaryGoal"].angle / 360;
        if (objects.ContainsKey("Adversary"))
        {
            sensorsInput[6] = objects["Adversary"].distance / FIELD_SIZE;
            sensorsInput[7] = objects["Adversary"].angle / 360.0f;
        }
        else
        {
            sensorsInput[6] = -1;// -1 == não existe
            sensorsInput[7] = -1;// -1 == não existe
        }
        sensorsInput[8] = Mathf.CeilToInt(Vector3.Distance(ball.transform.localPosition, MyGoal.transform.localPosition)) / FIELD_SIZE; 
        sensorsInput[9] = Mathf.CeilToInt(Vector3.Distance(ball.transform.localPosition, AdversaryGoal.transform.localPosition)) / FIELD_SIZE; 
        sensorsInput[10] = objects["Wall"].distance / FIELD_SIZE;
        sensorsInput[11] = objects["Wall"].angle / 360.0f;

        ////// 
        // Agent speed and angle with previous position
        Vector2 pp = new Vector2(previousPos.x, previousPos.z);
        Vector2 aPos = new Vector2(agent.transform.localPosition.x, agent.transform.localPosition.z);
        aPos = aPos - pp;
        sensorsInput[12] = aPos.magnitude / FIELD_SIZE;
        sensorsInput[13] = Vector2.Angle(aPos, Vector2.right) / 360.0f;
        // Ball speed and angle with previous position
        pp = new Vector2(ballPreviousPos.x, ballPreviousPos.z);
        aPos = new Vector2(ball.transform.localPosition.x, ball.transform.localPosition.z);
        aPos = aPos - pp;
        sensorsInput[14] = aPos.magnitude / FIELD_SIZE;
        sensorsInput[15] = Vector2.Angle(aPos.normalized, Vector2.right) / 360.0f;
        // Adversary Speed and angle with previous position
        if (objects.ContainsKey("Adversary"))
        {
            Vector2 adp = new Vector2(advPreviousPos.x, advPreviousPos.z);
            Vector2 adPos = new Vector2(Adversary.transform.localPosition.x, Adversary.transform.localPosition.z);
            adPos = adPos - adp;
            sensorsInput[16] = adPos.magnitude / FIELD_SIZE;
            sensorsInput[17] = Vector2.Angle(adPos, Vector2.right) / 360.0f;
        }
        else
        {
            sensorsInput[16] = -1;
            sensorsInput[17] = -1;
        }

        if (countFrames % SampleRate == 0)
        {
            distanceToBall.Add(sensorsInput[0]);
            distanceToMyGoal.Add(sensorsInput[2]);
            distanceToAdversaryGoal.Add(sensorsInput[4]);
            distanceToAdversary.Add(sensorsInput[6]);
            distancefromBallToMyGoal.Add(sensorsInput[8]);
            distancefromBallToAdversaryGoal.Add(sensorsInput[9]);
            distanceToClosestWall.Add(sensorsInput[10]);
            // 
            agentSpeed.Add(sensorsInput[12]);
            ballSpeed.Add(sensorsInput[14]);
            advSpeed.Add(sensorsInput[16]);
        }
    }


    public void updateGameStatus()
    {
        // This is the information you can use to build the fitness function. 
        Vector2 pp = new Vector2(previousPos.x, previousPos.z);
        Vector2 aPos = new Vector2(agent.transform.localPosition.x, agent.transform.localPosition.z);
        currentDistance = Vector2.Distance(pp, aPos);
        distanceTravelled += currentDistance;
        hitTheBall = agent.hitTheBall;
        hitTheWall = agent.hitTheWall;

        // update positions!
        previousPos = agent.transform.localPosition;
        ballPreviousPos = ball.transform.localPosition;
        if (Adversary != null)
        {
            advPreviousPos = Adversary.transform.localPosition;
        }


        // get my score
        GoalsOnMyGoal = ScoreSystem.GetComponent<ScoreKeeper>().score[player == 0 ? 1 : 0];
        // get adversary score
        GoalsOnAdversaryGoal = ScoreSystem.GetComponent<ScoreKeeper>().score[player];


    }

    public void wrapUp()
    {
        avgSpeed = avgSpeed / simulationTime;
        gameOver = true;
        running = false;
        countFrames = 0;
        fixedUpdateCalls = 0;
    }

    public static float StdDev(IEnumerable<float> values)
    {
        float ret = 0;
        int count = values.Count();
        if (count > 1)
        {
            //Compute the Average
            float avg = values.Average();

            //Perform the Sum of (value-avg)^2
            float sum = values.Sum(d => (d - avg) * (d - avg));

            //Put it all together
            ret = Mathf.Sqrt(sum / count);
        }
        return ret;
    }

    //******************************************************************************************
    //* FITNESS AND END SIMULATION CONDITIONS *// 
    //******************************************************************************************
    private bool endSimulationConditions()
    {
        // You can modify this to change the length of the simulation of an individual before evaluating it.
        //maxSimulTime = 15.0f; //seconds
        return simulationTime > this.maxSimulTime;
    }

    public float GetScoreBlue()
    {
        // Fitness function for the Blue player. The code to attribute fitness to individuals should be written here.  
        //* YOUR CODE HERE*//

        switch (bluePopulationAptidaoMethod)
        {
            case TipoFuncaoAptidao.Atacante0:
                return aptidaoAtacanteMethod();
            case TipoFuncaoAptidao.GuardaRedes:
                return aptidaoGuardaRedesMethod();
            case TipoFuncaoAptidao.Atacante2:
                return aptidaoAtacante2Method();
        }
        return 0.0f;
    }

    public float GetScoreRed()
    {
        // Fitness function for the Red player. The code to attribute fitness to individuals should be written here. 
        switch (redPopulationAptidaoMethod)
        {
            case TipoFuncaoAptidao.Atacante0:
                return aptidaoAtacanteMethod();
            case TipoFuncaoAptidao.GuardaRedes:
                return aptidaoGuardaRedesMethod();
            case TipoFuncaoAptidao.Atacante2:
                return aptidaoAtacante2Method();
        }
        return 0.0f;
    }

    //DEFINICAO DOS VÁRIOS METODOS DE APTIDÃO A SEREM ESCOLHIDOS e USADOS 
    public float aptidaoAtacanteMethod()
    {
        float fitness = 0;

        fitness += distanceTravelled * 2.0f; //valorizar a distancia percorrida (é importante valorizar a distancia percorrida para q a rede neuronal perceba q há vantagens em o agente n ficar parado logo a seguir a ter marcado um golo, abrindo assim a possibilidade de serem marcados mais golos)

        //fitness += (GoalsOnAdversaryGoal / distanceTravelled) * 4000.0f;

        //fitness -= distanceToBall.Average() * 100.0f; //penalizar uma distancia média da bola superior 

        fitness += GoalsOnAdversaryGoal * 100000.0f; //valorizar bastante a marcação de golos
        fitness -= GoalsOnMyGoal * 200000.0f; //penalizar o sofrimento de golos (ou a marcação de auto-golos)

        fitness += (agentSpeed.Average() * 1000.0f) * 1000.0f; //valoriza-se uma velocidade média maior, do agente 
        fitness += (ballSpeed.Average() * 1000.0f) * 2000.0f; //valorizar a velocidade média da bola 

        //fitness += (hitTheBall / (hitTheWall + 1)) * (distanceTravelled / 2); 
        fitness += (hitTheBall) - hitTheWall;


        if (ballSpeed.Max() >= agentSpeed.Max()) //se a velocidade máxima atingida pela bola for superior à velocidade máxima atingida pelo agente
        {
            fitness += 5600.0f;
        }
        else
        {
            fitness -= 5600.0f;
        }
        if (distanceToAdversaryGoal.Average() > distanceToMyGoal.Average())
        {
            fitness += 10000.0f;
        }

        /*if (distancefromBallToAdversaryGoal.Min() <= distancefromBallToMyGoal.Min()) //bola mais proxima da baliza adversária
        {
            fitness += 1000.0f;
        }
        else 
        {
            fitness -= 500.0f;
        }*/

        if (distancefromBallToAdversaryGoal.Average() < distancefromBallToMyGoal.Average()) //bola passa mais tempo proxima da baliza adversária do que da propria baliza
        {
            fitness += 6500.0f;
        }
        else
        {
            fitness -= 6500.0f;
        }

        //if (fitness<0 || distanceToClosestWall.Average()==0 || distanceTravelled == 0 || distanceToMyGoal.Max() < 0.1052632f || hitTheBall == 0 || ballSpeed.Average() == 0.0f || agentSpeed.Average() == 0.0f)
        //    fitness = 0;
        //para relatorio: inicialmente estávamos a forçar o fitness=0 qnd algumas condições menos favoráveis eram detetadas como por exemplo o facto de o agente estar parado..
        //mas percebemos que dessa forma estaríamos a fazer com que fosse atribuído a imensos indivíduos da população um fitness de 0..deixando de haver assim uma distinção entre eles 
        //prejudicando bastante o resultado do algoritmo genético..muito provávelmente na escolhad e indivíduos ao nível do tournament selection...
        //percebemos então que seria mais vantajoso penalizar o agente para cada condição menos favorável detetada..

        //muito penalizador
        if (distanceToClosestWall.Average() == 0)
        {
            fitness -= 7500.0f; //penalizar se estiver demasiado tempo junto da parede
        }
        if (distanceTravelled == 0 || agentSpeed.Average() == 0.0f)
        {
            fitness -= 6000.0f;
        }
        if (ballSpeed.Average() == 0.0f || hitTheBall == 0)
        {
            fitness -= 25000.0f;
            if (distanceToAdversaryGoal.Average() < distanceToMyGoal.Average())
            {
                fitness -= 10000.0f;
            }
        }
        if (distanceToMyGoal.Max() < 0.1052632f)
        {
            fitness -= 6500.0f;
        }

        //Debug.Log(fitness);
        return fitness;
    }
    /*
    public float aptidaoAtacante1Method()
    {
        float fitness = 0;
        float next = 0;
        next = 10 / (5 + (distanceToAdversaryGoal.Average() * 10));
        Debug.Log(string.Format("Next: {0}    distance to adversary goal: {1}", next, distancefromBallToAdversaryGoal.Average()));
        fitness += next;

        next = distanceTravelled / 4;
        Debug.Log(string.Format("Next: {0}    distanceTravelled: {1}", next, agentSpeed.Average() * 950));
        fitness += next;

        next = (GoalsOnAdversaryGoal * 30 - GoalsOnMyGoal * 10);
        fitness += next;

        if (hitTheBall > 0) next = 6;
        Debug.Log(string.Format("Next: {0}    hit the ball: {1}", next, hitTheBall));
        fitness += next;

        if (distanceToMyGoal.Average() < 0.1052632)
            fitness -= distanceToMyGoal.Average() * 10 + 5;

        return fitness;
    }*/
    public float aptidaoAtacante2Method()
    {
        float fitness = 0;

        fitness += distanceTravelled * 2.0f; //valorizar a distancia percorrida (é importante valorizar a distancia percorrida para q a rede neuronal perceba q há vantagens em o agente n ficar parado logo a seguir a ter marcado um golo, abrindo assim a possibilidade de serem marcados mais golos)
        fitness += (GoalsOnAdversaryGoal / distanceTravelled) * 4000.0f;


        fitness += GoalsOnAdversaryGoal * 20000.0f; //valorizar bastante a marcação de golos
        fitness += -GoalsOnMyGoal * 30000.0f; //penalizar o sofrimento de golos (ou a marcação de auto-golos)

        fitness += (agentSpeed.Average() * 1000.0f) * 100.0f; //valoriza-se uma velocidade média maior, do agente 
        fitness += (ballSpeed.Average() * 1000.0f) * 500.0f; //valorizar a velocidade média da bola 


        if (ballSpeed.Max() >= agentSpeed.Max()) //se a velocidade máxima atingida pela bola for superior à velocidade máxima atingida pelo agente
        {
            fitness += 1500.0f;
        }
        else
        {
            fitness -= 750.0f;
        }


        if (distancefromBallToAdversaryGoal.Average() < distancefromBallToMyGoal.Average()) //bola passa mais tempo proxima da baliza adversária do que da propria baliza
        {
            fitness += 2500.0f;
        }
        else
        {
            fitness -= 2500.0f;
        }

        //muito penalizador
        if (distanceToClosestWall.Average() == 0)
        {
            fitness -= 5500.0f; //penalizar se estiver demasiado tempo junto da parede
        }
        if (distanceTravelled == 0 || agentSpeed.Average() == 0.0f)
        {
            fitness -= 4000.0f;
        }
        if (ballSpeed.Average() == 0.0f || hitTheBall == 0)
        {
            fitness -= 4500.0f;
        }
        if (distanceToMyGoal.Max() < 0.1052632f)
        {
            fitness -= 4500.0f;
        }

        //Debug.Log(fitness);
        return fitness;
    }
    public float aptidaoGuardaRedesMethod() {
        float fitness = 0;
        
        if (distanceTravelled==0) fitness-= 500;
        else fitness += distanceTravelled * 10;

        //  se o individuo defendeu a bola
        if (GoalsOnMyGoal == 0) fitness += 99999;
        else fitness -= 1000;

        //  incentivar o agente a avançar 
        if (distanceToAdversaryGoal.Min() <= 1) fitness += 150;

        //  se o individuo estiver dentro da baliza
        if (distanceToMyGoal.Average() <= 0.5) fitness -= 5000;

        if (hitTheBall==0) fitness -= 99999;
        else fitness += hitTheBall * 5000;

        fitness += 5000 * distancefromBallToMyGoal.Min();
        fitness += 500 * agentSpeed.Average();
        fitness += 200 / (distanceToBall.Min() + 1);
        
        return fitness;
    }

}
