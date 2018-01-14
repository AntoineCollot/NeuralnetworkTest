using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetPack : MonoBehaviour {

    [SerializeField]
    Vector2 flyForce;

    [SerializeField]
    Vector2 gravityForce;

    Vector2 momemtum;

    [HideInInspector]
    public bool fly;

    public void CustomUpdate()
    {
        if (Simulation.Instance.step < 50)
            return;

        if (Input.GetButton("Fly") || fly)
        {
            momemtum += flyForce;
        }

        momemtum += gravityForce;

        transform.Translate(momemtum, Space.World);
    }

    public void SetFly(bool value)
    {
        fly = value;
    }

    public void Reset()
    {
        transform.localPosition = Vector3.zero;
        momemtum = Vector2.zero;
        fly = false;
    }
}
