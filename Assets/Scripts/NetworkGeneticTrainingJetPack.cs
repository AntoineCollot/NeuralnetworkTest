using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NeuralNetwork;

public class NetworkGeneticTrainingJetPack : MonoBehaviour {

    bool runEvolution;

    [SerializeField]
    int targetScore;

    [Header("Networks")]

    public int population;

    [SerializeField]
    InputLayer input;

    [SerializeField]
    JetPack output;

    [SerializeField]
    NeuralNetwork.Network neuralNetworkPrefab;

    List<NetworkIndividual> networkPopulation = new List<NetworkIndividual>();

    [Header("Mutations")]

    [Range(0,1)]
    [SerializeField]
    float mutateChance;

    [Range(0, 10)]
    [SerializeField]
    int geneToMutate;

    [Header("Breeding")]

    [Range(0, 1)]
    [SerializeField]
    float deathRatio;

    [Header("UI")]

    [SerializeField]
    GenerationInfoPanelManager infoPanelManager;

    [SerializeField]
    Button runButton;

    [SerializeField]
    Button runOneButton;

    [SerializeField]
    Graph bestCurve;

    [SerializeField]
    Graph averageCurve;


    int generation = 0;

    // Use this for initialization
    void Start () {
        GenerateNewPopulation();
        averageCurve.curve.maxValue = targetScore;
        bestCurve.curve.maxValue = targetScore;
    }

    [ContextMenu("Generate New Population")]
    public void GenerateNewPopulation()
    {
        for (int i = 0; i < population; i++)
        {
            CreateNewNetwork();
        }
    }

    [ContextMenu("RunEvolution")]
    public void RunGeneticEvolution()
    {
        runEvolution = true;
        runButton.interactable = false;
        runOneButton.interactable = false;
        StartCoroutine(RunEvolutionLoop());
    }

    public void StopEvolution()
    {
        runButton.interactable = true;
        runOneButton.interactable = true;
        runEvolution = false;
    }

    [ContextMenu("RunEvolution")]
    public void RunGeneticEvolutionOneRound()
    {
        StartCoroutine(RunSingleEvolutionRound());
    }

    IEnumerator RunEvolutionLoop()
    {
        while(runEvolution)
        {
            yield return StartCoroutine(RunSingleEvolutionRound());

            //Export the best individual each 10 generations
            if(generation%10==0)
            {
                ExportBest();
            }

            if(networkPopulation[0].fitness>targetScore)
            {
                print("Best of generation " + generation + " reached target, and thus has been exported");
                ExportBest();
                StopEvolution();
            }
        }
    }

    public IEnumerator RunSingleEvolutionRound()
    {
        generation++;

        //Run the simulation for each individual
        for (int i = 0; i < networkPopulation.Count; i++)
        {
            yield return StartCoroutine(Simulation.Instance.RunSimulation(networkPopulation[i].network,0));
            networkPopulation[i].fitness = Simulation.Instance.step;
        }

        //Sort the list by fitness
        networkPopulation.Sort(
            delegate (NetworkIndividual p1, NetworkIndividual p2)
            {
                return p2.fitness.CompareTo(p1.fitness);
            }
        );

        //Kill the weak individuals
        int numberToKill = (int)(population * deathRatio);
        Queue<NetworkIndividual> killedIndividual = new Queue<NetworkIndividual>();
        for (int i = 0; i < numberToKill; i++)
        {
            //Some survive by luck
            if (Random.Range(0.0f, 1.0f) > 0.05f)
            {
                killedIndividual.Enqueue(networkPopulation[networkPopulation.Count - 1]);
                networkPopulation.RemoveAt(networkPopulation.Count - 1);
            }
        }

        //Calculate the average fitness for ui
        float average = 0;
        foreach (NetworkIndividual i in networkPopulation)
        {
            average += i.fitness;
        }
        average /= networkPopulation.Count;


        int mutatedCount = 0;
        //Mutate some random individuals
        foreach(NetworkIndividual i in networkPopulation)
        {
            if (mutateChance > Random.Range(0.0f, 1.0f))
            {
                MutateGene(i.network);
                mutatedCount++;
            }
        }

        //Breed
        int survivorsCount = networkPopulation.Count;
        int childrenToProduce = population - survivorsCount;
        for (int i = 0; i < childrenToProduce; i++)
        {
            //Produce a child with two random survivors
            NetworkIndividual child = killedIndividual.Dequeue();
            networkPopulation.Add(child);
            Breed(networkPopulation[Random.Range(0, survivorsCount)].network, networkPopulation[Random.Range(0, survivorsCount)].network, child.network);
        }

        //Display an information panel about this generation
        infoPanelManager.NewGeneration(generation, networkPopulation[0].fitness,(int)average, mutatedCount, childrenToProduce, networkPopulation.Count);

        //Update the curves
        averageCurve.AddPoint(average);
        bestCurve.AddPoint(networkPopulation[0].fitness);
    }

