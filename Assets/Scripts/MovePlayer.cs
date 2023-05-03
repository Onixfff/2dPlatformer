using System;
using System.Collections;
using System.Collections.Generic;
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

    private RaycastHit2D _hit;
    private float _gravity = 9.8f;
    private bool _isGround;
    private InputPlayer _inputPlayer;
    private Rigidbody2D _rigidbody2D;
    private Transform _transform;
    private EnumMove _enumMove;
    private bool _jump;
    private float _jumpTimer;

    private void Awake()
    {
        _inputPlayer = GetComponent<InputPlayer>();
        _transform = GetComponent<Transform>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _enumMove = new EnumMove();
    }

    private void FixedUpdate()
    {
        OnMove();
        OnJump();
    }

    private void Update()
    {
        _enumMove = _inputPlayer.GetMoveInput();
        _jump = _inputPlayer.GetJumpInput();
    }

    private void OnMove()
    {
        float sumTimeSpeed = _speed * Time.fixedDeltaTime;

        if(_enumMove != EnumMove.Down && _enumMove != EnumMove.Up && _enumMove != EnumMove.Zero)
            _playerAnimator.OnWalkAnimator(true);

        switch (_enumMove)
        {
            case EnumMove.Right:
                _transform.Translate(Vector2.right * sumTimeSpeed);
                break;
            case EnumMove.Left:
                _transform.Translate(Vector2.left * sumTimeSpeed);
                break;
            case EnumMove.Zero:
                _playerAnimator.OnWalkAnimator(false);
                break;
            default:
                Debug.Log("Ошибка передвижения в методе OnMove");
                break;
        }
    }

    public void OnMovePhisics()
    {
        switch (_enumMove)
        {
            case EnumMove.Right:
                _rigidbody2D.velocity = new Vector2(Vector2.right.x * _speed * Time.fixedDeltaTime, _rigidbody2D.velocity.y);
                break;
            case EnumMove.Left:
                _rigidbody2D.velocity = new Vector2(Vector2.left.x * _speed * Time.fixedDeltaTime, _rigidbody2D.velocity.y);
                break;
            case EnumMove.Zero:
                
                break;
            default:
                Debug.Log("Ошибка передвижения в методе OnMovePhisics");
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            _isGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGround = false;
        }
    }

    private void OnJump()
    {
            Debug.Log(_jump);
        if (_jump == false)
            return;

        if (_jumpTimer <=0 && _isGround == true)
        {
            _jumpTimer = _jumpTime;
            _isGround = false;
        }

        if (_jumpTimer > 0)
        {
            _jumpTimer -= Time.deltaTime;
            _rigidbody2D.AddForce(Vector2.up * _jumpForse * Time.fixedDeltaTime);
            Debug.Log(_jumpTimer);
        }
        else
        {
            _isGround = true;
            _inputPlayer.ResetJumpInput();
        }
    }
}
