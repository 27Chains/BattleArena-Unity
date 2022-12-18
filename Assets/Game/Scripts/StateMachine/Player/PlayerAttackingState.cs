// using UnityEngine;
// using UnityEngine.InputSystem;

// public class PlayerAttackingState : PlayerBaseState
// {
//     private float transitionDuration = 0.1f;

//     private int attackIndex = 0;

//     private bool comboFailed;

//     private float weaponForce = 5f;

//     private float timePassed;

//     private float clipLength;

//     private float clipSpeed;

//     private bool alreadyAppliedForce;

//     private PlayerState nextComboState;

//     public PlayerAttackingState(
//         PlayerStateMachine stateMachine,
//         int attackIndex
//     ) :
//         base(stateMachine)
//     {
//         // TODO We are checking what animation to play based on the attack index which is hardcoded for each attack state
//         this.attackIndex = attackIndex;
//     }

//     public override void Enter()
//     {
//         Debug.Log("Entered Attacking State");
//         stateMachine.Animator.SetFloat("MovementSpeed", 0f);
//         if (!stateMachine.IsOwner) return;
//         stateMachine.Animator.SetTrigger("Attack");
//         stateMachine
//             .AudioSource
//             .PlayOneShot(stateMachine
//                 .SwordWhooshClips[Random
//                     .Range(0, stateMachine.SwordWhooshClips.Length)]);

//         // stateMachine.InputReader.AttackEvent += TryComboAttack;
//     }

//     private void Attack()
//     {
//         Vector3 mousePosition = GetMousePositionInWorld();
//         Vector3 direction =
//             (mousePosition - stateMachine.transform.position).normalized;
//         stateMachine.Player.ServerRotatePlayer (direction);
//     }

//     public override void Exit()
//     {
//         alreadyAppliedForce = false;
//         comboFailed = false;
//         if (stateMachine.IsServer)
//         {
//             stateMachine.DamageCollider.DisableCollider();
//         }
//         if (!stateMachine.IsOwner) return;

//         // stateMachine.InputReader.AttackEvent -= TryComboAttack;
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
//         timePassed += Time.deltaTime;
//         clipLength =
//             stateMachine.Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
//         clipSpeed = stateMachine.Animator.GetCurrentAnimatorStateInfo(0).speed;

//         if (timePassed >= clipLength / clipSpeed)
//         {
//             // stateMachine.SwitchState(PlayerState.Movement);
//         }
//     }

//     private Vector3 GetMousePositionInWorld()
//     {
//         LayerMask layerMask = LayerMask.GetMask("Ground");

//         Ray ray =
//             Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

//         if (Physics.Raycast(ray, out RaycastHit hit, 1000, layerMask))
//         {
//             return hit.point;
//         }

//         return Vector3.zero;
//     }

//     private void TryApplyForce()
//     {
//         if (alreadyAppliedForce) return;
//         alreadyAppliedForce = true;
//         stateMachine
//             .Player
//             .ServerApplyForce(stateMachine.transform.forward,
//             weaponForce,
//             ForceType.Smooth);
//     }

//     // // TODO thinking this could be done better
//     // private void TryComboAttack()
//     // {
//     //     if (comboFailed) return;
//     //     if (
//     //         attackIndex ==
//     //         stateMachine.Fighter.CurrentWeapon.AttackAnimations.Length - 1
//     //     )
//     //     {
//     //         comboFailed = true;
//     //         return;
//     //     }

//     //     float comboAttackTime =
//     //         stateMachine.Fighter.CurrentWeapon.ComboAttackTime[attackIndex];
//     //     float comboWindow =
//     //         stateMachine.Fighter.CurrentWeapon.ComboAttackWindow[attackIndex];

//     //     if (normalizedTime < comboAttackTime)
//     //     {
//     //         comboFailed = true;
//     //         return;
//     //     }
//     //     if (
//     //         normalizedTime > comboAttackTime &&
//     //         normalizedTime < comboAttackTime + comboWindow
//     //     )
//     //     {
//     //         // TODO This is ugly and hardly expandable
//     //         if (stateMachine.CurrentState == PlayerState.ComboAttack1)
//     //         {
//     //             stateMachine.SwitchState(PlayerState.ComboAttack2);
//     //         }
//     //         else
//     //         {
//     //             stateMachine.SwitchState(PlayerState.ComboAttack1);
//     //         }
//     //     }
//     // }
// }
