using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject Health;

    public void SetHP(float hpNormalized)
    {
        Health.transform.localScale = new Vector3(hpNormalized, 1f);
    }
}