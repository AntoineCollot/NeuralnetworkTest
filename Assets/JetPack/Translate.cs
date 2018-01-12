using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translate : MonoBehaviour {

    [SerializeField]
    Vector3 translation;
	
	// Update is called once per frame
	void Update () {
        transform.Translate(translation * Time.deltaTime);
	}
}
