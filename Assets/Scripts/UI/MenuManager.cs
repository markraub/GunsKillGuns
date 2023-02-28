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
        GameData.EnemyLevel = 0;
    }

    void DisplayGun(int Index)
    {
        GunAction Gun = Guns[Index].GetComponent<GunAction>();
        GunImage.sprite = Guns[Index].GetComponent<SpriteRenderer>().sprite;
        GunName.text = Gun.GunName;
        GunHP.text = "HP: " + Gun.GetMaxHealth();
        GunDamage.text = "DAMAGE: " + Gun.GetAttack().Damage.x + "-" + Gun.GetAttack().Damage.y;
        GunAccuracy.text = "ACCURACY: " + Gun.GetAttack().Accuracy.ToString();
        GunAttacks.text = "ATTACKS: " + Gun.GetAttack().Name + ", " + Gun.GetAttack(true).Name;

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
