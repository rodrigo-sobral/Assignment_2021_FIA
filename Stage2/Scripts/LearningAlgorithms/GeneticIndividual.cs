using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MetaHeuristic;

public class GeneticIndividual : Individual {
    public float mean = 0.0f;
    public float stdev = 0.5f;

    public GeneticIndividual(int[] topology, int numberOfEvaluations, MutationType mutation) : base(topology, numberOfEvaluations, mutation) {
	}

	public override void Initialize () 
	{
		for (int i = 0; i < totalSize; i++)
		{
			genotype[i] = Random.Range(-1.0f, 1.0f);
		}
	}

   public override void Initialize(NeuralNetwork nn)
    {
        int count = 0;
        for (int i = 0; i < topology.Length - 1; i++)
        {
            for (int j = 0; j < topology[i]; j++)
            {
                for (int k = 0; k < topology[i + 1]; k++)
                {
                    genotype[count++] = nn.weights[i][j][k];
                }

            }
        }
    }

    public override Individual Clone()
    {
        GeneticIndividual new_ind = new GeneticIndividual(this.topology, this.maxNumberOfEvaluations, this.mutation);

        genotype.CopyTo(new_ind.genotype, 0);
        new_ind.fitness = this.Fitness;
        new_ind.evaluated = false;

        return new_ind;
    }


    public override void Mutate(float probability)
    {
        switch (mutation)
        {
            case MetaHeuristic.MutationType.Gaussian:
                MutateGaussian(probability);
                break;
            case MetaHeuristic.MutationType.Random:
                MutateRandom(probability);
                break;
        }
    }
    public void MutateRandom(float probability)
    {
        for (int i = 0; i < totalSize; i++)
        {
            if (Random.Range(0.0f, 1.0f) < probability)
            {
                genotype[i] = Random.Range(-1.0f, 1.0f);
            }
        }
    }

    public void MutateGaussian(float probability)
    {
        int i;
        for (i = 0; i < totalSize; i++)
        {
            if (Random.Range(0.0f, 1.0f) < probability)
            {
                genotype[i] = genotype[i] + NextGaussian(mean, stdev);
            }
        }

    }

    public override void Crossover(Individual partner, float probability)
    {   /* Nota: O crossover deverá alterar ambos os indivíduos */
        if (Random.Range(0.0f, 1.0f) < probability)
        {
            int crossoverPoint = Random.Range(0, totalSize); //returns num between 0 and totalSize-1
            //genotypes after the crossoverPoint are swapped
            float aux;
            for (int i = crossoverPoint; i < totalSize; i++) //totalSize==tamanho do genotype
            {   //swap genotypes
                aux = genotype[i];
                genotype[i] = partner[i];
                partner[i] = aux;
            }
        }
    }


}
