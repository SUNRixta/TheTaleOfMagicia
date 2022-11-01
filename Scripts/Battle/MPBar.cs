using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPBar : MonoBehaviour
{
    [SerializeField] GameObject Mana;

    public void SetMP(float mpNormalized)
    {
        Mana.transform.localScale = new Vector3(mpNormalized, 1f);
    }
}