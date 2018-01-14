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

    float lastYPosition;

	// Use this for initialization
	void Awake () {
        CalculateDirections();

        //One input per raycast + the y velocity input
        inputs = new float[raycastCount + 1];
        lastYPosition = transform.position.y;
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
	
    public void CustomUpdate()
    {
        //Set the raycast inputs
        for (int i = 0; i < directions.Length; i++)
        {
            float hitDistance = Physics2D.Raycast(transform.position, directions[i], raycastMaxDistance, raycastLayer).distance;
            inputs[i] = hitDistance / raycastMaxDistance;
        }

        //Set the last input as the y velocity
        float currentYPos = transform.position.y;
        inputs[inputs.Length - 1] = currentYPos - lastYPosition;
        lastYPosition = currentYPos;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        CalculateDirections();

        foreach (Vector3 dir in directions)
                Gizmos.DrawLine(transform.position, transform.position + dir * raycastMaxDistance);
    }
}
