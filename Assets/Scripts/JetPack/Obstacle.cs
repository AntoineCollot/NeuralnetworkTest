using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

    public Vector3 translation;

    public int stepLifeTime;

	public void CustomUpdate () {
        transform.Translate(translation * Time.deltaTime);
	}
}
