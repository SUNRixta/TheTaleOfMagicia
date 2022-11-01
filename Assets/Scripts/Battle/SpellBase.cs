using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Create New Spells")]
public class SpellBase : ScriptableObject
{
    [SerializeField] string spellName;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] SpellTypes spellTypes;
    [SerializeField] int power;
    [SerializeField] int accuracy;
    [SerializeField] int usedMP;

    public string Spell
    {
        get { return spellName; }
    }

    public string Description
    {
        get { return description; }
    }

    public SpellTypes SpellTypes
    {
        get { return spellTypes; }
    }

    public int Power
    {
        get { return power; }
    }

    public int Accuracy
    {
        get { return accuracy; }
    }

    public int UsedMP
    {
        get { return usedMP; }
    }
}

public enum SpellTypes
{
    Attack,
    Magic,
    Defend,
    Heal,
    Items,
    StatusAfflict,
    StatusCure
}