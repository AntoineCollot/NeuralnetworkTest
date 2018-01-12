using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NeuralNetwork;

public class RaycastInput : InputLayer {

    [Range(2,50)]
    [SerializeField]
    int raycastCount;

    [SerializeField]
    float raycastAngleRange;

    [SerializeField]
    float raycastMaxDistance;

    [SerializeField]
    LayerMask raycastLayer;

    Vector3[] directions;

	// Use this for initialization
	void Awake () {
        CalculateDirections();
        inputs = new float[raycastCount];
    }

    void CalculateDirections()
    {
        directions = new Vector3[raycastCount];

        Vector3 startDirection = Vector3.right;
        startDirection = Quaternion.AngleAxis(-raycastAngleRange * 0.5f, Vector3.forward) * startDirection;

        float deltaAngle = raycastAngleRange / (raycastCount-1);
        Vector3 currentDirection = startDirection;

        for (int i = 0; i < directions.Length; i++)
        {
            directions[i] = currentDirection;
            currentDirection = Quaternion.AngleAxis(deltaAngle, Vector3.forward) * currentDirection;
        }
    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < directions.Length; i++)
        {
            float hitDistance = Physics2D.Raycast(transform.position, directions[i], raycastMaxDistance, raycastLayer).distance;
            inputs[i] = hitDistance / raycastMaxDistance;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        CalculateDirections();

        foreach (Vector3 dir in directions)
                Gizmos.DrawLine(transform.position, transform.position + dir * raycastMaxDistance);
    }
}
