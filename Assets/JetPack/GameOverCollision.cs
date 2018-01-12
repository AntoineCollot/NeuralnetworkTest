using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverCollision : MonoBehaviour {

    [SerializeField]
    float colliderRadius;

    [SerializeField]
    LayerMask collisionLayer;

    public void CustomUpdate()
    {
        if (Physics2D.OverlapCircle(transform.position, colliderRadius, collisionLayer))
        {
            Simulation.Instance.gameOver = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, colliderRadius);
    }
}
