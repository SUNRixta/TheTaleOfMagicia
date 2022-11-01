using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public enum BattleState { Start, PlayerAction, PlayerMove, PlayerSelectEnemy, PlayerCasting, EnemyMove, Busy }
public class BattleSystem : MonoBehaviour
{
    [SerializeField] List<BattleUnit> playerUnits;
    [SerializeField] List<Animator> playerAnimators;
    [SerializeField] List<BattleUnit> enemyUnits;
    [SerializeField] List<Animator> enemyAnimators;
    [SerializeField] List<BattleHUD> playerHUD;
    [SerializeField] List<EnemyHUD> enemyHUD;
    [SerializeField] BattleDialogueBox battleDialogueBox;
    [SerializeField] AudioSource hoverSFX;
    [SerializeField] AudioSource confirmSFX;
    [SerializeField] AudioSource declineSFX;

    BattleState state;
    int currentAction;
    int currentSpell;
    int currentEnemyUnit;
    int randBattleDialogue;
    int ActionIndex;
    int SpellIndex;
    int AllyIndex = 0;
    int EnemyIndex;
    int EnemyCount;

    private KeywordRecognizer keywordRecognise;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();
    private float countTime;
    private bool counterActive;


    private void Start()
    {
        StartCoroutine(SetupBattle());
        EnemyCount = enemyUnits.Count;

        actions.Add("Chrono Cut", AttackCasted);
        actions.Add("Edge of Chronia", AttackCasted);

        keywordRecognise = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognise.OnPhraseRecognized += RecognisedSpeech;
    }

    private void RecognisedSpeech(PhraseRecognizedEventArgs speech)
    {
        if (speech.text == playerUnits[AllyIndex].Character.spells[SpellIndex].Base.name)
        {
            actions[playerUnits[AllyIndex].Character.spells[SpellIndex].Base.name].Invoke();
        }
    }

    private void startCount()
    {
        counterActive = true;
    }
    private void stopCount()
    {
        countTime = 0;
        counterActive = false;
    }

    private void AttackCasted()
    {
        keywordRecognise.Stop();
        stopCount();
        playerAnimators[AllyIndex].SetBool("isCasting", false);
        StartCoroutine(CastingComplete());
    }

    public IEnumerator SetupBattle()
    {
        for (int i = 0; i < playerUnits.Count; ++i)
        {
            playerUnits[i].SetUp();
            playerHUD[i].SetData(playerUnits[i].Character);

            battleDialogueBox.SetActionNames(playerUnits[i].Character.actions);
            battleDialogueBox.SetMoveNames(playerUnits[i].Character.spells);
            battleDialogueBox.SetEnemyNames(enemyUnits);
        }

        randBattleDialogue = UnityEngine.Random.Range(1, 4);
        for (int i = 0; i < enemyUnits.Count; ++i)
        {
            enemyUnits[i].SetUp();
            enemyHUD[i].SetData(enemyUnits[i].Character);
        }


        if (enemyUnits.Count == 1)
        {
            if (randBattleDialogue == 1)
            {
                yield return battleDialogueBox.TypeDialogue($"{enemyUnits[0].Character.Base.name} engages!");
                yield return new WaitForSeconds(1f);
                StartCoroutine(PlayerAction());
            }
            else if (randBattleDialogue == 2)
            {
                yield return battleDialogueBox.TypeDialogue($"{enemyUnits[0].Character.Base.name} came out of nowhere!");
                yield return new WaitForSeconds(1f);
                StartCoroutine(PlayerAction());
            }
            else
            {
                yield return battleDialogueBox.TypeDialogue($"You encountered {enemyUnits[0].Character.Base.name}!");
                yield return new WaitForSeconds(1f);
                StartCoroutine(PlayerAction());
            }
        }
        else
        {
            if (randBattleDialogue == 1)
            {
                yield return battleDialogueBox.TypeDialogue($"A group of enemies engages!");
                yield return new WaitForSeconds(1f);
                StartCoroutine(PlayerAction());
            }
            else if (randBattleDialogue == 2)
            {
                yield return battleDialogueBox.TypeDialogue($"A group of enemies came out of nowhere!");
                yield return new WaitForSeconds(1f);
                StartCoroutine(PlayerAction());
            }
            else
            {
                yield return battleDialogueBox.TypeDialogue($"You encountered a group of enemies!");
                yield return new WaitForSeconds(1f);
                StartCoroutine(PlayerAction());
            }
        }
    }

