using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFX : MonoBehaviour
{

    [Header("Sprites")]
    [SerializeField] public Sprite BulletSprite;
    [SerializeField] public Sprite ImpactSprite;

    [Header("SFX")]
    [SerializeField] AudioClip FloorSFX;
    [SerializeField] AudioClip ImpactSFX;
    
    public ParticleSystem ImpactParticles;
    public SpriteRenderer BulletSR;
    public Rigidbody2D BulletRB;

    public PhysicsMaterial2D BulletPM;

    AudioSource BulletAudioSource;

    void Awake()
    {
        ImpactParticles = GetComponent<ParticleSystem>();
        BulletSR = GetComponent<SpriteRenderer>();
        BulletRB = GetComponent<Rigidbody2D>();
        BulletSR.sprite = BulletSprite;

        BulletPM = BulletRB.sharedMaterial;

        BulletAudioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Floor")
        {
            BulletAudioSource.PlayOneShot(FloorSFX);
            BulletSR.sprite = ImpactSprite;
            BulletRB.gravityScale = 2;
            BulletPM.bounciness = 0;
            BulletPM.friction = 1;
            Destroy(gameObject, 1);
            
        }
        else if (other.gameObject.tag == "Player")
        {
            BulletAudioSource.PlayOneShot(ImpactSFX);
        }
    }





}
