using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class GeneticAlgorithm : MetaHeuristic
{

    public int tournamentSize;

    [Header("Red Population Parameters")]
    public float mutationProbabilityRedPopulation;
    public float crossoverProbabilityRedPopulation;
    public bool elitistRed = true;

    [Header("Blue Population Parameters")]
    public float mutationProbabilityBluePopulation;
    public float crossoverProbabilityBluePopulation;
    public bool elitistBlue = true;

    public override void InitPopulation()
    {
        GamesPerIndividualForEvaluation = Mathf.Min(GamesPerIndividualForEvaluation, populationSize);
        populationRed = new List<Individual>();
        populationBlue = new List<Individual>();

        // init get individuals from folder
        List<string> redfiles = new List<string>();
        int redIndex = 0;

        if (seedRedPopulationFromFolder)
        {
            foreach (string file in Directory.EnumerateFiles(redFolderPath, "*.dat"))
            {
                redfiles.Add(file);
            }

            Debug.Log(string.Join(" ", redfiles));
            if (redfiles.Count == 0)
            {
                Debug.LogError("RED FILES NOT FOUND");
            }
        }
        List<string> bluefiles = new List<string>();
        int blueIndex = 0;
        if (seedBluePopulationFromFolder)
        {

            foreach (string file in Directory.EnumerateFiles(blueFolderPath, "*.dat"))
            {
                bluefiles.Add(file);
            }

            Debug.Log(string.Join(" ", bluefiles));
            if (bluefiles.Count == 0)
            {
                Debug.LogError("BLUE FILES NOT FOUND");
            }
        }
        // end init get individuals from folder


        while (populationRed.Count < populationSize)
        {
            GeneticIndividual new_ind_red = new GeneticIndividual(NNTopology, GamesPerIndividualForEvaluation, mutationMethod);
            GeneticIndividual new_ind_blue = new GeneticIndividual(NNTopology, GamesPerIndividualForEvaluation, mutationMethod);

            if (seedPopulationFromFile)
            {
                NeuralNetwork nnRed = getRedIndividualFromFile();
                NeuralNetwork nnBlue = getBlueIndividualFromFile();
                new_ind_red.Initialize(nnRed);
                new_ind_blue.Initialize(nnBlue);
                //only the first individual is an exact copy. the others are going to suffer mutations
                if (populationRed.Count != 0 && populationBlue.Count != 0)
                {
                    new_ind_red.Mutate(mutationProbabilityRedPopulation);
                    new_ind_blue.Mutate(mutationProbabilityBluePopulation);
                }

            }
            else
            {
                // Load from folder
                if (seedRedPopulationFromFolder)
                {
                    Debug.Log("Loading Red :" + redfiles[redIndex % redfiles.Count]);
                    NeuralNetwork nnRed = getIndividualFromFile(redfiles[redIndex % redfiles.Count]);
                    new_ind_red.Initialize(nnRed);

                    //first loads are copies the rest are mutations
                    if (redIndex >= redfiles.Count)
                    {
                        new_ind_red.Mutate(mutationProbabilityRedPopulation);
                        Debug.Log("Mutated Red Index" + redIndex + " : " + new_ind_red);
                    }
                    else
                    {
                        Debug.Log("Original Red Index" + redIndex + " : " + new_ind_red);
                    }
                    redIndex++;
                }
                else
                {
                    // no seeding from file or folder
                    new_ind_red.Initialize();

                }

                if (seedBluePopulationFromFolder)
                {
                    Debug.Log("Loading Blue:" + bluefiles[blueIndex % bluefiles.Count]);
                    NeuralNetwork nnBlue = getIndividualFromFile(bluefiles[blueIndex % bluefiles.Count]);
                    new_ind_blue.Initialize(nnBlue);
                    //first loads are copies the rest are mutations
                    if (blueIndex >= bluefiles.Count)
                    {
                        new_ind_blue.Mutate(mutationProbabilityBluePopulation);
                        Debug.Log("Mutated Blue Index" + blueIndex + " : " + new_ind_blue);
                    }
                    else
                    {
                        Debug.Log("Original Blue Index" + blueIndex + " : " + new_ind_blue);
                    }
                    blueIndex++;
                }
                else
                {
                    // no seeding from file or folder
                    new_ind_blue.Initialize();

                }
            }

            // add to population
            populationRed.Add(new_ind_red);
            populationBlue.Add(new_ind_blue);
        }


        switch (selectionMethod)
        {
            case SelectionType.Tournament:
                selection = new TournamentSelection(tournamentSize);
                break;
        }
    }

    //The Step function assumes that the fitness values of all the individuals in the population have been calculated.
    public override void Step()
    {
        updateReport(); //called to get some stats

        // update the generation before creating the new pop
        generation++;
        if (generation == numberOfGenerations) // no need to execute the step if we reached max generation
            return;
        
        List<Individual> newPopRed;
        List<Individual> newPopBlue;

        //Select parents
        newPopRed = selection.selectIndividuals(populationRed, populationSize);
        newPopBlue = selection.selectIndividuals(populationBlue, populationSize);

        //Crossover
        for (int i = 0; i < populationSize - 1; i += 2)
        {
            Individual parent1Red = newPopRed[i];
            Individual parent2Red = newPopRed[i + 1];

            Individual parent1Blue = newPopBlue[i];
            Individual parent2Blue = newPopBlue[i + 1];

            parent1Red.Crossover(parent2Red, crossoverProbabilityRedPopulation);
            parent1Blue.Crossover(parent2Blue, crossoverProbabilityBluePopulation);
        }

        //Mutation 
        for (int i = 0; i < populationSize; i++)
        {
            newPopRed[i].Mutate(mutationProbabilityRedPopulation);
            newPopBlue[i].Mutate(mutationProbabilityBluePopulation);

        }

        if (elitistRed)
        {
            Individual tmpRed = overallBestRed.Clone();
            newPopRed[0] = tmpRed;
        }
        if (elitistBlue)
        {

            Individual tmpBlue = overallBestBlue.Clone();
            newPopBlue[0] = tmpBlue;
        }

        populationRed = newPopRed;
        populationBlue = newPopBlue;
    }


    

}