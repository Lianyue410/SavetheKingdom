using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    [Header("Refs")]
    [Tooltip("Main Camera")]
    public Transform cameraTransform;

    [Tooltip("character-female-e")]
    public Transform visualRoot;

    [Header("Move")]
    public float walkSpeed = 4.5f;
    public float runSpeed = 7.5f;          
    public float acceleration = 12f;     
    public float rotateSpeed = 720f;      
    public float gravity = -9.81f;

    [Header("Ground")]
    public float groundedStick = -2f;      

    [Header("Fake Walk Animation")]
    [Tooltip("The range of left-right oscillation")]
    public float swayAngle = 8f;
    [Tooltip("The range of up-and-down movement")]
    public float bobHeight = 0.04f;
    [Tooltip("Walking frequency")]
    public float walkBobFreq = 8f;
    [Tooltip("Running frequency")]
    public float runBobFreq = 12f;
    [Tooltip("Reversal speed")]
    public float animReturnSpeed = 18f;

    private CharacterController controller;
    private Vector3 velocity;            
    private Vector3 currentMove;         

    private Vector3 visualBaseLocalPos;
    private Quaternion visualBaseLocalRot;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        if (visualRoot == null)
        {
            if (transform.childCount > 0) visualRoot = transform.GetChild(0);
        }

        if (visualRoot != null)
        {
            visualBaseLocalPos = visualRoot.localPosition;
            visualBaseLocalRot = visualRoot.localRotation;
        }
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(x, 0f, z);
        input = Vector3.ClampMagnitude(input, 1f);

        Vector3 camForward = Vector3.forward;
        Vector3 camRight = Vector3.right;

        if (cameraTransform != null)
        {
            camForward = cameraTransform.forward;
            camRight = cameraTransform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();
        }

        Vector3 desiredDir = camForward * input.z + camRight * input.x;
        desiredDir = Vector3.ClampMagnitude(desiredDir, 1f);

        // Press Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float targetSpeed = isRunning ? runSpeed : walkSpeed;

        Vector3 desiredMove = desiredDir * targetSpeed;

        currentMove = Vector3.Lerp(currentMove, desiredMove, 1f - Mathf.Exp(-acceleration * Time.deltaTime));

        if (desiredDir.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(desiredDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
        }

        // Gravity
        if (controller.isGrounded && velocity.y < 0f)
            velocity.y = groundedStick;

        velocity.y += gravity * Time.deltaTime;

        Vector3 finalMove = currentMove + Vector3.up * velocity.y;
        controller.Move(finalMove * Time.deltaTime);

        UpdateFakeAnimation(currentMove, isRunning);
    }

    private void UpdateFakeAnimation(Vector3 move, bool isRunning)
    {
        if (visualRoot == null) return;

        Vector3 horizontal = new Vector3(move.x, 0f, move.z);
        float speed01 = Mathf.Clamp01(horizontal.magnitude / Mathf.Max(0.01f, runSpeed));

        bool moving = speed01 > 0.08f;

        if (moving)
        {
            float freq = isRunning ? runBobFreq : walkBobFreq;
            float t = Time.time * freq;

            // Up and down
            float bob = Mathf.Abs(Mathf.Sin(t)) * bobHeight * Mathf.Lerp(0.6f, 1f, speed01);

            // Left and right
            float sway = Mathf.Sin(t) * swayAngle * Mathf.Lerp(0.6f, 1f, speed01);

            Vector3 targetPos = visualBaseLocalPos + new Vector3(0f, bob, 0f);
            Quaternion targetRot = visualBaseLocalRot * Quaternion.Euler(0f, 0f, sway);

            visualRoot.localPosition = Vector3.Lerp(
                visualRoot.localPosition,
                targetPos,
                1f - Mathf.Exp(-animReturnSpeed * Time.deltaTime)
            );

            visualRoot.localRotation = Quaternion.Slerp(
                visualRoot.localRotation,
                targetRot,
                1f - Mathf.Exp(-animReturnSpeed * Time.deltaTime)
            );
        }
        else
        {
            // Return to normal
            visualRoot.localPosition = Vector3.Lerp(
                visualRoot.localPosition,
                visualBaseLocalPos,
                1f - Mathf.Exp(-animReturnSpeed * Time.deltaTime)
            );

            visualRoot.localRotation = Quaternion.Slerp(
                visualRoot.localRotation,
                visualBaseLocalRot,
                1f - Mathf.Exp(-animReturnSpeed * Time.deltaTime)
            );
        }
    }
}