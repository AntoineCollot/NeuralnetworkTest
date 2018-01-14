using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DelayedReload : MonoBehaviour {

    [SerializeField]
    float delay;

    public void OnGameOver()
    {
        Invoke("Reload", delay);
    }

    void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
