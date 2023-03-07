using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAction : MonoBehaviour
{


    public class Attack 
    {
        public string Name;
        public GameObject AmmoType;
        public int Cost;
        public Vector2 Damage;
        public float Accuracy;
        public float Rate;
        public float Speed;

    }


    [Header("Name")]
    [SerializeField] public string GunName;
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
    [SerializeField] string RegularAttackName;
    [SerializeField] GameObject RegularAmmoType;
    [SerializeField] int RegularAttackCost;
    [SerializeField] Vector2 RegularAttackDamage;
    [SerializeField] float RegularAttackAccuracy;
    [SerializeField] float RegularAttackRate;
    [SerializeField] float RegularAttackBulletSpeed;

    [Header("Special Attack")]
    [SerializeField] string SpecialAttackName;
    [SerializeField] GameObject SpecialAmmoType;
    [SerializeField] int SpecialAttackCost;
    [SerializeField] Vector2 SpecialAttackDamage;
    [SerializeField] float SpecialAttackAccuracy;
    [SerializeField] float SpecialAttackRate;
    [SerializeField] float SpecialAttackBulletSpeed;

    [Header("Defend")]
    [SerializeField] int ReloadCount;
    private float currentHealth;
    private int currentAmmo;

    private Rigidbody2D pistolRigidBody;


    AudioSource gunAudioSource;

    Attack regularAttack;
    Attack specialAttack;

    Transform BulletSpawnPoint;

    void Awake()
    {

        BulletSpawnPoint = transform.GetChild(1);
        currentAmmo = MaxAmmo;
        currentHealth = MaxHealth;
        gunAudioSource = GetComponent<AudioSource>();
        pistolRigidBody = GetComponent<Rigidbody2D>();
        FloatDistance += Random.Range(-0.3f, 0.3f);
        Speed += Random.Range(-0.3f, 0.3f);

        regularAttack = new Attack();
        regularAttack.Name = RegularAttackName;
        regularAttack.AmmoType = RegularAmmoType;
        regularAttack.Accuracy = RegularAttackAccuracy;
        regularAttack.Cost = RegularAttackCost;
        regularAttack.Damage = RegularAttackDamage;
        regularAttack.Speed = RegularAttackBulletSpeed;
        regularAttack.Rate = RegularAttackRate;

        specialAttack = new Attack();
        specialAttack.Name = SpecialAttackName;
        specialAttack.AmmoType = SpecialAmmoType;
        specialAttack.Accuracy = SpecialAttackAccuracy;
        specialAttack.Cost = SpecialAttackCost;
        specialAttack.Damage = SpecialAttackDamage;
        specialAttack.Speed = SpecialAttackBulletSpeed;
        specialAttack.Rate = SpecialAttackRate;
    
        
    }

    void Update()
    {
        float floatpos = Mathf.PingPong(Time.time * Speed, FloatDistance) - 1;
        transform.position = new Vector2(transform.position.x, floatpos);
    }

    private IEnumerator Fire(int Rounds, float Accuracy, GameObject AmmoType, float BulletSpeed, float AttackSpeed, bool Special)
    {
        int AimDirection = (int)transform.localScale.x;
        
        if (Rounds > currentAmmo) {yield break;}
        for (int i = Rounds; i > 0; i--)
        {
            GameObject bullet = Instantiate(AmmoType, BulletSpawnPoint.position, transform.rotation, transform);
            bullet.GetComponent<SpriteRenderer>().flipX = AimDirection < 0;
            
            if (Special)
            {
                bullet.tag = "Special";

            }
            else {bullet.tag = "Bullet"; }
            Rigidbody2D bulletRigidBody = bullet.GetComponent<Rigidbody2D>();

            float angle = Random.Range(-Accuracy, Accuracy);
            bulletRigidBody.velocity += new Vector2(BulletSpeed * AimDirection, angle);


            currentAmmo--;
            gunAudioSource.PlayOneShot(FireSFX);
            yield return new WaitForSeconds(AttackSpeed);
            
        }
        
    }

    private IEnumerator Reload(int Rounds)
    {
        for (int i = Rounds; i > 0; i--)
        {
            if (currentAmmo == MaxAmmo){yield break;}
            currentAmmo++;
            gunAudioSource.PlayOneShot(ReloadBulletSFX);
            yield return new WaitForSeconds(0.05f);
        }
        gunAudioSource.PlayOneShot(FinishReloadSFX);
    }

    public void RegularAttack()
    {
        StartCoroutine(Fire(RegularAttackCost, RegularAttackAccuracy, RegularAmmoType, RegularAttackBulletSpeed, RegularAttackRate, false));
    }

    public void SpecialAttack()
    {
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
            Debug.Log("HIT");
            GameObject opponent = other.gameObject.transform.parent.gameObject;
            GunAction opponentGun = opponent.GetComponent<GunAction>();
            float opponentDamage = Random.Range(opponentGun.regularAttack.Damage.x, opponentGun.regularAttack.Damage.y);
            currentHealth -= opponentDamage;

            BulletFX bullet = other.gameObject.GetComponent<BulletFX>();
            bullet.ImpactParticles.Play();
            bullet.BulletSR.sprite = bullet.ImpactSprite;
            bullet.BulletRB.gravityScale = 2;
            

            //Destroy(other.gameObject, 0.5f);
        }
        else if (other.gameObject.tag == "Special")
        {
            GameObject opponent = other.gameObject.transform.parent.gameObject;
            GunAction opponentGun = opponent.GetComponent<GunAction>();
            float opponentDamage = Random.Range(opponentGun.specialAttack.Damage.x, opponentGun.specialAttack.Damage.y);
            currentHealth -= opponentDamage;

            BulletFX bullet = other.gameObject.GetComponent<BulletFX>();
            bullet.ImpactParticles.Play();
            bullet.BulletSR.sprite = bullet.ImpactSprite;
            bullet.BulletRB.gravityScale = 2;


            //Destroy(other.gameObject, 0.5f);
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

    public Attack GetAttack(bool Special = false)
    {
        if (Special)
        {
            specialAttack = new Attack();
            specialAttack.Name = SpecialAttackName;
            specialAttack.AmmoType = SpecialAmmoType;
            specialAttack.Accuracy = SpecialAttackAccuracy;
            specialAttack.Cost = SpecialAttackCost;
            specialAttack.Damage = SpecialAttackDamage;
            specialAttack.Speed = SpecialAttackBulletSpeed;
            specialAttack.Rate = SpecialAttackRate;
            return specialAttack;
        }
        else
        {

            regularAttack = new Attack();
            regularAttack.Name = RegularAttackName;
            regularAttack.AmmoType = RegularAmmoType;
            regularAttack.Accuracy = RegularAttackAccuracy;
            regularAttack.Cost = RegularAttackCost;
            regularAttack.Damage = RegularAttackDamage;
            regularAttack.Speed = RegularAttackBulletSpeed;
            regularAttack.Rate = RegularAttackRate;
            return regularAttack;
        }

    }
}
