using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum BattleState {START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour
{

    [SerializeField] GameObject playerPrefab;
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
        GameObject playerGO = Instantiate(playerPrefab, playerSpawnPoint);
        player = playerGO.GetComponent<GunAction>();
        GameObject enemyGO = Instantiate(enemyPrefab, enemySpawnPoint);
        enemy = enemyGO.GetComponent<GunAction>();

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
