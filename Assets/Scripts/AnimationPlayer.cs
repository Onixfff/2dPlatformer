using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationPlayer : MonoBehaviour
{
    private Animator _animator;

    public bool isMoving { private get; set; }
    public bool isFlying { private get; set; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetBool("isMoving", isMoving);
        _animator.SetBool("isFlying", isFlying);
    }

    public void OnJumpAnimator(bool jump)
    {
        _animator.SetBool("Jump", jump);
    }
}
