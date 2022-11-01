using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHUD : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] HPBar hpBar;

    Character _character;

    public void SetData(Character character)
    {
        _character = character;
        hpBar.SetHP((float)character.HP / character.MaxHP);
    }

    public void UpdateHP()
    {
        hpBar.SetHP((float)_character.HP / _character.MaxHP);
    }
    public void Dead()
    {
        gameObject.SetActive(false);
    }

}
