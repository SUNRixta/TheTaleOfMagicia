using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text lvlText;
    [SerializeField] TMP_Text hpText;
    [SerializeField] TMP_Text mpText;
    [SerializeField] HPBar hpBar;
    [SerializeField] MPBar mpBar;
    
    Character _character;

    public void SetData(Character character)
    {
        _character = character;

        nameText.text = character.Base.Name;
        lvlText.text = "Lvl" + character.Level;
        hpText.text = character.HP + "/" + character.MaxHP;
        hpBar.SetHP((float) character.HP / character.MaxHP);
        mpText.text = character.MP + "/" + character.MaxMP;
        mpBar.SetMP((float)character.HP / character.MaxMP);
    }

    public void UpdateHUD()
    {
        hpBar.SetHP((float)_character.HP / _character.MaxHP);
        mpBar.SetMP((float)_character.MP / _character.MaxMP);
        hpText.text = _character.HP + "/" + _character.MaxHP;
        mpText.text = _character.MP + "/" + _character.MaxMP;
    }
}
