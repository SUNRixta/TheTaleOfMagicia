using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell
{
    public SpellBase Base { get; set; }
    public int UsedMP { get; set; }

    public Spell(SpellBase sBase)
    {
        Base = sBase;
        UsedMP = sBase.UsedMP;
    }
}
