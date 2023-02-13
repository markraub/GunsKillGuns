using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public enum BattleState {START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour
{

    [SerializeField] GameObject enemyPrefab;
    [SerializeField] BattleState state;

    [SerializeField] Transform playerSpawnPoint;
    [SerializeField] Transform enemySpawnPoint;

    [SerializeField] BattleHUD playerHUD;
    [SerializeField] BattleHUD enemyHUD;

    GunAction player;
    GunAction enemy;


    void Start()
    {
        state = BattleState.START;
        SetupBattle();
    }

    void SetupBattle()
    {
        GameObject playerGO = Instantiate(GameData.PlayerGun, playerSpawnPoint);
        player = playerGO.GetComponent<GunAction>();
        playerGO.name = player.GunName;
        GameObject enemyGO = Instantiate(enemyPrefab, enemySpawnPoint);
        enemyGO.transform.localScale = new Vector3(-1f, 1f, 1f);
        enemy = enemyGO.GetComponent<GunAction>();
        enemyGO.name = "Evil " + enemy.GunName;

        playerHUD.SetHUD(player);
        playerHUD.SetPlayerButtons(player);

        enemyHUD.SetHUD(enemy);

        playerHUD.DisablePlayerUI();


        if (Random.Range(0, 100) > 50)
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine("EnemyTurn");
        }

    }

    void PlayerTurn()
    {
        if (player.GetCurrentHealth() <= 0)
        {
            SceneManager.LoadScene("GAME OVER");
        } 
        if (player.GetCurrentAmmo() >= 1)
        {
            playerHUD.AttackButton.enabled = true;
            playerHUD.AttackButton.gameObject.SetActive(true);

        }

        if (player.GetCurrentAmmo() >= 3)
        {
            playerHUD.SpecialAttackButton.enabled = true;
            playerHUD.SpecialAttackButton.gameObject.SetActive(true);
        }

        playerHUD.DefendButton.enabled = true;
        playerHUD.DefendButton.gameObject.SetActive(true);


    }

    public void OnAttackButton()
    {
        Debug.Log("Attack!!");
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        //make this a thing? not like a thing, like a specific function for attack and whatever. Should be in a pistol class? 
        player.Attack();
        EndPlayerTurn();

    }

    public void OnSpecialAttackButton()
    {
        Debug.Log("Special Attack!");
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        player.SpecialAttack();
        EndPlayerTurn();
        


    }

    public void OnDefendButton()
    {
        Debug.Log("Reloading!");
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        player.Defend();
        EndPlayerTurn();

    }

    public void EndPlayerTurn()
    {
        state = BattleState.ENEMYTURN;
        playerHUD.DisablePlayerUI();
        StartCoroutine("EnemyTurn");
    }

    IEnumerator EnemyTurn()
    {

        if (enemy.GetCurrentHealth() <= 0)
        {
            SceneManager.LoadScene("GAME OVER");
        }
        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        if (enemy.GetCurrentAmmo() > 4)
        {
            enemy.SpecialAttack();
        }
        else if (enemy.GetCurrentAmmo() <= 4 && enemy.GetCurrentAmmo() >= 1)
        {
            enemy.Attack();
        }
        else
        {
            enemy.Defend();
        }
        state = BattleState.PLAYERTURN;
        PlayerTurn();

    }



    void Update()
    {

        playerHUD.SetHUD(player);
        enemyHUD.SetHUD(enemy);



    }


}
