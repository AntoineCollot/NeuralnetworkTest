using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

namespace NeuralNetwork
{
    public class NetworkExporter : MonoBehaviour
    {
        [SerializeField]
        Network neuralNetwork;

        [ContextMenu("TextExport")]
        public void TestExport()
        {
            Export(neuralNetwork,"TestNetwork");
        }

        public void Export(Network neuralNetwork,string fileName)
        {
            List<string> lines = new List<string>();

            foreach(NeuronLayer hiddenLayer in neuralNetwork.hiddenLayers)
            {
                //Note that we add a new hidden layer data
                lines.Add("#HiddenLayer");

                foreach(Neuron n in hiddenLayer.neurons)
                {
                    //Note that we add a neuron
                    lines.Add("#Neuron");

                    //Note the activation value
                    lines.Add(n.activationValue.ToString());

                    //Note all weights
                    foreach (float w in n.weights)
                        lines.Add(w.ToString());
                }
            }

            //Note that we add the output Layer
            lines.Add("#OutputLayer");

            foreach (Neuron n in neuralNetwork.outputLayer.neurons)
            {
                //Note that we add a neuron
                lines.Add("#Neuron");

                //Note the activation value
                lines.Add(n.activationValue.ToString());

                //Note all weights
                foreach (float w in n.weights)
                    lines.Add(w.ToString());
            }

            StringBuilder str = new StringBuilder();
            foreach (string line in lines)
                str.AppendLine(line);

            File.WriteAllText(Application.streamingAssetsPath+@"\NeuralNetworks\"+fileName+".txt", str.ToString());
        }
    }
}
