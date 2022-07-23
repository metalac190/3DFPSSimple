using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInput : MonoBehaviour
{
    public event Action SpacePressed;
    public event Action EscapePressed;
    public event Action BackspacePressed;

    void Update()
    {
        // Backspace - Reload
        //TODO make this work with controller
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            BackspacePressed?.Invoke();
            //LevelLoader.ReloadLevel();
        }
        // Escape - Quit
        if (Input.GetButtonDown("Cancel"))
        {
            EscapePressed?.Invoke();
            //Application.Quit();
        }
        // Spacebar - Confirm
        if (Input.GetButtonDown("Jump"))
        {
            SpacePressed?.Invoke();
        }
    }
}
