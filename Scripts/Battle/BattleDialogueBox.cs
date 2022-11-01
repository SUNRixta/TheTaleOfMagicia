using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleDialogueBox : MonoBehaviour
{
    [SerializeField] BattleSystem battleSystem;


    [SerializeField] int letterPerSecond;
    [SerializeField] Color highlightedColor;

    [SerializeField] TMP_Text battleDialogueText;
    [SerializeField] TMP_Text castingText;
    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject characterUI;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDetails;
    [SerializeField] GameObject enemySelector;
    [SerializeField] GameObject castingBox;

    [SerializeField] List<TMP_Text> actionTexts;
    [SerializeField] List<TMP_Text> spellTexts;
    [SerializeField] List<TMP_Text> enemyNameTexts;

    [SerializeField] TMP_Text moveDescription;

    public void SetDialogue(string battleDialogue)
    {
        battleDialogueText.text = battleDialogue;
    }

    public IEnumerator TypeDialogue(string battleDialogue)
    {
        battleDialogueText.text = "";
        foreach (var letter in battleDialogue.ToCharArray())
        {
            battleDialogueText.text += letter;
            yield return new WaitForSeconds(1f / letterPerSecond);
        }
    }
    public IEnumerator TypeCasting(string battleDialogue)
    {
        castingText.text = "";
        foreach (var letter in battleDialogue.ToCharArray())
        {
            castingText.text += letter;
            yield return new WaitForSeconds(1f / letterPerSecond);
        }
    }

    public void EnableBattleDialogueText(bool enabled)
    {
        gameObject.SetActive(enabled);
    }
    public void EnableCharacterUI(bool enabled)
    {
        characterUI.SetActive(enabled);
    }
    
    public void EnableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }
    public void EnableMoveSelector(bool enabled)
    {
        moveSelector.SetActive(enabled);
        moveDetails.SetActive(enabled);
    }
    public void EnableEnemySelector(bool enabled)
    {
        enemySelector.SetActive(enabled);
    }
    public void EnableCastingBox(bool enabled)
    {
        castingText.text = string.Empty;
        castingBox.SetActive(enabled);
    }

    public void SetActionNames(List<MainAction> actions)
    {
        for (int i = 0; i < actionTexts.Count; i++)
        {
            if (i < actions.Count)
                actionTexts[i].text = actions[i].Base.name;
            else
                actionTexts[i].text = "-";
        }
    }

    public void SetMoveNames(List<Spell> spells) 
    {
        for (int i = 0; i < spellTexts.Count; i++)
        {
            if (i < spells.Count)
                spellTexts[i].text = spells[i].Base.name;
            else
                spellTexts[i].text = "-";
        }
    }

    public void SetEnemyNames(List<BattleUnit> enemyUnits)
    {
        for (int i = 0; i < enemyUnits.Count; i++)
        {
            if (i < enemyUnits.Count)
                enemyNameTexts[i].text = enemyUnits[i].name;
        }
    }

    public void UpdateActionSelection(int selectedAction)
    {
        for (int i=0; i < actionTexts.Count; ++i)
        {
            if (i == selectedAction)
            {
                actionTexts[i].color = highlightedColor;
            }
            else
            {
                actionTexts[i].color = Color.white;
            }
        }
    }

    public void UpdateMoveSelection(int selectedMove, Spell spell)
    {
        for (int i = 0; i < spellTexts.Count; ++i)
        {
            if (i == selectedMove)
            {
                spellTexts[i].color = highlightedColor;
            }
            else
            {
                spellTexts[i].color = Color.white;
            }
        }

        moveDescription.text = spell.Base.Description.ToString();
    }

    public void UpdateEnemySelection(int selectedEnemy)
    {
        for (int i = 0; i < enemyNameTexts.Count; ++i)
        {
            if (i == selectedEnemy)
            {
                enemyNameTexts[i].color = highlightedColor;
            }
            else
            {
                enemyNameTexts[i].color = Color.white;
            }
        }
    }
}
