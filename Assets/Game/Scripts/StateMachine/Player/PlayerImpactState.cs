using UnityEngine;

public class PlayerImpactState : PlayerBaseState
{
    private int ImpactAnimHash = Animator.StringToHash("DamageTaken");

    private float duration;

    public PlayerImpactState(PlayerStateMachine stateMachine) :
        base(stateMachine)
    {
        duration = stateMachine.KnockbackDuration;
    }

    public override void Enter()
    {
        if (!stateMachine.IsOwner) return;
        stateMachine.Player.ServerPlayAnim (ImpactAnimHash, crossFadeDuration);
        stateMachine
            .AudioSource
            .PlayOneShot(stateMachine
                .SwordHitClips[Random
                    .Range(0, stateMachine.SwordHitClips.Length)]);
    }

    public override void Exit()
    {
        duration = stateMachine.KnockbackDuration;
    }

    public override void MovementUpdate(
        MoveData moveData,
        bool asServer,
        bool replaying = false
    )
    {
        Move((float) stateMachine.Player.TimeManager.TickDelta);
    }

    public override void Tick(float deltaTime)
    {
        if (!stateMachine.IsOwner) return;
        duration -= deltaTime;
        if (duration <= 0)
        {
            stateMachine.SwitchState(PlayerState.Movement);
        }
    }
}
