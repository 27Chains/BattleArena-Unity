using FishNet.Object;
using UnityEngine;

public class StateMachine : NetworkBehaviour
{
    public State currentState;

    public void Initialize(State startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    [ServerRpc(RunLocally = true)]
    public void ChangeState(PlayerState newState)
    {
        currentState.Exit();
        currentState = currentState.character.GetState(newState);
        currentState.Enter();
    }
}
