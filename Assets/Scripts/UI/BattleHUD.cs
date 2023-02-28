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
    [SerializeField] TextMeshProUGUI bulletText;
    [SerializeField] public Button AttackButton;
    [SerializeField] public Button SpecialAttackButton;
    [SerializeField] public Button DefendButton;
    public void SetHUD(GunAction gun)
    {
        nameText.text = gun.gameObject.name;
        int currentHP = (int)gun.GetCurrentHealth();
        currentHPText.text = currentHP.ToString();
        maxHPText.text = gun.GetMaxHealth().ToString();
        hpSlider.maxValue = gun.GetMaxHealth();
        hpSlider.value = gun.GetCurrentHealth();
        bulletText.text = "AMMO: " + gun.GetCurrentAmmo() + "/" + gun.GetMaxAmmo();

    }

    public void SetPlayerButtons(GunAction gun)
    {
        AttackButton.GetComponentInChildren<TextMeshProUGUI>().text = gun.GetAttack().Name;
        SpecialAttackButton.GetComponentInChildren<TextMeshProUGUI>().text = gun.GetAttack(true).Name;
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



}
