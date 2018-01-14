using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NeuralNetwork;

public class NetworkGeneticTrainingJetPack : MonoBehaviour {

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

    [Range(0,10)]
    [SerializeField]
    int geneToTransfer;

    [Range(0, 10)]
    [SerializeField]
    int geneToMutate;

    // Use this for initialization
    void Start () {
       // TrySimulation();
    }

    [ContextMenu("Generate New Population")]
    public void GenerateNewPopulation()
    {
        for (int i = 0; i < population; i++)
        {
            NeuralNetwork.Network newNetwork = Instantiate(neuralNetworkPrefab, transform);
            newNetwork.inputLayer = input;
            newNetwork.onOutput.AddListener(output.SetFly);

            //Add the new network to the list of networks
            NetworkIndividual newIndividual = new NetworkIndividual();
            newIndividual.network = newNetwork;
            networkPopulation.Add(newIndividual);
        }
    }

    [ContextMenu("RunEvolution")]
    public void RunGeneticEvolution()
    {
        StartCoroutine(RunSingleEvolutionRound());
    }

    public IEnumerator RunSingleEvolutionRound()
    {
        //Run the simulation for each individual
        for (int i = 0; i < networkPopulation.Count; i++)
        {
            yield return StartCoroutine(Simulation.Instance.RunSimulation(networkPopulation[i].network,0));
            networkPopulation[i].score = Simulation.Instance.step;
            print("Network "+i.ToString()+" : "+Simulation.Instance.step);
        }

        //Create a new list of the individual to pick randomly from
        List<NetworkIndividual> candidates = new List<NetworkIndividual>(networkPopulation);

        while(candidates.Count>1)
        {
            //Pick a frist contestant and remove it from the list
            int randomId = Random.Range(0, candidates.Count);
            NetworkIndividual contestant1 = candidates[randomId];
            candidates.RemoveAt(randomId);

            //Pick a second contestant and remove it from the list
            randomId = Random.Range(0, candidates.Count);
            NetworkIndividual contestant2 = candidates[randomId];
            candidates.RemoveAt(randomId);

            //Mutate the looser based on the winner DNA
            if (contestant1.score > contestant2.score)
            {
                print("Mutate " + contestant2.score.ToString() + " based on " + contestant1.score.ToString());
                Mutate(contestant2.network, contestant1.network);
            }
            else
            {
                print("Mutate " + contestant1.score.ToString() + " based on " + contestant2.score.ToString());
                Mutate(contestant1.network, contestant2.network);
            }
        }
    }

    void Mutate(NeuralNetwork.Network weakIndividual, NeuralNetwork.Network strongindividual)
    {
        int geneCount = strongindividual.GetGeneCount();

        TransferGene(strongindividual, weakIndividual, geneCount);
        MutateGene(weakIndividual, geneCount);
    }

    void TransferGene(NeuralNetwork.Network from, NeuralNetwork.Network to,int geneCount)
    {
        for (int i = 0; i < geneToTransfer; i++)
        {
            int geneToTransferId = Random.Range(0, geneCount);

            //Look in HiddenLayers
            for (int l = 0; l < from.hiddenLayers.Length; l++)
            {
                for (int n = 0; n < from.hiddenLayers[l].neurons.Length; n++)
                {
                    int neuronGeneCount = from.hiddenLayers[l].neurons[n].GetGeneCount();

                    //If the gene isn't in this neuron, continue to search
                    if (geneToTransferId >= neuronGeneCount)
                        geneToTransferId -= neuronGeneCount;
                    //If it's the last one, transfer the activation value
                    else if (geneToTransferId == neuronGeneCount - 1)
                    {
                        print("transfered hidden activation value");
                        to.hiddenLayers[l].neurons[n].activationValue = from.hiddenLayers[l].neurons[n].activationValue;
                        geneToTransferId = -1;
                        break;
                    }
                    //Else tranfer the weight
                    else
                    {
                        print("transfered hidden weight");
                        to.hiddenLayers[l].neurons[n].weights[geneToTransferId] = from.hiddenLayers[l].neurons[n].weights[geneToTransferId];
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
                for (int n = 0; n < from.outputLayer.neurons.Length; n++)
                {
                    int neuronGeneCount = from.outputLayer.neurons[n].GetGeneCount();

                    //If the gene isn't in this neuron, continue to search
                    if (geneToTransferId >= neuronGeneCount)
                    {
                        print("ERROR");
                        geneToTransferId -= neuronGeneCount;
                    }
                    //If it's the last one, transfer the activation value
                    else if (geneToTransferId == neuronGeneCount - 1)
                    {
                        print("transfered out activation value");
                        to.outputLayer.neurons[n].activationValue = from.outputLayer.neurons[n].activationValue;
                        geneToTransferId = -1;
                        break;
                    }
                    //Else tranfer the weight
                    else
                    {
                        print("transfered out weight");
                        to.outputLayer.neurons[n].weights[geneToTransferId] = from.outputLayer.neurons[n].weights[geneToTransferId];
                        geneToTransferId = -1;
                        break;
                    }
                }
            }
        }
    }

    void MutateGene(NeuralNetwork.Network networkToMutate,int geneCount)
    {
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
                        print("mutated hidden activation value");
                        networkToMutate.hiddenLayers[l].neurons[n].activationValue += Random.Range(-1, 1);
                        geneToTransferId = -1;
                        break;
                    }
                    //Else tranfer the weight
                    else
                    {
                        print("mutated hidden weight");
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
                        print("ERROR");
                        geneToTransferId -= neuronGeneCount;
                    }
                    //If it's the last one, transfer the activation value
                    else if (geneToTransferId == neuronGeneCount - 1)
                    {
                        print("mutated out activation value");
                        networkToMutate.outputLayer.neurons[n].activationValue += Random.Range(-1, 1);
                        geneToTransferId = -1;
                        break;
                    }
                    //Else tranfer the weight
                    else
                    {
                        print("mutated out weight");
                        networkToMutate.outputLayer.neurons[n].weights[geneToTransferId] += Random.Range(-1, 1);
                        geneToTransferId = -1;
                        break;
                    }
                }
            }
        }
    }

    [ContextMenu("RunSimulation")]
    public void TrySimulation()
    {
        Simulation.Instance.RunSimulation(neuralNetworkPrefab, 0.01f);
    }

    public class NetworkIndividual
    {
        public NeuralNetwork.Network network;
        public int score;
    }
}
