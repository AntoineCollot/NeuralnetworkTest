
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeuralNetwork
{
    public class NeuronLayer : MonoBehaviour
    {
        public int neuronCount = 0;

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
