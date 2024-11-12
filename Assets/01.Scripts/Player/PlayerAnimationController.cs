using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController
{
    private readonly Animator _animator;
    private static readonly int IsRunning = Animator.StringToHash("IsRunning");
    private static readonly int IsAim = Animator.StringToHash("IsAim");
    private static readonly int MovementX = Animator.StringToHash("MovementX");
    private static readonly int MovementY = Animator.StringToHash("MovementY");
    private static readonly int IsSkill = Animator.StringToHash("IsSkill");
    private static readonly int DieTrigger = Animator.StringToHash("Die");

    public PlayerAnimationController(Animator animator)
    {
        _animator = animator;
    }

    public void UpdateAnimations(MovementInfo movementInfo)
    {
        if (movementInfo.IsAimLocked)
        {
            UpdateAimAnimations(movementInfo.Movement);
        }
        else
        {
            _animator.SetFloat(IsRunning, movementInfo.Movement.magnitude);
        }

        _animator.SetBool(IsAim, movementInfo.IsAimLocked);
        _animator.SetBool(IsSkill, movementInfo.IsSkillActive);
    }

    private void UpdateAimAnimations(Vector3 movement)
    {
        Vector3 localMovement = _animator.transform.InverseTransformDirection(movement);
        _animator.SetFloat(MovementX, localMovement.x);
        _animator.SetFloat(MovementY, localMovement.z);
    }

    public void TriggerDeathAnimation()
    {
        _animator.SetBool(DieTrigger, true);
    }
}
