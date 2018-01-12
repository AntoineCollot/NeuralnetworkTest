﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyObstacleGenerator : MonoBehaviour {

    [SerializeField]
    Transform[] obstaclePrefabs;

    [SerializeField]
    float minTime;

    [SerializeField]
    float maxTime;

    [SerializeField]
    float heightVariationsRange;

    [SerializeField]
    float obstacleLifetime;

	// Use this for initialization
	void Start () {
        StartCoroutine(GenerateObstacles());
	}

    IEnumerator GenerateObstacles()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));

            Transform newObstacle = Instantiate(obstaclePrefabs[Random.Range(0,obstaclePrefabs.Length)], transform);
            Vector3 pos = Vector3.up * Random.Range(-heightVariationsRange*0.5f, heightVariationsRange*0.5f);
            newObstacle.localPosition = pos;
            Destroy(newObstacle, obstacleLifetime);
        }
    }
	
}