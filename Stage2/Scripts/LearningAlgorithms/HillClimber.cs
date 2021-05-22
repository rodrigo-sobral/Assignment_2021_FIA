using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HillClimber : MetaHeuristic
{
    [Header("Red Population Parameters")]
    public float mutationProbabilityRedPopulation;

    [Header("Blue Population Parameters")]
    public float mutationProbabilityBluePopulation;





    public override void InitPopulation(){

        GamesPerIndividualForEvaluation = Mathf.Min(GamesPerIndividualForEvaluation, populationSize);


        populationRed = new List<Individual> ();
        populationBlue = new List<Individual>();

        while (populationRed.Count < populationSize) {
			HillClimberIndividual new_ind_red = new HillClimberIndividual (NNTopology, GamesPerIndividualForEvaluation, mutationMethod);
            HillClimberIndividual new_ind_blue = new HillClimberIndividual(NNTopology, GamesPerIndividualForEvaluation, mutationMethod);
            if (seedPopulationFromFile)
            {
                NeuralNetwork nnRed = getRedIndividualFromFile();
                NeuralNetwork nnBlue = getBlueIndividualFromFile();
                new_ind_red.Initialize(nnRed);
                new_ind_blue.Initialize(nnBlue);
                //only the first individual is an exact copy. the other are going to suffer mutations
                if (populationRed.Count != 0 && populationBlue.Count != 0)
                {
                    new_ind_red.Mutate(mutationProbabilityRedPopulation);
                    new_ind_blue.Mutate(mutationProbabilityBluePopulation);
                }

            }
            else
            {
                new_ind_red.Initialize();
                new_ind_blue.Initialize();
            }

            populationRed.Add (new_ind_red);
            populationBlue.Add(new_ind_blue);
        }

	}

	//The Step function assumes that the fitness values of all the individuals in the population have been calculated.
	public override void Step()
	{
		List<Individual> newPopRed = new List<Individual> ();
        List<Individual> newPopBlue = new List<Individual>();

        updateReport (); //called to get some stats
		// fills the rest with mutations of the best !
		for (int i = 0; i < populationSize ; i++) {
			HillClimberIndividual tmpRed = (HillClimberIndividual) overallBestRed.Clone ();
            HillClimberIndividual tmpBlue = (HillClimberIndividual) overallBestBlue.Clone();
            tmpRed.Mutate (mutationProbabilityRedPopulation);
            tmpBlue.Mutate(mutationProbabilityBluePopulation);
            newPopRed.Add (tmpRed.Clone());
            newPopBlue.Add(tmpBlue.Clone());
        }

		populationRed = newPopRed;
        populationBlue = newPopBlue;

        generation++;
	}

}

