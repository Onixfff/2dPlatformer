using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationPlayer : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnWalkAnimator(bool speed)
    {
        _animator.SetBool("Run", speed);
    }

    public void OnJumpAnimator(bool jump)
    {
        _animator.SetBool("Jump", jump);
    }
}
