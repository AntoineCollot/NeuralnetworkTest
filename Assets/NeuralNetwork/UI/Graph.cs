using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour {

    public GraphCurve curve;

    [SerializeField]
    Transform bottomLeft;
    
    [SerializeField]
    Transform topRight;

    LineRenderer lineRenderer;

	// Use this for initialization
	void Awake () {
        lineRenderer = GetComponent<LineRenderer>();
        curve.values = new List<float>();
    }

    public void AddPoint(float value)
    {
        curve.values.Add(value);
        Draw();
    }
	
    public void Draw()
    {
        lineRenderer.positionCount = curve.values.Count;
        Vector3 position = Vector3.zero;
        for (int i = 0; i < curve.values.Count; i++)
        {
            position.x = Mathf.Lerp(bottomLeft.position.x,topRight.position.x,i/ (float)curve.values.Count);
            position.y = Mathf.Lerp(bottomLeft.position.y, topRight.position.y, curve.values[i] / curve.maxValue);
            lineRenderer.SetPosition(i, position);
        }
    }

    [System.Serializable]
    public struct GraphCurve
    {
        public float maxValue;
        public List<float> values;
    }
}
