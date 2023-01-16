using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Slider hpSlider;
    [SerializeField] TextMeshProUGUI currentHPText;
    [SerializeField] TextMeshProUGUI maxHPText;

    [SerializeField] public Button AttackButton;
    [SerializeField] public Button SpecialAttackButton;
    [SerializeField] public Button DefendButton;


    GameObject[] BulletImages = new GameObject[0];

  


    public void SetHUD(GunAction gun)
    {
        nameText.text = gun.gameObject.name;
        int currentHP = (int)gun.GetCurrentHealth();
        currentHPText.text = currentHP.ToString();
        maxHPText.text = gun.GetMaxHealth().ToString();
        hpSlider.maxValue = gun.GetMaxHealth();
        hpSlider.value = gun.GetCurrentHealth();

        DrawBullets(gun);
    }

    public void SetPlayerButtons(GunAction gun)
    {
        AttackButton.GetComponentInChildren<TextMeshProUGUI>().text = gun.RegularAttackName;
        SpecialAttackButton.GetComponentInChildren<TextMeshProUGUI>().text = gun.SpecialAttackName;
    }

    public void SetHP(float hp)
    {
        hpSlider.value = hp;
    }

    public void DisablePlayerUI()
    {
        AttackButton.enabled = false;
        SpecialAttackButton.enabled = false;
        DefendButton.enabled = false;

        AttackButton.gameObject.SetActive(false);
        SpecialAttackButton.gameObject.SetActive(false);
        DefendButton.gameObject.SetActive(false);
    }

    public void DrawBullets(GunAction gun)
    {

        Sprite BulletSprite;
        BulletSprite = gun.GetAmmoSprite();
        if (BulletImages.Length > 0)
        {
            foreach (GameObject bullet in BulletImages)
            {
                Destroy(bullet);
            }
        }

        BulletImages = new GameObject[gun.GetCurrentAmmo()];

        for (int i = 0; i < gun.GetCurrentAmmo(); i++)
        {
            BulletImages[i] = new GameObject("Bullet Img " + i);
            BulletImages[i].transform.parent = transform;
            Image newBulletImage = BulletImages[i].AddComponent<Image>();
            newBulletImage.sprite = BulletSprite;
            RectTransform bulletPos = BulletImages[i].GetComponent<RectTransform>();
            bulletPos.position = new Vector3(
                hpSlider.transform.position.x - (hpSlider.GetComponent<RectTransform>().rect.width / 2.2f) + BulletSprite.texture.width / 2f * i, 
                hpSlider.transform.position.y - 25, 
                0
                );

        } 
    }

}
