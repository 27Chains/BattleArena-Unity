// using UnityEngine;

// public class PlayerDeadState : PlayerBaseState
// {
//     private int DeadHash = Animator.StringToHash("Death");

//     private float deathScreenDuration = 3f;

//     private float crossFadeDuration = 0.1f;

//     public PlayerDeadState(PlayerStateMachine stateMachine) :
//         base(stateMachine)
//     {
//     }

//     public override void Enter()
//     {
//         if (!stateMachine.IsOwner) return;
//         stateMachine.Player.ServerPlayAnim (DeadHash, crossFadeDuration);

//         // TODO looks like character controller is still a valid collider after setting it to false, preferably we should disable it
//         // TODO possibly character controller is only disabled on the dead player and the server but not on observers
//         stateMachine.CharacterController.enabled = false;
//     }

//     public override void Exit()
//     {
//     }

//     public override void MovementUpdate(
//         MoveData moveData,
//         bool asServer,
//         bool replaying = false
//     )
//     {
//     }

//     public override void Tick(float deltaTime)
//     {
//         // TODO Do something after a duration, like respawn or return to main menu
//     }
// }
