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
}
