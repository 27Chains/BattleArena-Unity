// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerBlockingState : PlayerBaseState
// {
//     private int BlockAnimHash = Animator.StringToHash("Block");

//     private float crossFadeDuration = 0.1f;

//     public PlayerBlockingState(PlayerStateMachine stateMachine) :
//         base(stateMachine)
//     {
//     }

//     public override void Enter()
//     {
//         if (stateMachine.IsServer)
//         {
//             stateMachine.BlockingCollider.EnableCollider();
//         }
//         if (!stateMachine.IsOwner) return;
//         stateMachine.Player.ServerPlayAnim (BlockAnimHash, crossFadeDuration);
//     }

//     public override void Exit()
//     {
//         if (stateMachine.IsServer)
//         {
//             stateMachine.BlockingCollider.DisableCollider();
//         }
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
//         if (!stateMachine.InputReader.IsBlocking)
//         {
//             stateMachine.SwitchState(PlayerState.Movement);
//         }
//     }
// }
