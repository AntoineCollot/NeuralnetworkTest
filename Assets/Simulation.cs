using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Simulation : MonoBehaviour {

    public UnityEvent onSimulationStep = new UnityEvent();

    [HideInInspector]
    public bool gameOver;

    public static Simulation Instance;

    void Awake()
    {
        Instance = this;
    }

	// Update is called once per frame
	void Update () {
        onSimulationStep.Invoke();
    }

    public void RunSimulation(NeuralNetwork.Network neuralNetwork,float deltaTime)
    {
        StartCoroutine(RunSimulationC(neuralNetwork,deltaTime));
    }

    IEnumerator RunSimulationC(NeuralNetwork.Network neuralNetwork,float deltaTime)
    {
        gameOver = false;
        while (!gameOver)
        {
            onSimulationStep.Invoke();

            if (deltaTime > 0)
                yield return new WaitForSeconds(deltaTime);
        }

        yield return null;
    }
}
