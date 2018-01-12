using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetPack : MonoBehaviour {

    [SerializeField]
    Vector2 flyForce;

    [SerializeField]
    Vector2 gravityForce;

    Vector2 momemtum;

    bool fly;

    public void CustomUpdate()
    {
        if (Input.GetButtonDown("Fly") || fly)
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
