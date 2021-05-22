using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public abstract class Individual {

	protected float[] genotype;
	protected MetaHeuristic.MutationType mutation;
	protected int[] topology;

	protected int totalSize = 0;
	protected float fitness;
    protected List<float> evaluations;
    protected bool evaluated;
	protected NeuralNetwork network;
	[HideInInspector]
	protected int completedEvaluations = 0;
	[HideInInspector]
	protected int maxNumberOfEvaluations = 1;

	public int Size
	{
		get { return totalSize;}
	}

	public float Fitness
	{
	    get { return fitness;}
	
	}

	public float this[int i]
	{
		get { return genotype[i]; }
		set { genotype[i] = value; }
	}

	public void SetEvaluations(float quality)
    {
        evaluations.Insert(completedEvaluations, quality);
        completedEvaluations++;
        if(completedEvaluations == maxNumberOfEvaluations)
        {
            completedEvaluations = 0;
            fitness = evaluations.Average();
            evaluated = true;
        }
    }

    public bool Evaluated
	{
		get { return evaluated;}
	}


	public Individual(int[] topology, int numberOfEvaluations, MetaHeuristic.MutationType mutation) {
		for (int i = 1; i < topology.Length; i++)
		{
			totalSize += topology[i - 1] * topology[i];
		}
		this.topology = topology;
        fitness = 0.0f;
        maxNumberOfEvaluations = numberOfEvaluations;
        evaluations = new List<float>(numberOfEvaluations);
		evaluated = false;
        completedEvaluations = 0;
		genotype = new float[totalSize];
		//
		this.mutation = mutation;
	}


	public NeuralNetwork getIndividualController() {
		network = new NeuralNetwork (topology);
		network.map_from_linear (genotype);
		return network;
	}

	public override string ToString () {
		if (network == null) {
			getIndividualController ();
		}
		string res = "[GeneticIndividual]: [";
		for (int i = 0; i < totalSize; i++) {
			res += genotype [i].ToString ();
			if (i != totalSize - 1) {
				res += ",";
			}
		}
		res += "]\n";
		res += "Neural Network\n" + network.ToString() + "\n";
		return res;

	}

    public static float NextGaussian(float mean, float standard_deviation)
    {
        return mean + NextGaussian() * standard_deviation;
    }


    public static float NextGaussian()
    {
        float v1, v2, s;
        do
        {
            v1 = 2.0f * Random.Range(0f, 1f) - 1.0f;
            v2 = 2.0f * Random.Range(0f, 1f) - 1.0f;
            s = v1 * v1 + v2 * v2;
        } while (s >= 1.0f || s == 0f);

        s = Mathf.Sqrt((-2.0f * Mathf.Log(s)) / s);

        return v1 * s;
    }


    //override on each specific individual class
    public abstract void Initialize ();
    public abstract void Initialize(NeuralNetwork nn);
    public abstract void Mutate (float probability);
	public abstract void Crossover (Individual partner, float probability);
	public abstract Individual Clone ();
}
