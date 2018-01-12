using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace NeuralNetwork
{
    public class Network : MonoBehaviour
    {
        [Header("Input Layer")]

        public InputLayer inputLayer;

        [Header("Hidden Layers")]

        public NeuronLayer[] hiddenLayers;

        [Header("Output Layer")]

        public NeuronLayer outputLayer;

        [System.Serializable]
        public class OutputEvent : UnityEvent<bool> { }
        public OutputEvent onOutput = new OutputEvent();

        // Use this for initialization
        void Start()
        {
            int connectionsCount = inputLayer.inputs.Length;

            for (int i = 0; i < hiddenLayers.Length; i++)
            {
                hiddenLayers[i].InitializeNeurons(connectionsCount);
                connectionsCount = hiddenLayers[i].neuronCount;
            }

            outputLayer.InitializeNeurons(connectionsCount);
            connectionsCount = outputLayer.neuronCount;
        }

        [ContextMenu("Compute")]
        public void Compute()
        {
            float[] inputs = inputLayer.inputs;

            for (int i = 0; i < hiddenLayers.Length; i++)
            {
                inputs = hiddenLayers[i].Compute(inputs);
            }

            if (outputLayer.Compute(inputs)[0] >= 0.5f)
                onOutput.Invoke(true);
            else
                onOutput.Invoke(false);
        }
    }

    public class InputLayer : MonoBehaviour
    {
        public float[] inputs;
    }

    [System.Serializable]
    public struct NeuronLayer
    {
        public int neuronCount;

        Neuron[] neurons;

        public float[] Compute(float[] inputs)
        {
            float[] outputs = new float[neurons.Length];

            for (int i = 0; i < neurons.Length; i++)
            {
                outputs[i] = neurons[i].Compute(inputs);
            }

            return outputs;
        }

        public void InitializeNeurons(int connectionsCount)
        {
            neurons = new Neuron[neuronCount];

            for (int i = 0; i < neurons.Length; i++)
            {
                neurons[i] = new Neuron(connectionsCount);
            }
        }
    }

    public struct Neuron
    {
        public Neuron(int inputCount)
        {
            weights = new float[inputCount];

            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = Random.Range(-1, 1);
            }
            activationValue = Random.Range(0, 1);
        }

        public float Compute(float[] inputs)
        {
            float output = 0;

            for (int i = 0; i < inputs.Length; i++)
            {
                output += inputs[i] * weights[i];
            }

            if (output >= activationValue)
                output = 1;
            else
                output = 0;

            return output;
        }

        public float[] weights;

        public int activationValue;
    }
}
