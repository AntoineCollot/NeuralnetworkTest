using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeuralNetwork
{
    public class NetworkLoader : MonoBehaviour
    {
        [SerializeField]
        InputLayer input;

        [SerializeField]
        TextAsset inputNetwork;

        [ContextMenu("Read Network")]
        public void Read()
        {
            string[] layersData = System.Text.RegularExpressions.Regex.Split(inputNetwork.text, "#HiddenLayer|#OutputLayer");
            //string[] lines = System.Text.RegularExpressions.Regex.Split(inputNetwork.text, System.Environment.NewLine);

            List<NeuronLayer> layers = new List<NeuronLayer>();
            foreach (string layerData in layersData)
            {
                if(layerData.Length>5)
                    layers.Add(LoadLayer(layerData));
            }

            Network loadedNetwork = gameObject.AddComponent<Network>();
            loadedNetwork.hiddenLayers = new NeuronLayer[layers.Count-1];
            for (int i = 0; i < layers.Count - 1; i++)
            {
                loadedNetwork.hiddenLayers[i] = layers[i];
            }
            loadedNetwork.outputLayer = layers[layers.Count - 1];
            loadedNetwork.inputLayer = input;
        }

        NeuronLayer LoadLayer(string layerData)
        {
            string[] neuronsData = System.Text.RegularExpressions.Regex.Split(layerData, "#Neuron");

            List<Neuron> neurons = new List<Neuron>();
            foreach(string neuronData in neuronsData)
            {
                if (neuronData.Length > 5)
                    neurons.Add(LoadNeuron(neuronData));
            }

            NeuronLayer layer = new NeuronLayer();
            layer.neuronCount = neurons.Count;
            layer.neurons = neurons.ToArray();
            return layer;
        }

        Neuron LoadNeuron(string neuronData)
        {
            string[] lines = System.Text.RegularExpressions.Regex.Split(neuronData, System.Environment.NewLine);

            Neuron neuron = new Neuron();
            neuron.activationValue = float.Parse(lines[1]);
            List<float> weights = new List<float>();

            for (int i = 2; i < lines.Length; i++)
            {
                if (lines[i]!=string.Empty)
                    weights.Add(float.Parse(lines[i]));
            }

            neuron.weights = weights.ToArray();
            return neuron;
        }
    }
}
