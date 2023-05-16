using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

[RequireComponent(typeof(InputPlayer))]
[RequireComponent(typeof(Transform))]
public class MovePlayer : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForse;
    [SerializeField] private float _jumpTime;
    [SerializeField] private AnimationPlayer _playerAnimator;
    [SerializeField] private Vector3 _groundCheckOffset;
    [SerializeField] private float _rayLength = 0.2f;
    [SerializeField] private LayerMask _layerMask;

    private bool _isMoving;
    private float _gravityScaleStart;
    private float _gravityScaleJump = 3;
    private bool _isFlying;
    private SpriteRenderer _spriteRenderer;
    private bool _isGround;
    private InputPlayer _inputPlayer;
    private Rigidbody2D _rigidbody2D;
    private EnumMove _enumMove;
    private bool _jump;
    private float _jumpTimer;
    private Transform _transform;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _inputPlayer = GetComponent<InputPlayer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _enumMove = new EnumMove();
        _gravityScaleStart = _rigidbody2D.gravityScale;
    }

    private void FixedUpdate()
    {
        OnMove();
        CheckGround();
        OnJump();
    }

    private void Update()
    {
        _enumMove = _inputPlayer.GetMoveInput();
        _jump = _inputPlayer.GetJumpInput();
    }

    public void OnMove()
    {
        switch (_enumMove)
        {
            case EnumMove.Right:
                _rigidbody2D.velocity = new Vector2(Vector2.right.x * _speed * Time.deltaTime, _rigidbody2D.velocity.y);
                _spriteRenderer.flipX = false;
                _isMoving = true;
                break;
            case EnumMove.Left:
                _rigidbody2D.velocity = new Vector2(Vector2.left.x * _speed * Time.deltaTime, _rigidbody2D.velocity.y);
                _spriteRenderer.flipX = true;
                _isMoving = true;
                break;
            case EnumMove.Zero:
                _isMoving = false;
                break;
            default:
                Debug.Log("Ошибка передвижения в методе OnMovePhisics");
                break;
        }

        _playerAnimator.isMoving = _isMoving;
    }

    private void CheckGround()
    {
        Ray2D ray = new Ray2D(_transform.position, Vector2.down);
        Debug.DrawRay(ray.origin, ray.direction * _rayLength);

        if(Physics2D.Raycast(ray.origin, ray.direction, _rayLength, _layerMask))
        {
            _isGround = true;
            _isFlying = false;
            _rigidbody2D.gravityScale = _gravityScaleStart;
        }
        else
        {
            _isGround = false;
            _isFlying = true;
            _rigidbody2D.gravityScale = _gravityScaleJump;
        }

        _playerAnimator.isFlying = _isFlying;
    }

    private void OnJump()
    {
        if (_jump == false)
            return;

        if (_jumpTimer <=0 && _isGround == true)
        {
            _jumpTimer = _jumpTime;
            _playerAnimator.OnJumpAnimator(true);
        }

        if (_jumpTimer > 0)
        {
            _jumpTimer -= Time.fixedDeltaTime;
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, Vector2.up.y * _jumpForse * Time.fixedDeltaTime);
            Debug.Log(_jumpTimer);
        }
        else
        {
            _inputPlayer.ResetJumpInput();
            _playerAnimator.OnJumpAnimator(false);
        }
    }
}
