using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

namespace NeuralNetwork
{
    public class NetworkExporter
    {
        public static void Export(Network neuralNetwork,string fileName)
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

        public static void ExportGeneration(Network[] neuralNetworks, int generation)
        {
            // Specify the directory you want to manipulate.
            string directoryPath = Application.streamingAssetsPath + @"\NeuralNetworks\Generation_" + generation.ToString();

            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(directoryPath))
                {
                    Debug.Log("That directory exists already (" + directoryPath + ")");
                    return;
                }

                // Try to create the directory.
                Directory.CreateDirectory(directoryPath);
            }
            catch (Exception e)
            {
                Debug.Log("The export generation process failed: "+ e.ToString());
            }

            for (int i = 0; i < neuralNetworks.Length; i++)
            {
                Export(neuralNetworks[i], "Generation_" + generation.ToString() + @"\Network_" + i.ToString("00000"));
            }

            Debug.Log("Exported successfully at : "+directoryPath);
        }
    }
}