    public IEnumerator PlayerAction()
    {
        battleDialogueBox.EnableBattleDialogueText(true);
        yield return battleDialogueBox.TypeDialogue("Choose your action...");
        yield return new WaitForSeconds(0.2f);
        state = BattleState.PlayerAction;
        battleDialogueBox.EnableCharacterUI(true);
        battleDialogueBox.EnableActionSelector(true);
    }

    public void PlayerMove()
    {
        state = BattleState.PlayerMove;
        battleDialogueBox.EnableActionSelector(false);
        battleDialogueBox.EnableMoveSelector(true);
    }

    public void PlayerSelectEnemy()
    {
        state = BattleState.PlayerSelectEnemy;
        battleDialogueBox.EnableEnemySelector(true);
    }
    
    public void PlayerCasting()
    {
        state = BattleState.PlayerCasting;
        playerAnimators[AllyIndex].SetBool("isCasting", true);
    }

    public IEnumerator PerformPlayerMove(int allyIndex, int actionIndex, int spellIndex, int enemyIndex)
    {
        state = BattleState.Busy;
        battleDialogueBox.EnableBattleDialogueText(true);

        if (actionIndex == 0)
        {
            playerAnimators[allyIndex].SetBool("isCasting", true);
            var action = playerUnits[allyIndex].Character.actions[spellIndex];
            yield return battleDialogueBox.TypeDialogue($"{playerUnits[AllyIndex].Character.Base.name} used {action.Base.name}");
            yield return new WaitForSeconds(0.5f);

            bool isFainted = enemyUnits[enemyIndex].Character.TakePhysicalDamage(action, playerUnits[AllyIndex].Character);
            playerAnimators[allyIndex].SetBool("isCasting", false);
            enemyAnimators[enemyIndex].SetBool("isHurt", true);
            enemyHUD[enemyIndex].UpdateHP();
            yield return new WaitForSeconds(0.5f);

            if (isFainted)
            {
                enemyAnimators[enemyIndex].SetBool("isHurt", false);
                enemyAnimators[enemyIndex].SetBool("isDead", true);
                enemyHUD[enemyIndex].Dead();
                EnemyCount--;
                if (EnemyCount <= 0)
                {
                    yield return battleDialogueBox.TypeDialogue($"You defeated {enemyUnits[0].Character.Base.Name}!");
                    yield return new WaitForSeconds(2f);
                    FindObjectOfType<SceneSwitcher>().LoadNextScene();
                }
                else
                {
                    EnemyCount--;
                    StartCoroutine(EnemyMove());
                }
            }
            else
            {
                yield return new WaitForSeconds(1f);
                enemyAnimators[enemyIndex].SetBool("isHurt", false);
                StartCoroutine(EnemyMove());                
            }
        }
        else if (actionIndex == 1)
        {
            var spell = playerUnits[allyIndex].Character.spells[spellIndex];
            yield return battleDialogueBox.TypeDialogue($"{playerUnits[AllyIndex].Character.Base.name} used {spell.Base.name}");
            yield return new WaitForSeconds(0.5f);

            bool isFainted = enemyUnits[enemyIndex].Character.TakeMagicDamage(spell, playerUnits[AllyIndex].Character);
            enemyAnimators[enemyIndex].SetBool("isHurt", true);
            enemyHUD[enemyIndex].UpdateHP();
            yield return new WaitForSeconds(0.5f);

            if (isFainted)
            {
                enemyAnimators[enemyIndex].SetBool("isHurt", false);
                enemyAnimators[enemyIndex].SetBool("isDead", true);
                enemyHUD[enemyIndex].Dead();
                EnemyCount--;
                if (EnemyCount <= 0)
                {
                    yield return battleDialogueBox.TypeDialogue($"You defeated {enemyUnits[0].Character.Base.Name}!");
                    yield return new WaitForSeconds(2f);
                    FindObjectOfType<SceneSwitcher>().LoadNextScene();
                }
                else
                {
                    StartCoroutine(EnemyMove());
                }
            }
            else
            {
                yield return new WaitForSeconds(1f);
                enemyAnimators[enemyIndex].SetBool("isHurt", false);
                StartCoroutine(EnemyMove());
            }
        }
        
    }

