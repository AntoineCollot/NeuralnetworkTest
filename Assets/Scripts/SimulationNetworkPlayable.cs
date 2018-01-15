using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationNetworkPlayable : Simulation {

    [SerializeField]
    NeuralNetwork.Network network;

    // Update is called once per frame
    protected void Update()
    {
        if (network == null)
        {
            network = GetComponentInChildren<NeuralNetwork.Network>();
            return;
        }

        network.Compute();
        onSimulationStep.Invoke();
        if (!gameOver)
            step++;
    }
}
