using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Simulation : MonoBehaviour {

    public UnityEvent onSimulationStep = new UnityEvent();

    public UnityEvent onGameOver = new UnityEvent();

    [HideInInspector]
    public bool gameOver;

    [HideInInspector]
    public int step;

    public static Simulation Instance;

    protected void Awake()
    {
        Instance = this;
        step = 0;
    }

    public IEnumerator RunSimulation(NeuralNetwork.Network neuralNetwork,float deltaTime)
    {
        step = 0;
        gameOver = false;
        while (!gameOver)
        {
            neuralNetwork.Compute();

            onSimulationStep.Invoke();
            step++;

            if (deltaTime > 0)
                yield return new WaitForSeconds(deltaTime);
        }

        yield return null;
    }

    public void GameOver()
    {
        //First time, invoke the event
        if (!gameOver)
            onGameOver.Invoke();

        gameOver = true;
    }

    protected void OnGUI()
    {
        GUI.Label(new Rect(Screen.width - 100, 10, 100, 100), "Score : "+step.ToString());
    }
}
