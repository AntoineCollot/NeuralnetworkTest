using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationPlayable : Simulation
{

    // Update is called once per frame
    protected void Update()
    {
        onSimulationStep.Invoke();
        if(!gameOver)
            step++;
    }
}
