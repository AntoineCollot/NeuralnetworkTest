using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NeuralNetwork;

public class NetworkGeneticTrainingJetPack : MonoBehaviour {

    public int population;

    [SerializeField]
    InputLayer input;

    [SerializeField]
    JetPack output;

    [SerializeField]
    NeuralNetwork.Network neuralNetwork;

	// Use this for initialization
	void Start () {
		
	}

    [ContextMenu("Generate New Population")]
    public void GenerateNewPopulation()
    {
        for (int i = 0; i < population; i++)
        {
            NeuralNetwork.Network newNetwork = Instantiate(neuralNetwork, transform);
            newNetwork.inputLayer = input;
            newNetwork.onOutput.AddListener(output.SetFly);
        }
    }
}
