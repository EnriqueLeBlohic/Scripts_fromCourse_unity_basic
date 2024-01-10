using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationId {
    Idle = 0,
    Run = 1,
    PrepareJump=2,
    Jump=3,
    Attack=4,
    Hurt=5,
    UsePowerUp=6
}

public class AnimatorController : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Play(AnimationId animationId){

        animator.Play(animationId.ToString());
    }
}
