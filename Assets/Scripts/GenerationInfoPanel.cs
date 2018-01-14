using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerationInfoPanel : MonoBehaviour {

    [SerializeField]
    Text generationText;

    [SerializeField]
    Text bestText;

    [SerializeField]
    Text averageText;

    [SerializeField]
    Text mutatedText;

    [SerializeField]
    Text childrenText;

    [SerializeField]
    Text populationText;

    public void Init(int generation, int bestScore,int average,int mutated, int children, int population)
    {
        generationText.text = "Generation " + generation.ToString();
        bestText.text = "Best " + bestScore.ToString();
        averageText.text = "Average " + average.ToString();
        mutatedText.text = "Mutated " + mutated.ToString();
        childrenText.text = "Children " + children.ToString();
        populationText.text = "Pop " + population.ToString();
    }
}