    public IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;
        battleDialogueBox.EnableBattleDialogueText(true);

        if (EnemyCount == 1)
        {
            for (int i = 0; i < playerUnits.Count; ++i)
            {
                enemyAnimators[0].SetBool("isAttack", true);
                var spell = enemyUnits[0].Character.GetRandomSpell();
                yield return battleDialogueBox.TypeDialogue($"{enemyUnits[0].Character.Base.name} used {spell.Base.name}");
                yield return new WaitForSeconds(0.5f);

                bool isFainted = playerUnits[i].Character.TakeMagicDamage(spell, playerUnits[i].Character);
                playerAnimators[i].SetBool("isHurt", true);
                playerHUD[i].UpdateHUD();
                yield return new WaitForSeconds(0.5f);

                if (isFainted)
                {
                    playerAnimators[i].SetBool("isHurt", false);
                    enemyAnimators[0].SetBool("isAttack", false);
                    yield return battleDialogueBox.TypeDialogue($"{playerUnits[AllyIndex].Character.Base.name} is knocked out!");
                    yield return new WaitForSeconds(2f);
                    FindObjectOfType<SceneSwitcher>().LoadNextScene();
                    playerUnits[i].Character.HP = playerUnits[i].Character.Base.MaxHP;

                }
                else
                {
                    yield return new WaitForSeconds(0.5f);
                    playerAnimators[i].SetBool("isHurt", false);
                    enemyAnimators[0].SetBool("isAttack", false);
                    battleDialogueBox.EnableBattleDialogueText(false);
                    StartCoroutine(PlayerAction());
                }
            }
        }
        else
        {
            for (int e = 0; e < EnemyCount; ++e)
            {
                for (int i = 0; i < playerUnits.Count; ++i)
                {
                    enemyAnimators[e].SetBool("isAttack", true);
                    var spell = enemyUnits[e].Character.GetRandomSpell();
                    yield return battleDialogueBox.TypeDialogue($"{enemyUnits[e].Character.Base.name} used {spell.Base.name}");
                    yield return new WaitForSeconds(0.5f);

                    bool isFainted = playerUnits[i].Character.TakeMagicDamage(spell, playerUnits[i].Character);
                    playerAnimators[i].SetBool("isHurt", true);
                    playerHUD[i].UpdateHUD();
                    yield return new WaitForSeconds(0.5f);

                    if (isFainted)
                    {
                        playerAnimators[i].SetBool("isHurt", false);
                        enemyAnimators[e].SetBool("isAttack", false);
                        yield return battleDialogueBox.TypeDialogue($"{playerUnits[AllyIndex].Character.Base.name} is knocked out!");
                        yield return new WaitForSeconds(2f);
                        FindObjectOfType<SceneSwitcher>().LoadNextScene();
                        playerUnits[i].Character.HP = playerUnits[i].Character.Base.MaxHP;

                    }
                    else
                    {
                        yield return new WaitForSeconds(0.5f);
                        playerAnimators[i].SetBool("isHurt", false);
                        enemyAnimators[e].SetBool("isAttack", false);
                    }
                }
            }
            StartCoroutine(PlayerAction());

        }
        
    }

    private void Update()
    {
        if (state == BattleState.PlayerAction)
        {
            HandleActionSelection();
        }
        else if (state == BattleState.PlayerMove)
        {
            HandleMoveSelection();
        }
        else if (state == BattleState.PlayerSelectEnemy)
        {
            HandleEnemySelection(ActionIndex);
        }
        else if (state == BattleState.PlayerCasting)
        {
            Casting();
        }
    }

    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (currentAction % 2 < 1)
            {
                hoverSFX.Play();
                ++currentAction;
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (currentAction % 2 > 0)
            {
                hoverSFX.Play();
                --currentAction;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (currentAction < 5)
            {
                hoverSFX.Play();
                currentAction += 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (currentAction > 1)
            {
                hoverSFX.Play();
                currentAction -= 2;
            }
        }

        battleDialogueBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.C))
        {
            confirmSFX.Play();
            ActionIndex = currentAction;
            if (currentAction == 0)
            {
                battleDialogueBox.EnableCastingBox(true);
                PlayerSelectEnemy();
            }
            else if (currentAction == 1)
            {
                PlayerMove();
            }
        }
    }

    void HandleMoveSelection()
    {
        if (playerUnits[AllyIndex].Character.spells.Count > 1)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (currentSpell < playerUnits[AllyIndex].Character.spells.Count - 1)
                {
                    hoverSFX.Play();
                    ++currentSpell;
                }
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                if (currentSpell > 0)
                {
                    hoverSFX.Play();
                    --currentSpell;
                }
            }
        }
        else
        {
            currentSpell = 0;
        }
        battleDialogueBox.UpdateMoveSelection(currentSpell, playerUnits[AllyIndex].Character.spells[currentSpell]);

        if (Input.GetKeyDown(KeyCode.C))
        {
            confirmSFX.Play();
            SpellIndex = currentSpell;
            battleDialogueBox.EnableActionSelector(false);
            PlayerSelectEnemy();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            declineSFX.Play();
            battleDialogueBox.EnableMoveSelector(false);
            battleDialogueBox.EnableCastingBox(true);
            StartCoroutine(PlayerAction());
        }
    }

    void HandleEnemySelection(int actionIndex)
    {
        if (enemyUnits.Count > 1)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (currentEnemyUnit < enemyUnits.Count - 1)
                {
                    hoverSFX.Play();
                    ++currentEnemyUnit;
                }
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                if (currentEnemyUnit > 0)
                {
                    hoverSFX.Play();
                    --currentEnemyUnit;
                }
            }
        }
        else
        {
            currentEnemyUnit = 0;
        }
        battleDialogueBox.UpdateEnemySelection(currentEnemyUnit);

        if (Input.GetKeyDown(KeyCode.C))
        {
            EnemyIndex = currentEnemyUnit;
            confirmSFX.Play();
            if (actionIndex == 0)
            {
                battleDialogueBox.EnableEnemySelector(false);
                battleDialogueBox.EnableActionSelector(false);
                battleDialogueBox.EnableCastingBox(true);
                StartCoroutine(PerformPlayerMove(AllyIndex, actionIndex, SpellIndex, EnemyIndex));
            }
            else if (actionIndex == 1)
            {
                battleDialogueBox.EnableMoveSelector(false);
                battleDialogueBox.EnableBattleDialogueText(false);
                battleDialogueBox.EnableEnemySelector(false);
                battleDialogueBox.EnableCastingBox(true);
                PlayerCasting();
                StartCoroutine(CastingText());
                keywordRecognise.Start();
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            declineSFX.Play();
            if (actionIndex == 0)
            {
                battleDialogueBox.EnableEnemySelector(false);
                battleDialogueBox.EnableActionSelector(false);
                battleDialogueBox.EnableCastingBox(true);
                StartCoroutine(PlayerAction());
            }
            else if (actionIndex == 1)
            {
                battleDialogueBox.EnableEnemySelector(false);
                battleDialogueBox.EnableMoveSelector(true);
                PlayerMove();
            }
        }
    }

    public void Casting()
    {
        startCount();
        if (counterActive == true)
        {
            countTime += Time.deltaTime;
        }
        if (countTime > 10)
        {
            stopCount();
            keywordRecognise.Stop();
            playerAnimators[AllyIndex].SetBool("isCasting", false);
            StartCoroutine(CastingFailed());
        }
    }

    public IEnumerator CastingText()
    {
        yield return battleDialogueBox.TypeCasting("Casting Magic In Progress...");
    }

    public IEnumerator CastingFailed()
    {
        yield return battleDialogueBox.TypeCasting("Casting Failed!");
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(EnemyMove());
    }

    public IEnumerator CastingComplete()
    {
        yield return battleDialogueBox.TypeCasting("Casting Complete!");
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(PerformPlayerMove(AllyIndex, ActionIndex, SpellIndex, EnemyIndex));
    }
}