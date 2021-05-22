using System;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class NeuralNetwork {

	public float [][][] weights;
	public int [] parameters;

	public int networkSize;

	void initializeVariables()
	{
		this.weights = new float[parameters.Length - 1][][];
		this.networkSize = parameters.Length;
	}

    public float LogisticFunction(float x)
    {
        return 1 / (1 + Mathf.Exp((-1) * x));

    }


    public NeuralNetwork(int [] parameters)
	{
		//Pos 0: input
		//Last Pos: output
		//In between: size of the hidden layers
		//{3,5,2} -> 3 inputs, 1 hidden with 5 neurons, 2 outputs
		this.parameters = parameters;
		initializeVariables ();
	}

	public NeuralNetwork(int [] parameters, int random) {
		//Pos 0: input
		//Last Pos: output
		//In between: size of the hidden layers
		//{3,5,2} -> 3 inputs, 1 hidden with 5 neurons, 2 outputs
		this.parameters = parameters;
		initializeVariables ();
		for (int i = 0; i < parameters.Length - 1 ; i++) {

			weights[i] = new float[parameters[i]][];

			for (int j = 0; j < parameters [i]; j++) {

				weights[i][j] =  new float[parameters[i+1]];

				for (int k = 0; k < parameters [i + 1]; k++) {

					weights [i] [j] [k] = getRandomWeight ();
				}

			}
		}

	}

	public void map_from_linear(float[] geno) {
		int counter = 0;
		for (int i = 0; i < parameters.Length - 1 ; i++) {
			weights[i] = new float[parameters[i]][];
			for (int j = 0; j < parameters [i]; j++) {
				weights[i][j] =  new float[parameters[i+1]];
				for (int k = 0; k < parameters [i + 1]; k++) {
					weights [i] [j] [k] = geno[counter++];
				}

			}
		}	
	}

	public float [] process(float [] inputs)
	{
		
		if (inputs.Length != parameters [0]) {
			Debug.Log ("Input lenght does not match the number of neurons in the input layer!");
			return null;
		}

		float[] outputs;
		//for each layer
		for (int i = 0; i < (networkSize-1); i++) {
			outputs = new float[parameters [i+1]];


			//for each input neuron
			for (int j = 0; j < inputs.Length; j++) {

				//and for each output neuron
				for (int k = 0; k < outputs.Length; k++) {

					outputs [k] += inputs [j] * weights [i] [j] [k];
				}
			}
				
			inputs = new float[outputs.Length];

			for (int l = 0; l < outputs.Length; l++) {
                // LeakyReLu
                inputs [l] = Mathf.Max((float)(0.01 * outputs[l]), outputs [l]);
            }

		}
		//these will be the values of the last layer
        for(int i = 0; i < inputs.Length; i++)
        {
            inputs[i] = (float)Math.Tanh(inputs[i]); //  tanh
		}
		return inputs;
	}

	public override string ToString () {
		string output = "";
		for (int i = 0; i < weights.Length ; i++) {
			for (int j = 0; j < weights[i].Length; j++) {
				output += "Weights Layer " + i + "\n";
				for (int k = 0; k < weights[i][j].Length; k++) {
					output += weights [i] [j] [k] + " "; 
				}
				output += "\n";
			}
		}
			
		return output;
	}
		

	float getRandomWeight()
	{
		return (float) Random.Range(-0.5f, 0.5f);              
	}

}
