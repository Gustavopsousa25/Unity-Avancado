using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [Header("Movement Settings")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _JumpHeight;
    [SerializeField] private float _jumpCooldown;

    [Header("Dash")]
    [SerializeField] float dashSpeed = 20f;
    [SerializeField] float dashDuration = 0.2f;
    [SerializeField] float dashCooldown = 1f;

    [Header("Ground Distance")]
    [SerializeField] private float groundDistance;

    private bool _canMove, _canDash, _canJump;
    private Vector2 _move;
    private Rigidbody _rigidBody;
    private Animator _animator;
    private bool _jump, _isGrounded;
    private bool _isDashing, _isJumping;
    

    public float MovementSpeed { get => _movementSpeed; set => _movementSpeed = value; }
    public Animator PlayerAnimator { get => _animator; set => _animator = value; }
    public Rigidbody ControllerRigidBody { get => _rigidBody; set => _rigidBody = value; }

    private void Start()
    {
        _canMove = true;
        _canJump = true;
        _canDash = true;
        ControllerRigidBody = GetComponent<Rigidbody>();
        PlayerAnimator = GetComponentInChildren<Animator>();
        
    }
    private void Update()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * groundDistance, Color.red);

        /*FaceDirection();       
        if (_canMove != true)
        {
            ControllerRigidBody.velocity = Vector3.zero;
           //PlayerAnimator.SetBool("isRuning", false);
        }*/
    
    }
    private void FixedUpdate()
    {
        if (!_isDashing && !_isJumping)
        MoveInDirection();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        _move = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, groundDistance);     
        if (_isGrounded && _canJump)
        {
            StartCoroutine(JumpCorountine());
        }
    }
    public void OnDash(InputAction.CallbackContext context)
    {
        if (!context.performed || !_canDash) return;
        StartCoroutine(DashCoroutine());
    }
    private void MoveInDirection()
    {
        /*if (direction == Vector2.zero)
        {
          // PlayerAnimator.SetBool("isRuning", false);
        }
        else
        {
           // PlayerAnimator.SetBool("isRuning", true);
        }*/
        Vector3 finalVelocity = (_move.x * Vector3.right + _move.y * Vector3.forward).normalized * MovementSpeed;
        finalVelocity.y = ControllerRigidBody.velocity.y;
        ControllerRigidBody.velocity = finalVelocity;
        Vector3 newDirection = new Vector3(finalVelocity.x, 0, finalVelocity.z);
        FaceDirection(newDirection);
    }
    private void FaceDirection(Vector3 direction)
    {
        if(direction.sqrMagnitude > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.15f);
        }      
    }

    IEnumerator DashCoroutine()
    {
        _canDash = false;
        _isDashing = true;
        float timer = 0f;

        while (timer < dashDuration)
        {
            ControllerRigidBody.velocity = transform.forward * dashSpeed;
            timer += Time.deltaTime;
            yield return null;
        }

        _isDashing = false;

        Vector3 newVelocity = ControllerRigidBody.velocity;
        newVelocity.x = newVelocity.y = 0;
        ControllerRigidBody.velocity = newVelocity;

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
        _isJumping = true;
        ControllerRigidBody.velocity = Vector3.zero;
        ControllerRigidBody.AddForce(Vector2.up * _JumpHeight, ForceMode.Impulse);
        _isJumping = false;

        yield return new WaitForSeconds(_jumpCooldown);
        _canJump = true;
    }
    private void OnCollisionEnter(Collision other)
    {
        CapsuleCollider myCapsuleCollider = GetComponent<CapsuleCollider>();

        Vector3 ContactPoint = other.contacts[0].point;
        ContactPoint.y = 0.0f;

        Vector3 CapsuleLocation = myCapsuleCollider.transform.position;
        CapsuleLocation.y = 0.0f;

        if (Vector3.Distance(ContactPoint, CapsuleLocation) > myCapsuleCollider.radius)
        {
            Vector3 Velocity = ControllerRigidBody.velocity;

            Velocity.y = 0.0f;
            Velocity.y = -6f;

            ControllerRigidBody.velocity = Velocity;
        }
    }
}
