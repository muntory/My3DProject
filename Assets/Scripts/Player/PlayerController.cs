using Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput;

    Vector2 moveInput;

    PlayerStat stat;
    PlayerCharacter playerCharacter;

    [Header("Player Input")]
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

    float cameraPitch;

    const float DETECTION_INTERVAL = 0.1f;
    float lastDetectionTime;
    public float detectionDistance = 5f;
    public LayerMask interactableLayer;
    public Transform raycastOrigin;
    IInteractable currentInteractable;

    Rigidbody rb;
    Animator animator;

    bool isJumping;
    bool isFalling;
    bool isGrounded;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        stat = GetComponent<PlayerStat>();
        playerCharacter = GetComponent<PlayerCharacter>();

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();
        cameraPitch = cameraArm.localEulerAngles.x;

        if (raycastOrigin == null)
        {
            raycastOrigin = cameraArm;
        }

        isJumping = false;
        isFalling = false;
        isGrounded = false;
    }


    private void OnEnable()
    {
        playerInput.onActionTriggered += HandleInputActionTriggered;
    }


    private void OnDisable()
    {
        playerInput.onActionTriggered -= HandleInputActionTriggered;
    }

    private void Update()
    {
        if (Time.time - lastDetectionTime > DETECTION_INTERVAL)
        {
            DetectInteractable();
            lastDetectionTime = Time.time;
        }

       
        
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

        DetectGround();

        if (isJumping && isFalling)
        {
            if (isGrounded)
            {
                isJumping = false;
                animator.SetBool(AnimatorParam.IsJumping, false);

            }
        }

        isFalling = !isGrounded && rb.velocity.y < 0f;
        animator.SetBool(AnimatorParam.IsFalling, isFalling);


        

        //if (isJumping || isFalling)
        //{

        //    if (Mathf.Abs(rb.velocity.y) < jumpThreshold)
        //    {
        //        animator.SetBool(AnimatorParam.IsGrounded, true);
        //        animator.SetBool(AnimatorParam.IsJumping, false);
        //        animator.SetBool(AnimatorParam.IsFalling, false);
        //    }

        //}
        //else
        //{
        //    animator.SetBool(AnimatorParam.IsGrounded, true);
        //}

        //if (isJumping && rb.velocity.y < 0f || rb.velocity.y < -5f)
        //{
        //    animator.SetBool(AnimatorParam.IsFalling, true);

        //}

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

    void Jump(float jumpForce)
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isJumping = true;
        animator.SetBool(AnimatorParam.IsJumping, true);

    }

    public void JumpByOther(float jumpForce)
    {
        Jump(jumpForce);

    }


    void DetectInteractable()
    {
        Vector3 origin = raycastOrigin.position;
        Vector3 direction = raycastOrigin.forward;

        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, detectionDistance, interactableLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            currentInteractable = interactable;

            if (!UIManager.Instance.IsActive<InteractPopup>())
            {
                UIManager.Instance.ShowPopup<InteractPopup>().SetInteractable(interactable);
            }

        }
        else
        {
            currentInteractable = null;
            if (UIManager.Instance.IsActive<InteractPopup>())
            {
                UIManager.Instance.ClosePopupUI();
            }
        }
        Debug.DrawRay(origin, direction * detectionDistance,
                     currentInteractable != null ? Color.green : Color.red);
    }

    void DetectGround()
    {
        Vector3 origin = transform.position + Vector3.up;
        Vector3 direction = Vector3.down;
        LayerMask layermask = LayerMask.GetMask("Ground");

        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, 1.1f, layermask))
        {
            isGrounded = true;
            
        }
        else
        {
            isGrounded = false;
        }
        animator.SetBool(AnimatorParam.IsGrounded, isGrounded);

        Debug.DrawRay(origin, direction * 1.1f,
                     isGrounded ? Color.green : Color.red);
    }


    void UpdateMovementInput()
    {
        Vector3 moveDir = transform.forward * moveInput.y + transform.right * moveInput.x;
        if (moveDir != Vector3.zero)
        {
            rb.MovePosition(rb.position + moveDir * stat.WalkSpeed * Time.fixedDeltaTime);
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
            case PlayerInputAction.Interact:
                OnInteract(context);
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
        cameraPitch -= delta.y * lookSensitivityVertical;
        cameraPitch = Mathf.Clamp(cameraPitch, minRotX, maxRotX);

        cameraArm.localEulerAngles = new Vector3(cameraPitch, 0, 0);
    }


    void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (isJumping || !isGrounded)
        {
            return;
        }

        Jump(jumpForce);
    }


    void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (currentInteractable == null) return;

        currentInteractable.OnInteract(playerCharacter);
    }
}
