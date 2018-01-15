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

        public void InitializeConnections()
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

        //Count how many gene this network has, for mutation purpose
        public int GetGeneCount()
        {
            int geneCount = 0;

            foreach(NeuronLayer layer in hiddenLayers)
            {
                foreach(Neuron n in layer.neurons)
                {
                    geneCount += n.GetGeneCount();
                }
            }

            foreach (Neuron n in outputLayer.neurons)
            {
                geneCount += n.GetGeneCount();
            }

            return geneCount;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            //Inputs
            Gizmos.color = Color.green;
            List<Vector3> inputPositions = new List<Vector3>();
            Vector3 currentPos = transform.position;
            foreach (float i in inputLayer.inputs)
            {
                inputPositions.Add(currentPos);
                Gizmos.DrawSphere(currentPos, 0.5f);
                UnityEditor.Handles.Label(currentPos + new Vector3(-1.5f,0.25f, 0), i.ToString());
                currentPos.y -= 2;
            }

            //Neurons
            Gizmos.color = Color.blue;
            List<Vector3> previousLayerPositions = inputPositions;
            currentPos = transform.position;

            foreach (NeuronLayer l in hiddenLayers)
            {
                currentPos.x += 7;
                List<Vector3> neuronPositions = new List<Vector3>();

                //Draw neuron
                for (int i = 0; i < l.neuronCount; i++)
                {
                    Gizmos.DrawSphere(currentPos, 0.5f);
                    neuronPositions.Add(currentPos);

                    //Draw lines
                    for (int j = 0; j < previousLayerPositions.Count; j++)
                    {
                        Gizmos.DrawLine(currentPos, previousLayerPositions[j]);
                        if (l.neurons != null)
                        {
                            UnityEditor.Handles.Label(Vector3.Lerp(previousLayerPositions[j], currentPos, Mathf.Clamp01((l.neurons[i].weights[j] * 0.8f + 1) * 0.5f)), l.neurons[i].weights[j].ToString());
                        }
                    }
                    currentPos.y -= 3;
                }

                previousLayerPositions = neuronPositions;
            }

            //Output
            Gizmos.color = Color.red;
            currentPos.y = transform.position.y;
            currentPos.x += 7;

            Gizmos.DrawSphere(currentPos, 0.5f);

            //Draw lines
            for (int j = 0; j < previousLayerPositions.Count; j++)
            {
                Gizmos.DrawLine(currentPos, previousLayerPositions[j]);
                if (outputLayer.neurons != null)
                {
                    UnityEditor.Handles.Label(Vector3.Lerp(previousLayerPositions[j], currentPos, Mathf.Clamp01((outputLayer.neurons[0].weights[j] * 0.8f + 1) * 0.5f)), outputLayer.neurons[0].weights[j].ToString());
                }
            }
        }
#endif
    }

    public class InputLayer : MonoBehaviour
    {
        public float[] inputs;
    }

    [System.Serializable]
    public struct NeuronLayer
    {
        public int neuronCount;

        [HideInInspector]
        public Neuron[] neurons;

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
                weights[i] = Random.Range(-1.0f, 1.0f);
            }
            activationValue = Random.Range(0.0f, 1.0f);
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

        public int GetGeneCount()
        {
            //Weigths + activationValue
            return weights.Length + 1;
        }

        public float[] weights;

        public float activationValue;
    }
}
