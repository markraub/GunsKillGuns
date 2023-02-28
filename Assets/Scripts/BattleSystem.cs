using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public enum BattleState {START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleSystem : MonoBehaviour
{

    [SerializeField] BattleState state;

    [SerializeField] Transform playerSpawnPoint;
    [SerializeField] Transform enemySpawnPoint;

    [SerializeField] BattleHUD playerHUD;
    [SerializeField] BattleHUD enemyHUD;
    [SerializeField] Slider TimerSlider;
    [SerializeField] float TurnTime;
    [SerializeField] TextMeshProUGUI EnemyTurnTextObject;
    [SerializeField] TextMeshProUGUI PlayerTurnTextObject;

    [Header("Enemy Settings")]
    [SerializeField] GameObject[] EnemyOrder;
    float currentTurnTime;
    GunAction player;
    GunAction enemy;
    private bool takenTurn = false;


    void Start()
    {
        state = BattleState.START;
        currentTurnTime = TurnTime;
        SetupBattle();
    }

    void SetupBattle()
    {
        if (GameData.PlayerGun == null)
        {
            GameData.PlayerGun = EnemyOrder[GameData.EnemyLevel];
        }
        GameObject playerGO = Instantiate(GameData.PlayerGun, playerSpawnPoint);
        player = playerGO.GetComponent<GunAction>();
        playerGO.name = player.GunName;
        GameObject enemyGO = Instantiate(EnemyOrder[GameData.EnemyLevel], enemySpawnPoint);
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
            EnemyTurn();
        }

    }

    void PlayerTurn()
    {
        

        if (player.GetCurrentAmmo() >= player.GetAttack().Cost)
        {
            playerHUD.AttackButton.enabled = true;
            playerHUD.AttackButton.gameObject.SetActive(true);

        }

        if (player.GetCurrentAmmo() >= player.GetAttack(true).Cost)
        {
            playerHUD.SpecialAttackButton.enabled = true;
            playerHUD.SpecialAttackButton.gameObject.SetActive(true);
        }

        playerHUD.DefendButton.enabled = true;
        playerHUD.DefendButton.gameObject.SetActive(true);


    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        player.RegularAttack();
        EndPlayerTurn();

    }

    public void OnSpecialAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        player.SpecialAttack();
        EndPlayerTurn();
        
    }

    public void OnDefendButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        player.Defend();
        EndPlayerTurn();

    }

    public void EndPlayerTurn()
    {
        currentTurnTime = TurnTime;
        state = BattleState.ENEMYTURN;
        takenTurn = false;
        playerHUD.DisablePlayerUI();
        //EnemyTurn();
    }

    void EnemyTurn()
    {
        
        if (enemy.GetCurrentAmmo() > enemy.GetAttack(true).Cost)
        {
            enemy.SpecialAttack();
            takenTurn = true;


        }
        else if (enemy.GetCurrentAmmo() <= enemy.GetAttack(true).Cost && enemy.GetCurrentAmmo() >= 1)
        {
            enemy.RegularAttack();
            takenTurn = true;

        }
        else
        {
            enemy.Defend();
            takenTurn = true;
        }
       
    }

    void EndEnemyTurn()
    {
        state = BattleState.PLAYERTURN;
        currentTurnTime = TurnTime;
        StartCoroutine(TurnTitleAnimator(PlayerTurnTextObject, 0.5f));
        PlayerTurn();
    }

    IEnumerator TurnTitleAnimator(TextMeshProUGUI Title, float speed)
    {
        float xpos = -600;
        RectTransform title_pos = Title.GetComponent<RectTransform>();
        Title.enabled = true;
        title_pos.position = new Vector3(xpos, 0, 0);


        while (xpos <= 0)
        {
            title_pos.position = new Vector3(xpos, 0, 0);
            xpos += 1 * Time.deltaTime;
            yield return new WaitForSeconds(speed/10);
        }
        yield return new WaitForSeconds(1);
        Title.enabled = false;
    }


    void Update()
    {
        if (state != BattleState.START || state != BattleState.WON || state != BattleState.LOST)
        {
            if (state == BattleState.PLAYERTURN)
            {
                if (player.GetCurrentHealth() <= 0)
                {
                    state = BattleState.LOST;
                    SceneManager.LoadScene("GAME OVER");
                }
                else if (enemy.GetCurrentHealth() <= 0)
                {
                    state = BattleState.WON;
                    SceneManager.LoadScene("WIN");
                }

                currentTurnTime -= Time.deltaTime;
                TimerSlider.value = currentTurnTime / TurnTime;

                if (currentTurnTime <= 0)
                {
                    EndPlayerTurn();
                }
            }
            else if (state == BattleState.ENEMYTURN)
            {
               if (player.GetCurrentHealth() <= 0)
                {
                    state = BattleState.LOST;
                    SceneManager.LoadScene("GAME OVER");
                }
                else if (enemy.GetCurrentHealth() <= 0)
                {
                    state = BattleState.WON;
                    
                    GameData.EnemyLevel++;
                    if (GameData.EnemyLevel > 9){
                        GameData.EnemyLevel = 0;
                    }
                    SceneManager.LoadScene("WIN");
                }


                float turnBuffer = Random.Range(TurnTime / 2, TurnTime - 2);
                if (currentTurnTime <= turnBuffer && !takenTurn)
                {
                    EnemyTurn(); 
                }
                
                currentTurnTime -= Time.deltaTime;
                TimerSlider.value = currentTurnTime / TurnTime;

                if (currentTurnTime <= 0)
                {
                    EndEnemyTurn();
                }
            }
        }
        playerHUD.SetHUD(player);
        enemyHUD.SetHUD(enemy);

    }


}
