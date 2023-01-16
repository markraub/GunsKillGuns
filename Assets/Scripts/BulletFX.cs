using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFX : MonoBehaviour
{

    [SerializeField] public Sprite BulletSprite;
    [SerializeField] public Sprite ImpactSprite;
    
    public ParticleSystem ImpactParticles;
    public SpriteRenderer BulletSR;
    public Rigidbody2D BulletRB;

    void Awake()
    {
        ImpactParticles = GetComponent<ParticleSystem>();
        BulletSR = GetComponent<SpriteRenderer>();
        BulletRB = GetComponent<Rigidbody2D>();
        BulletSR.sprite = BulletSprite;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Floor")
        {
            BulletSR.sprite = ImpactSprite;
            BulletRB.gravityScale = 2;
        }
    }





}
