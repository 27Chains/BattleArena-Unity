using UnityEngine;
using UnityEngine.InputSystem;

public abstract class State
{
    public Character character;

    public StateMachine stateMachine;

    public State(StateMachine _stateMachine, Character _character)
    {
        character = _character;
        stateMachine = _stateMachine;
    }

    public abstract void Enter();

    public abstract void Exit();

    public virtual void HandleInput()
    {
    }

    public virtual void LogicUpdate()
    {
    }

    public float
    GetNormalizedTime(Animator animator, int layerIndex, string animationName)
    {
        AnimatorStateInfo currentInfo =
            animator.GetCurrentAnimatorStateInfo(layerIndex);
        AnimatorStateInfo nextInfo =
            animator.GetNextAnimatorStateInfo(layerIndex);

        if (
            animator.IsInTransition(layerIndex) &&
            nextInfo.IsName(animationName)
        )
        {
            return nextInfo.normalizedTime;
        }
        else if (currentInfo.IsName(animationName))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }

    public Vector3 GetMousePositionInWorld()
    {
        LayerMask layerMask = LayerMask.GetMask("Ground");

        Ray ray =
            Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 1000, layerMask))
        {
            return hit.point;
        }

        return Vector3.zero;
    }
}
