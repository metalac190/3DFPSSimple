
/// <summary>
/// Created by Adam Chandler, 2020
/// In order to implement this StateMachine, you need to do the following things:
/// 1. Create some sort of Controller that inherits from StateMachine
/// 2. Create a few classes that inherit from State
/// 3. Implement the required functions in the new State class
/// 4. Create a few instances of the class somewhere in your new Controller
/// 5. Use ChangeState(state) to Change to any of the states in your Controller!
/// 6. If the States need more information, you can pass them down in the Constructor when the Controller initializes them
/// </summary>

using System.Collections;
using UnityEngine;

public abstract class StateMachineMB : MonoBehaviour
{
    public State CurrentState { get; private set; }
    State _previousState;

    bool _inTransition = false;

    public void ChangeState(State newState)
    {
        // ensure we're ready for a new state
        if (CurrentState == newState || _inTransition)
            return;

        ChangeStateSequence(newState);
    }

    public void ChangeStateToPrevious()
    {
        if (_previousState != null)
            ChangeState(_previousState);
        else
            Debug.Log("Previous State Null");
    }

    void ChangeStateSequence(State newState)
    {
        _inTransition = true;
        // begin our exit sequence, to prepare for new state
        if (CurrentState != null)
            CurrentState.Exit();
        // save our current state, in case we want to return to it
        StoreStateAsPrevious(newState);

        CurrentState = newState;

        // begin our new Enter sequence
        if (CurrentState != null)
            CurrentState.Enter();

        _inTransition = false;
    }

    private void StoreStateAsPrevious(State newState)
    {
        if (_previousState == null)
        {
            _previousState = newState;
        }
        else if (_previousState != null && CurrentState != null)
        {
            _previousState = CurrentState;
        }
    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {

    }

    // pass down Update ticks to States, since they won't have a MonoBehaviour
    protected virtual void Update()
    {
        // simulate update ticks in states
        if (CurrentState != null && !_inTransition)
            CurrentState.Update();
    }

    protected virtual void FixedUpdate()
    {
        // simulate update ticks in states
        if (CurrentState != null && !_inTransition)
            CurrentState.FixedUpdate();
    }

    protected virtual void OnDestroy()
    {
        CurrentState?.Exit();
    }
}
