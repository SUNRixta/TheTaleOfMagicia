using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public CharacterBase Base { get; set; }
    public int Level { get; set; }
    public int HP { get; set; }
    public int MP { get; set; }

    public List<Spell> spells { get; set; }
    public List<MainAction> actions { get; set; }

    public Character(CharacterBase charUnit, int charLevel)
    {
        Base = charUnit;
        Level = charLevel;
        HP = MaxHP;
        MP = MaxMP;

        spells = new List<Spell>();
        foreach (var spell in Base.LearnableActions)
        {
            if(spell.Level <= Level)
            {
                spells.Add(new Spell(spell.Base));
            }
        }

        actions = new List<MainAction>();
        foreach (var action in Base.MainActions)
        {
            if (action.Level <= Level)
            {
                actions.Add(new MainAction(action.Base));
            }
        }
    }

    public int Attack
    {
        get { return Mathf.FloorToInt((Base.Atk * Level) / 100f) + 5; }
    }

    public int Defense
    {
        get { return Mathf.FloorToInt((Base.Def * Level) / 100f) + 5; }
    }
    public int Intelligence
    {
        get { return Mathf.FloorToInt((Base.Int * Level) / 100f) + 5; }
    }
    public int Resistance
    {
        get { return Mathf.FloorToInt((Base.Res * Level) / 100f) + 5; }
    }
    public int Speed
    {
        get { return Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5; }
    }
    public int Luck
    {
        get { return Mathf.FloorToInt((Base.Luck * Level) / 100f) + 5; }
    }
    public int MaxHP
    {
        get { return Mathf.FloorToInt((Base.MaxHP * Level) / 100f) + 10; }
    }
    public int MaxMP
    {
        get { return Mathf.FloorToInt((Base.MaxMP * Level) / 100f) + 10; }
    }

    public bool TakePhysicalDamage(MainAction action, Character attacker)
    {
        float modifiers = Random.Range(0.85f, 1f);
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * action.Base.Power * ((float)attacker.Attack / attacker.Defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        HP -= damage;
        if (HP <= 0)
        {
            HP = 0;
            return true;
        }

        return false;
    }

    public bool TakeMagicDamage(Spell spell, Character attacker)
    {
        float modifiers = Random.Range(0.85f, 1f);
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * spell.Base.Power * ((float)attacker.Attack / attacker.Defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        HP -= damage;
        if (HP <= 0)
        {
            HP = 0;
            return true;
        }

        return false;
    }

    public Spell GetRandomSpell()
    {
        int r = Random.Range(0, spells.Count);
        return spells[r];
    }
}