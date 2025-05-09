using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _sprintMult;
    [SerializeField] private float _JumpHeight;
    [SerializeField] private float _jumpCooldown;

    [Header("Attack Settings")]
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _attackCooldown;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    [Header("Ground Distance")]
    [SerializeField] private float groundDistance;

    private bool _canMove, _canDash, _canJump;
    private Vector2 _move;
    private CharacterController charController;
    private Animator _animator;
    private bool _isDashing;
    private float _verticalVelocity, _currentSpeed;

    public float MovementSpeed { get => _movementSpeed; set => _movementSpeed = value; }
    public Animator PlayerAnimator { get => _animator; set => _animator = value; }
    public CharacterController ControllerRigidBodyComponent { get => charController; set => charController = value; }

    private void Start()
    {
        _canMove = _canJump = _canDash = true;
        _currentSpeed = MovementSpeed;
        charController = GetComponent<CharacterController>();
        PlayerAnimator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (!_isDashing)
            MoveInDirection(_currentSpeed);
        // gravity
        if (charController.isGrounded && _verticalVelocity < 0f)
            _verticalVelocity = -2f;

        _verticalVelocity += Physics.gravity.y * 1.5f * Time.deltaTime;      
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _move = context.ReadValue<Vector2>();
    }
    public void OnRunning(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _currentSpeed = MovementSpeed * _sprintMult;
        }else if (context.canceled)
        {
            _currentSpeed = MovementSpeed;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && charController.isGrounded && _canJump)
            StartCoroutine(JumpCorountine());
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!context.performed || !_canDash) return;
        StartCoroutine(DashCoroutine());
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            print("Attack");
    }

    private void MoveInDirection(float movementSpeed)
    {
        // build horizontal move
        Vector3 horizontal = new Vector3(_move.x, 0, _move.y).normalized * movementSpeed;
        // include vertical velocity
        Vector3 finalMove = horizontal + Vector3.up * _verticalVelocity;
        charController.Move(finalMove * Time.deltaTime);

        // rotate to face movement whitout vertical movement
        Vector3 newDirection = new Vector3(finalMove.x, 0, finalMove.z);
        FaceDirection(newDirection);
    }

    private void FaceDirection(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0.1f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.15f);
    }

    IEnumerator DashCoroutine()
    {
        _canDash = false;
        _isDashing = true;
        float timer = 0f;

        while (timer < dashDuration)
        {
            charController.Move(transform.forward * dashSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        _isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        _canDash = true;
    }

    IEnumerator MoveWaitTime()
    {
        _canMove = false;
        yield return new WaitForSeconds(.2f);
        _canMove = true;
    }

    IEnumerator JumpCorountine()
    {
        _canJump = false;

        _verticalVelocity = Mathf.Sqrt(_JumpHeight * -2f * Physics.gravity.y);
        yield return new WaitForSeconds(_jumpCooldown);

        _canJump = true;
    }
}
