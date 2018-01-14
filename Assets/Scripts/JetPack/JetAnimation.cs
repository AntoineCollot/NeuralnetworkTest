using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetAnimation : MonoBehaviour {

    [SerializeField]
    Sprite IdleSprite;

    [SerializeField]
    Sprite fallingSprite;

    [SerializeField]
    ParticleSystem fire;

    [Header("Death")]

    [SerializeField]
    ParticleSystem deathEffect;

    new SpriteRenderer renderer;
    JetPack jetPack;

    // Use this for initialization
    void Start () {
        renderer = GetComponent<SpriteRenderer>();
        jetPack = GetComponent<JetPack>();
    }
	
    public void CustomUpdate()
    {
        if((Input.GetButton("Fly") || jetPack.fly) && !Simulation.Instance.gameOver )
        {
            renderer.sprite = IdleSprite;
            fire.Play();
        }
        else
        {
            renderer.sprite = fallingSprite;
            fire.Stop();
        }

    }

    public void OnDeathEffect()
    {
        deathEffect.Play();
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
