// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerBlockHitState : PlayerBaseState
// {
//     private int ImpactAnimHash = Animator.StringToHash("DamageTaken");

//     private int BlockHitAnimHash = Animator.StringToHash("Block-HitReact");

//     private float initialDuration = 0.5f;

//     private float crossFadeDuration = 0.1f;

//     private float duration = 0.5f;

//     public PlayerBlockHitState(PlayerStateMachine stateMachine) :
//         base(stateMachine)
//     {
//     }

//     public override void Enter()
//     {
//         if (!stateMachine.IsOwner) return;
//         stateMachine
//             .Player
//             .ServerRotatePlayer(stateMachine.IncomingHitDirection);
//         stateMachine.Player.ServerPlayAnim (
//             BlockHitAnimHash,
//             crossFadeDuration
//         );
//         stateMachine
//             .AudioSource
//             .PlayOneShot(stateMachine
//                 .SwordHitShieldClips[Random
//                     .Range(0, stateMachine.SwordHitShieldClips.Length)]);
//     }

//     public override void Exit()
//     {
//         duration = initialDuration;
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
//             if (stateMachine.InputReader.IsBlocking)
//             {
//                 stateMachine.SwitchState(PlayerState.Block);
//             }
//             else
//             {
//                 stateMachine.SwitchState(PlayerState.Movement);
//             }
//         }
//     }
// }
