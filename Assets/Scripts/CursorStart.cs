using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorStart : MonoBehaviour
{
    [SerializeField] private bool isLocked = true;

    private void Start()
    {
        SetCursor(isLocked);
    }

    public void SetCursor(bool isLocked)
    {
        if (isLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
