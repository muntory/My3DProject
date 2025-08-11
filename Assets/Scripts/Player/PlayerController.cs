using Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput;

    Vector2 moveInput;

    [Header("Player Input")]
    [SerializeField]
    float walkSpeed = 5f;
    [SerializeField]
    float jumpForce = 2f;
    [SerializeField]
    float jumpThreshold = 0.03f;
    [SerializeField]
    float moveThreshold = 0.03f;

    [Header("Camera")]
    [SerializeField]
    Transform cameraArm;
    [SerializeField]
    float lookSensitivityHorizontal = 0.1f;
    [SerializeField]
    float lookSensitivityVertical = 0.01f;
    [SerializeField]
    float minRotX = -40f;
    [SerializeField]
    float maxRotX = 40f;
    
    Rigidbody rb;
    Animator animator;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();

    }


    private void OnEnable()
    {
        playerInput.onActionTriggered += HandleInputActionTriggered;
    }


    private void OnDisable()
    {
        playerInput.onActionTriggered -= HandleInputActionTriggered;
    }


    private void FixedUpdate()
    {
        UpdateMovementInput();
    }


    private void LateUpdate()
    {
        float groundSpeed = moveInput.magnitude;

        animator.SetFloat(AnimatorParam.GroundSpeed, groundSpeed);
        animator.SetFloat(AnimatorParam.VelocityY, rb.velocity.y);

        bool isJumping = animator.GetBool(AnimatorParam.IsJumping);
        bool isFalling = animator.GetBool(AnimatorParam.IsFalling);

        if (isJumping || isFalling)
        {
            if (Mathf.Abs(rb.velocity.y) < jumpThreshold)
            {
                animator.SetBool(AnimatorParam.IsGrounded, true);
                animator.SetBool(AnimatorParam.IsJumping, false);
                animator.SetBool(AnimatorParam.IsFalling, false);
            }

        }
        else
        {
            animator.SetBool(AnimatorParam.IsGrounded, true);
        }

        if (isJumping && rb.velocity.y < 0f || rb.velocity.y < -5f)
        {
            animator.SetBool(AnimatorParam.IsFalling, true);

        }

        if (groundSpeed < moveThreshold)
        {
            animator.SetBool(AnimatorParam.IsMoving, false);
        }
        else
        {
            animator.SetBool(AnimatorParam.IsMoving, true);
        }

        animator.SetFloat(AnimatorParam.Forward, moveInput.y);
        animator.SetFloat(AnimatorParam.Right, moveInput.x);

        
    }


    void UpdateMovementInput()
    {
        Vector3 moveDir = transform.forward * moveInput.y + transform.right * moveInput.x;
        if (moveDir != Vector3.zero)
        {
            rb.MovePosition(rb.position + moveDir * walkSpeed * Time.fixedDeltaTime);
        }
    }


    private void HandleInputActionTriggered(InputAction.CallbackContext context)
    {
        switch (context.action.name)
        {
            case PlayerInputAction.Move:
                OnMove(context);
                break;
            case PlayerInputAction.Look:
                OnLook(context);
                break;
            case PlayerInputAction.Jump:
                OnJump(context);
                break;
        }
    }


    void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }


    void OnLook(InputAction.CallbackContext context)
    {
        Vector2 delta = context.ReadValue<Vector2>();

        // 좌우 회전
        transform.Rotate(Vector3.up * delta.x * lookSensitivityHorizontal);

        // 상하 회전
        float cameraPitch = cameraArm.localEulerAngles.x;
        cameraPitch -= delta.y * lookSensitivityVertical;
        cameraPitch = Mathf.Clamp(cameraPitch, minRotX, maxRotX);

        cameraArm.localEulerAngles = new Vector3(cameraPitch, 0, 0);
    }


    void OnJump(InputAction.CallbackContext context)
    {
        bool isJumping = animator.GetBool(AnimatorParam.IsJumping);
        bool isGrounded = animator.GetBool(AnimatorParam.IsGrounded);

        if (isJumping || !isGrounded)
        {
            return;
        }

        animator.SetBool(AnimatorParam.IsJumping, true);
        animator.SetBool(AnimatorParam.IsGrounded, false);

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        

    }
}
