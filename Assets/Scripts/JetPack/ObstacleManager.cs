using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour {

    List<Transform> availableObstacles;

    Queue<Transform> inUseObstacles = new Queue<Transform>();

    [SerializeField]
    int spawnStepInterval;
    int nextSpawnTimer;

    [SerializeField]
    float heightVariationsRange;

    [SerializeField]
    float visibleZoneWidth;

    [SerializeField]
    float obstacleSpeed;

    [SerializeField]
    int maxObstaclesInUse;

	// Use this for initialization
	void Awake () {
        availableObstacles = new List<Transform>();

        foreach(Transform child in transform)
        {
            availableObstacles.Add(child);
        }

    }

    public void CustomUpdate()
    {
        foreach(Transform o in inUseObstacles)
        {
            o.Translate(-obstacleSpeed * Vector3.right, Space.World);
        }

        if (ShouldGenerateObstacle())
            GenerateObstacle();
    }

    bool ShouldGenerateObstacle()
    {
        nextSpawnTimer--;

        if(nextSpawnTimer<=0)
        {
            nextSpawnTimer = spawnStepInterval;
            return true;
        }

        return false;
    }

    void GenerateObstacle()
    {
        int obstacleToSpawnId = Random.Range(0, availableObstacles.Count);
        Transform obstacleToSpawn = availableObstacles[obstacleToSpawnId];
        availableObstacles.RemoveAt(obstacleToSpawnId);

        obstacleToSpawn.position = new Vector3(visibleZoneWidth * 0.5f,Random.Range(-heightVariationsRange * 0.5f, heightVariationsRange * 0.5f), 0);
        obstacleToSpawn.gameObject.SetActive(true);

        //Add the obstacle to the queue of obstacle in use
        inUseObstacles.Enqueue(obstacleToSpawn);

        //If we reached the max number of obstacle, remove the oldest one from the queue
        if(inUseObstacles.Count> maxObstaclesInUse)
        {
            Transform obstacleTodesactivate = inUseObstacles.Dequeue();
            obstacleTodesactivate.gameObject.SetActive(false);
            availableObstacles.Add(obstacleTodesactivate);
        }

    }
	
    public void Clear()
    {
        while(inUseObstacles.Count>0)
        {
            Transform o = inUseObstacles.Dequeue();
            o.gameObject.SetActive(false);
            availableObstacles.Add(o);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, new Vector3(visibleZoneWidth, heightVariationsRange, 1));
    }
}
