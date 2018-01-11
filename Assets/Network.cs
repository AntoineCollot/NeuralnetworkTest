using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeuralNetwork
{
    public class Network : MonoBehaviour
    {
        InputLayer inputLayer;

        NeuronLayer[] hiddenLayers;

        OutputLayer outputLayer;

        // Use this for initialization
        void Start()
        {
            int connectionsCount = inputLayer.inputs.Length;

            for (int i = 0; i < hiddenLayers.Length; i++)
            {
                hiddenLayers[i].InitializeNeurons(connectionsCount);
                connectionsCount = hiddenLayers[i].neuronCount;
            }

            //Initialize output
        }

        
    }

}
