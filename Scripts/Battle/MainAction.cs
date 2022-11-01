using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAction
{
    public MainActionBase Base { get; set; }

    public MainAction(MainActionBase maBase)
    {
        Base = maBase;
    }
}
