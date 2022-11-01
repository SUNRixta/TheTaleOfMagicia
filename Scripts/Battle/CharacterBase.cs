using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterStats/Create New Character Stats")]
public class CharacterBase : ScriptableObject
{
    [SerializeField] string charName;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] Sprite sprNormal;
    [SerializeField] Sprite sprAttack;
    [SerializeField] Sprite sprCasting;
    [SerializeField] Sprite sprHurt;
    [SerializeField] Sprite sprFallen;

    [SerializeField] int maxHP;
    [SerializeField] int maxMP;
    [SerializeField] int ATK;
    [SerializeField] int DEF;
    [SerializeField] int INT;
    [SerializeField] int RES;
    [SerializeField] int SPD;
    [SerializeField] int LUK;

    [SerializeField] List<LearnableActions> learnableActions;
    [SerializeField] List<MainActions> mainActions;

    public string GetName()
    {
        return charName;
    }

    public string Name
    {
        get { return charName; }
    }

    public string Description
    {
        get { return description; }
    }

    public Sprite NormalSprite
    {
        get { return sprNormal; }
    }

    public Sprite AttackSprite
    {
        get { return sprAttack; }
    }

    public Sprite CastingSprite
    {
        get { return sprCasting; }
    }

    public Sprite HurtSprite
    {
        get { return sprHurt; }
    }

    public Sprite FallenSprite
    {
        get { return sprFallen; }
    }

    public int MaxHP
    {
        get { return maxHP; }
    }

    public int MaxMP
    {
        get { return maxMP; }
    }

    public int Atk
    {
        get { return ATK; }
    }

    public int Def
    {
        get { return DEF; }
    }

    public int Int
    {
        get { return INT; }
    }

    public int Res
    {
        get { return RES; }
    }

    public int Speed
    {
        get { return SPD; }
    }

    public int Luck
    {
        get { return LUK; }
    }

    public List<LearnableActions> LearnableActions
    {
        get { return learnableActions; }
    }
    public List<MainActions> MainActions
    {
        get { return mainActions; }
    }
}

[System.Serializable]
public class LearnableActions
{
    [SerializeField] SpellBase spellBase;
    [SerializeField] int level;

    public SpellBase Base {
        get { return spellBase; }
    } 

    public int Level
    {
        get { return level; }
    }

    internal void Add(Spell spell)
    {
        throw new NotImplementedException();
    }
}

[System.Serializable]
public class MainActions
{
    [SerializeField] MainActionBase mainActionBase;
    [SerializeField] int level;

    public MainActionBase Base
    {
        get { return mainActionBase; }
    }

    public int Level
    {
        get { return level; }
    }

    internal void Add(MainAction mainAction)
    {
        throw new NotImplementedException();
    }
}