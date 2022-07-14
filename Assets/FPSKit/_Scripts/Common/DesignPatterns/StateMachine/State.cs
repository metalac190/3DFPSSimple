using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected float StateDuration = 0;

    public virtual void Enter() 
    {
        StateDuration = 0;
    }

    public virtual void Exit() 
    {
        StateDuration = 0;
    }

    public virtual void Update() 
    {
        StateDuration += Time.deltaTime;
    }

    public virtual void FixedUpdate() 
    { 
    
    }
}
