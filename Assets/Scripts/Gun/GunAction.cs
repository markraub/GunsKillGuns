using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunAction : MonoBehaviour
{




    [Header("FX")]
    [SerializeField] Animator FireAnimation;
    [SerializeField] AudioClip FireSFX;
    [SerializeField] AudioClip ReloadBulletSFX;
    [SerializeField] AudioClip FinishReloadSFX;

    [Header("Physics")]
    [SerializeField] float FloatDistance;
    [SerializeField] float Speed;

    [Header("Stats")]
    [SerializeField] int MaxAmmo;
    [SerializeField] float MaxHealth;

    [Header("Regular Attack")]
    [SerializeField] public string RegularAttackName;
    [SerializeField] GameObject RegularAmmoType;
    [SerializeField] int RegularAttackCost;
    [SerializeField] float RegularAttackDamageHigh;
    [SerializeField] float RegularAttackDamageLow;
    [SerializeField] float RegularAttackAccuracy;
    [SerializeField] float RegularAttackRate;
    [SerializeField] float RegularAttackBulletSpeed;

    [Header("Special Attack")]
    [SerializeField] public string SpecialAttackName;
    [SerializeField] GameObject SpecialAmmoType;
    [SerializeField] int SpecialAttackCost;
    [SerializeField] float SpecialAttackDamageHigh;
    [SerializeField] float SpecialAttackDamageLow;
    [SerializeField] float SpecialAttackAccuracy;
    [SerializeField] float SpecialAttackRate;
    [SerializeField] float SpecialAttackBulletSpeed;

    [Header("Defend")]
    [SerializeField] int ReloadCount;



    private float currentHealth;
    private int currentAmmo;

    private Rigidbody2D pistolRigidBody;

    public bool Attacking = false;

    AudioSource gunAudioSource;



    void Awake()
    {
        currentAmmo = MaxAmmo;
        currentHealth = MaxHealth;
        gunAudioSource = GetComponent<AudioSource>();
        pistolRigidBody = GetComponent<Rigidbody2D>();
        FloatDistance += Random.Range(-0.3f, 0.3f);
        Speed += Random.Range(-0.3f, 0.3f);
    }

    void Update()
    {
        float floatpos = Mathf.PingPong(Time.time * Speed, FloatDistance) - 1;
        transform.position = new Vector2(transform.position.x, floatpos);
    }



    private IEnumerator Fire(int Rounds, float Accuracy, GameObject AmmoType, float BulletSpeed, float AttackSpeed, bool Special)
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
            if (Special)
            {
                bullet.tag = "Special";

            }
            else {bullet.tag = "Bullet"; }
            Rigidbody2D bulletRigidBody = bullet.GetComponent<Rigidbody2D>();

            float angle = Random.Range(-Accuracy, Accuracy);
            bulletRigidBody.velocity += new Vector2(BulletSpeed * AimDirection, angle);
            if (bulletRigidBody.velocity.x < 0)
            {
                bullet.GetComponent<SpriteRenderer>().flipX = true;
            }

            currentAmmo--;
            gunAudioSource.PlayOneShot(FireSFX);
            Destroy(bullet, 3f);
            yield return new WaitForSeconds(AttackSpeed);
            
        }
        Attacking = false;
        
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
        Attacking = true;
        StartCoroutine(Fire(RegularAttackCost, RegularAttackAccuracy, RegularAmmoType, RegularAttackBulletSpeed, RegularAttackRate, false));
    }

    public void SpecialAttack()
    {
        Attacking = true;
        StartCoroutine(Fire(SpecialAttackCost, SpecialAttackAccuracy, SpecialAmmoType, SpecialAttackBulletSpeed, SpecialAttackRate, true));
    }

    public void Defend()
    {
        StartCoroutine("Reload", ReloadCount);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            GameObject opponent = other.gameObject.transform.parent.gameObject;
            GunAction opponentGun = opponent.GetComponent<GunAction>();
            float opponentDamage = Random.Range(opponentGun.RegularAttackDamageLow, opponentGun.RegularAttackDamageHigh);
            currentHealth -= opponentDamage;
            Destroy(other.gameObject, 0.1f);
        }
        else if (other.gameObject.tag == "Special")
        {
            GameObject opponent = other.gameObject.transform.parent.gameObject;
            GunAction opponentGun = opponent.GetComponent<GunAction>();
            float opponentDamage = Random.Range(opponentGun.SpecialAttackDamageLow, opponentGun.SpecialAttackDamageHigh);
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

    public Sprite GetAmmoSprite()
    {
        return RegularAmmoType.GetComponent<SpriteRenderer>().sprite;
    }




}
