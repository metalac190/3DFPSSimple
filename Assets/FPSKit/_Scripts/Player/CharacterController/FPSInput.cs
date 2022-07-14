using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FPSInput : MonoBehaviour
{
    public event Action PrimaryInputPressed;
    public event Action PrimaryInputReleased;
    public bool IsPrimaryInputHeld { get; private set; }

    public event Action SecondaryInputPressed;
    public event Action SecondaryInputReleased;
    public bool IsSecondaryInputHeld { get; private set; }

    public event Action JumpInputPressed;
    public event Action JumpInputReleased;
    public bool IsJumpInputHeld { get; private set; }

    public event Action SprintInputPressed;
    public event Action SprintInputReleased;
    public bool IsSprintInputHeld { get; private set; }

    void Update()
    {
        DetectPrimaryInput();
        DetectSecondaryInput();
        DetectJumpInput();
        DetectSprintInput();
    }

    private void DetectSprintInput()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            SprintInputPressed?.Invoke();
            IsSprintInputHeld = true;
        }
        else if (Input.GetButtonUp("Fire3"))
        {
            SprintInputReleased?.Invoke();
            IsSprintInputHeld = false;
        }
    }

    private void DetectJumpInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            JumpInputPressed?.Invoke();
            IsJumpInputHeld = true;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            JumpInputReleased?.Invoke();
            IsJumpInputHeld = false;
        }
    }

    private void DetectSecondaryInput()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            SecondaryInputPressed?.Invoke();
            IsSecondaryInputHeld = true;
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            SecondaryInputReleased?.Invoke();
            IsSecondaryInputHeld = false;
        }
    }

    private void DetectPrimaryInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            PrimaryInputPressed?.Invoke();
            IsPrimaryInputHeld = true;
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            PrimaryInputReleased?.Invoke();
            IsPrimaryInputHeld = false;
        }
    }
}
