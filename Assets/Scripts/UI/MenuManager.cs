using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject[] Guns;
    [SerializeField] Canvas StartMenu;
    [SerializeField] Canvas GunSelectMenu;

    [SerializeField] Image GunImage;
    [SerializeField] TextMeshProUGUI GunName;
    [SerializeField] TextMeshProUGUI GunHP;
    [SerializeField] TextMeshProUGUI GunDamage;
    [SerializeField] TextMeshProUGUI GunAccuracy;
    [SerializeField] TextMeshProUGUI GunAttacks;


    int GunSelectIndex;

    


    public void OnStartButton()
    {
        StartMenu.enabled = false;
        GunSelectMenu.enabled = true;
        GunSelectIndex = 0;
        DisplayGun(GunSelectIndex);

        //hide title
        //Show all gun options
        
    }

    void DisplayGun(int Index)
    {
        GunAction StarterGun = Guns[Index].GetComponent<GunAction>();
        GunImage.sprite = Guns[Index].GetComponent<SpriteRenderer>().sprite;
        GunName.text = StarterGun.GunName;
        GunHP.text = "HP: " + StarterGun.GetMaxHealth();
        GunDamage.text = "DAMAGE: " + StarterGun.RegularAttackDamageLow + "-" + StarterGun.RegularAttackDamageHigh;
        GunAccuracy.text = "ATTACKS: " + StarterGun.RegularAttackName + ", " + StarterGun.SpecialAttackName;

    }

    public void OnOptionsButton()
    {
        Debug.Log("Not yet implemented");
    }

    public void OnCreditsButton()
    {
        Debug.Log("Not yet implemented");
    }

    public void OnNextGun()
    {
        if (GunSelectIndex + 1 >= Guns.Length){
            GunSelectIndex = 0;
        }
        else {
            GunSelectIndex += 1;
        }

        DisplayGun(GunSelectIndex);
    }

    public void OnPrevGun()
    {
        if (GunSelectIndex - 1 < 0){
            GunSelectIndex = Guns.Length - 1;
        }
        else {
            GunSelectIndex -= 1;
        }
        DisplayGun(GunSelectIndex);
    }

    public void OnGunSelect()
    {
        GameData.PlayerGun = Guns[GunSelectIndex];
        SceneManager.LoadScene("Battle");
    }

}
