using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MainActions/Create New Actions")]
public class MainActionBase : ScriptableObject
{
    [SerializeField] string mainAction;

    [SerializeField] SpellTypes spellTypes;
    [SerializeField] int power;
    [SerializeField] int accuracy;

    public string MainAction
    {
        get { return mainAction; }
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
}
