using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAction : MonoBehaviour
{

    [SerializeField] GameObject AmmoType;


    [Header("FX")]
    [SerializeField] Animator FireAnimation;
    [SerializeField] AudioClip FireSFX;
    [SerializeField] AudioClip ReloadBulletSFX;
    [SerializeField] AudioClip FinishReloadSFX;


    [Header("Stats")]
    [SerializeField] float DamageLow;
    [SerializeField] float DamageHigh;

    [SerializeField] float Accuracy;

    [SerializeField] int MaxAmmo;

    [SerializeField] float MaxHealth;


    private float currentHealth;
    private int currentAmmo;

    AudioSource gunAudioSource;



    void Awake()
    {
        currentAmmo = MaxAmmo;
        currentHealth = MaxHealth;
        gunAudioSource = GetComponent<AudioSource>();
    }



    private IEnumerator Fire(int Rounds)
    {
        int AimDirection = 1;
        if (GetComponent<SpriteRenderer>().flipX)
        {
            AimDirection = -1;

        }
        
        if (Rounds > currentAmmo) {yield break;}
        for (int i = Rounds; i > 0; i--)
        {
            GameObject bullet = Instantiate(AmmoType, new Vector3(transform.position.x + 1f * AimDirection, transform.position.y + 0.4f, 0), transform.rotation, transform);
            bullet.tag = "Bullet";
            Rigidbody2D bulletRigidBody = bullet.GetComponent<Rigidbody2D>();

            float angle = Random.Range(-Accuracy, Accuracy);
            bulletRigidBody.velocity += new Vector2(30 * AimDirection, angle);
            currentAmmo--;
            gunAudioSource.PlayOneShot(FireSFX);
            Destroy(bullet, 3f);
            yield return new WaitForSeconds(0.5f);
            
        }
        
    }

    private IEnumerator Reload(int Rounds)
    {
        for (int i = Rounds; i > 0; i--)
        {
            if (currentAmmo == MaxAmmo){yield break;}
            currentAmmo++;
            gunAudioSource.PlayOneShot(ReloadBulletSFX);
            yield return new WaitForSeconds(0.5f);
        }
        gunAudioSource.PlayOneShot(FinishReloadSFX);
    }

    public void Attack()
    {
        StartCoroutine("Fire", 1);
    }

    public void SpecialAttack()
    {
        //needs to be reimplemented based on what type of gun I am! Since that's going to be a different prefab, and not the same prefab with different shit
        StartCoroutine("Fire", 3);
    }

    public void Defend()
    {
        StartCoroutine("Reload", 1);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            GameObject opponent = other.gameObject.transform.parent.gameObject;
            GunAction opponentGun = opponent.GetComponent<GunAction>();
            float opponentDamage = Random.Range(opponentGun.DamageLow, opponentGun.DamageHigh);
            currentHealth -= opponentDamage;

            Destroy(other.gameObject, 0.1f);

        }
    }



    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return MaxHealth;
    }

    public int GetMaxAmmo()
    {
        return MaxAmmo;
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public GameObject GetAmmoType()
    {
        return AmmoType;
    }




}
