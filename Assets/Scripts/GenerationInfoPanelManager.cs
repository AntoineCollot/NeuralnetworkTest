using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationInfoPanelManager : MonoBehaviour {

    [SerializeField]
    GenerationInfoPanel generationInfoPanelPrefab;

    List<GenerationInfoPanel> panelList = new List<GenerationInfoPanel>();

    [SerializeField]
    int maxNumberOfPanels;

    public void NewGeneration(int generation, int bestScore, int average,int mutated, int children, int population)
    {
        GenerationInfoPanel newPanel = Instantiate(generationInfoPanelPrefab, transform);
        newPanel.Init(generation,bestScore,average,mutated,children,population);
        newPanel.transform.SetAsFirstSibling();
        panelList.Add(newPanel);

        while(panelList.Count>maxNumberOfPanels)
        {
            Destroy(panelList[0].gameObject);
            panelList.RemoveAt(0);
        }
    }
}
