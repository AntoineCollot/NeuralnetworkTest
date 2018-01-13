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

    new SpriteRenderer renderer;
    JetPack jetPack;

    // Use this for initialization
    void Start () {
        renderer = GetComponent<SpriteRenderer>();
        jetPack = GetComponent<JetPack>();

    }
	
    public void CustomUpdate()
    {
        if(Input.GetButton("Fly") || jetPack.fly)
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
}
