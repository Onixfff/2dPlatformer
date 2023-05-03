using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class InputPlayer : MonoBehaviour
{
    private PlayerController _controls;
    private EnumMove _enumMove;
    private bool _jumpInput;

    private void Awake()
    {
        int[] r = new int[4];
        int[] e = new int[r.Count()];
        for(int i = e.Count() - 1; i>= 0; i--)  
        _enumMove = new EnumMove();
        _controls = new PlayerController();
        _controls.Player.Enable();
    }

    private void OnEnable()
    {
        _controls.Player.Move.performed += ctx =>  OnMovePerformed(ctx.ReadValue<Vector2>());
        _controls.Player.Move.canceled += ctx => OnMoveCanceled();
        _controls.Player.Jump.performed += ctx =>  OnJumpPerformed();
    }

    private void OnDisable()
    {
        _controls.Player.Move.performed -= ctx => OnMovePerformed(ctx.ReadValue<Vector2>());
        _controls.Player.Move.canceled -= ctx => OnMoveCanceled();
        _controls.Player.Jump.performed -= ctx => OnJumpPerformed();
    }

    private void OnMovePerformed(Vector2 value)
    {
        Debug.Log("Вошёл в отслеживание");
        if (value.x > 0)
            _enumMove = EnumMove.Right;
        else if(value.x < 0)
            _enumMove = EnumMove.Left;
        else if(value.y > 0)
            _enumMove = EnumMove.Up;
        else if(value.y < 0)
            _enumMove = EnumMove.Down;
    }

    private void OnMoveCanceled()
    {
        _enumMove = EnumMove.Zero;
    }

    private void OnJumpPerformed()
    {
        _jumpInput = true;
    }

    public EnumMove GetMoveInput()
    {
        return _enumMove;
    }

    public bool GetJumpInput()
    {
        return _jumpInput;
    }

    public void ResetJumpInput()
    {
        _jumpInput = false;
    }

}