    void MutateGene(NeuralNetwork.Network networkToMutate)
    {
        int geneCount = networkToMutate.GetGeneCount();

        for (int i = 0; i < geneToMutate; i++)
        {
            int geneToTransferId = Random.Range(0, geneCount);

            //Look in HiddenLayers
            for (int l = 0; l < networkToMutate.hiddenLayers.Length; l++)
            {
                for (int n = 0; n < networkToMutate.hiddenLayers[l].neurons.Length; n++)
                {
                    int neuronGeneCount = networkToMutate.hiddenLayers[l].neurons[n].GetGeneCount();

                    //If the gene isn't in this neuron, continue to search
                    if (geneToTransferId >= neuronGeneCount)
                        geneToTransferId -= neuronGeneCount;
                    //If it's the last one, transfer the activation value
                    else if (geneToTransferId == neuronGeneCount - 1)
                    {
                        networkToMutate.hiddenLayers[l].neurons[n].activationValue += Random.Range(-1, 1);
                        geneToTransferId = -1;
                        break;
                    }
                    //Else tranfer the weight
                    else
                    {
                        networkToMutate.hiddenLayers[l].neurons[n].weights[geneToTransferId] += Random.Range(-1, 1);
                        geneToTransferId = -1;
                        break;
                    }
                }
                if (geneToTransferId < 0)
                    break;
            }

            //Look in the output layer
            if (geneToTransferId >= 0)
            {
                for (int n = 0; n < networkToMutate.outputLayer.neurons.Length; n++)
                {
                    int neuronGeneCount = networkToMutate.outputLayer.neurons[n].GetGeneCount();

                    //If the gene isn't in this neuron, continue to search
                    if (geneToTransferId >= neuronGeneCount)
                    {
                        geneToTransferId -= neuronGeneCount;
                    }
                    //If it's the last one, transfer the activation value
                    else if (geneToTransferId == neuronGeneCount - 1)
                    {
                        networkToMutate.outputLayer.neurons[n].activationValue += Random.Range(-1, 1);
                        geneToTransferId = -1;
                        break;
                    }
                    //Else tranfer the weight
                    else
                    {
                        networkToMutate.outputLayer.neurons[n].weights[geneToTransferId] += Random.Range(-1, 1);
                        geneToTransferId = -1;
                        break;
                    }
                }
            }
        }
    }

    void Breed(NeuralNetwork.Network mother, NeuralNetwork.Network father, NeuralNetwork.Network child)
    {
        //For each hidden layer
        for (int l = 0; l < mother.hiddenLayers.Length; l++)
        {
            //For each neuron in the layer
            for (int n = 0; n < mother.hiddenLayers[l].neurons.Length; n++)
            {
                //For each weights
                for (int w = 0; w < mother.hiddenLayers[l].neurons[n].weights.Length; w++)
                {
                    //Take randomly the gene from the father or the mother
                    if (Random.Range(0.0f, 1.0f) > 0.5f)
                        child.hiddenLayers[l].neurons[n].weights[w] = mother.hiddenLayers[l].neurons[n].weights[w];
                    else
                        child.hiddenLayers[l].neurons[n].weights[w] = father.hiddenLayers[l].neurons[n].weights[w];
                }

                //Same for activation value of the neuron
                if (Random.Range(0.0f, 1.0f) > 0.5f)
                    child.hiddenLayers[l].neurons[n].activationValue = mother.hiddenLayers[l].neurons[n].activationValue;
                else
                    child.hiddenLayers[l].neurons[n].activationValue = father.hiddenLayers[l].neurons[n].activationValue;
            }
        }

        ///Output Layer
        //For each neuron in the layer
        for (int n = 0; n < mother.outputLayer.neurons.Length; n++)
        {
            //For each weights
            for (int w = 0; w < mother.outputLayer.neurons[n].weights.Length; w++)
            {
                //Take randomly the gene from the father or the mother
                if (Random.Range(0.0f, 1.0f) > 0.5f)
                    child.outputLayer.neurons[n].weights[w] = mother.outputLayer.neurons[n].weights[w];
                else
                    child.outputLayer.neurons[n].weights[w] = father.outputLayer.neurons[n].weights[w];
            }

            //Same for activation value of the neuron
            if (Random.Range(0.0f, 1.0f) > 0.5f)
                child.outputLayer.neurons[n].activationValue = mother.outputLayer.neurons[n].activationValue;
            else
                child.outputLayer.neurons[n].activationValue = father.outputLayer.neurons[n].activationValue;
        }
    }

    NeuralNetwork.Network CreateNewNetwork()
    {
        //Create a new 
        NeuralNetwork.Network newNetwork = Instantiate(neuralNetworkPrefab, transform);
        newNetwork.inputLayer = input;
        newNetwork.onOutput.AddListener(output.SetFly);
        newNetwork.InitializeConnections();

        //Add the new network to the list of networks
        NetworkIndividual newIndividual = new NetworkIndividual();
        newIndividual.network = newNetwork;
        networkPopulation.Add(newIndividual);

        return newNetwork;
    }


    public void ReplacePopulation(List<NeuralNetwork.Network> loadedNetworks)
    {
        //Clear the population
        while (networkPopulation.Count > 0)
        {
            Destroy(networkPopulation[0].network.gameObject);
            networkPopulation.RemoveAt(0);
        }

        //Load the new population
        population = loadedNetworks.Count;

        for (int i = 0; i < population; i++)
        {
            NetworkIndividual newIndividual = new NetworkIndividual();
            newIndividual.network = loadedNetworks[i];
            loadedNetworks[i].inputLayer = input;
            loadedNetworks[i].onOutput.AddListener(output.SetFly);
            networkPopulation.Add(newIndividual);
        }
    }

    public void ExportBest()
    {
        NetworkExporter.Export(networkPopulation[0].network, "BestIndividualGeneration_" + generation+" ["+ networkPopulation[0].fitness+"]");
    }

    public void ExportGeneration()
    {
        NeuralNetwork.Network[] generationPopulation = new NeuralNetwork.Network[networkPopulation.Count];

        for (int i = 0; i < networkPopulation.Count; i++)
        {
            generationPopulation[i] = networkPopulation[i].network;
        }

        NetworkExporter.ExportGeneration(generationPopulation, generation);
    }

    [ContextMenu("RunSimulation")]
    public void TrySimulation()
    {
        Simulation.Instance.RunSimulation(neuralNetworkPrefab, 0.01f);
    }

    public class NetworkIndividual
    {
        public NeuralNetwork.Network network;
        public int fitness;
    }
}
