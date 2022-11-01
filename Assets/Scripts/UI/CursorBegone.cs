using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorBegone : MonoBehaviour
{
    private void Awake()
    {
        //Set Cursor to not be visible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
