// using UnityEngine;

// public class PlayerImpactState : PlayerBaseState
// {
//     private int ImpactAnimHash = Animator.StringToHash("DamageTaken");

//     private float duration;

//     private float crossFadeDuration = 0.1f;

//     public PlayerImpactState(PlayerStateMachine stateMachine) :
//         base(stateMachine)
//     {
//         duration = stateMachine.KnockbackDuration;
//     }

//     public override void Enter()
//     {
//         if (!stateMachine.IsOwner) return;

//         //  rotate player to face the direction of the incoming hit
//         Vector3 direction =
//             (
//             stateMachine.IncomingHitDirection - stateMachine.transform.position
//             ).normalized;

//         stateMachine.Player.ServerRotatePlayer (direction);

//         stateMachine.Player.ServerPlayAnim (ImpactAnimHash, crossFadeDuration);
//         stateMachine
//             .AudioSource
//             .PlayOneShot(stateMachine
//                 .SwordHitFleshClips[Random
//                     .Range(0, stateMachine.SwordHitFleshClips.Length)]);
//     }

//     public override void Exit()
//     {
//         duration = stateMachine.KnockbackDuration;
//     }

//     public override void MovementUpdate(
//         MoveData moveData,
//         bool asServer,
//         bool replaying = false
//     )
//     {
//         Move((float) stateMachine.Player.TimeManager.TickDelta);
//     }

//     public override void Tick(float deltaTime)
//     {
//         if (!stateMachine.IsOwner) return;
//         duration -= deltaTime;
//         if (duration <= 0)
//         {
//             stateMachine.SwitchState(PlayerState.Movement);
//         }
//     }
// }
