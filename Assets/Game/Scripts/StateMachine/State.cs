using UnityEngine;

public abstract class State
{
    protected const float crossFadeDuration = 0.1f;

    public abstract void Enter();

    public abstract void Tick(float deltaTime);

    public abstract void Exit();

    public abstract void MovementUpdate(
        MoveData moveData,
        bool asServer,
        bool replaying = false
    );

    protected float GetNormalizedTime(Animator animator, string AnimationName)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if (animator.IsInTransition(0) && nextInfo.IsName(AnimationName))
        {
            return nextInfo.normalizedTime;
        }
        else if (!currentInfo.IsName(AnimationName))
        {
            return nextInfo.normalizedTime;
        }
        else if (
            !animator.IsInTransition(0) && currentInfo.IsName(AnimationName)
        )
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }
}
