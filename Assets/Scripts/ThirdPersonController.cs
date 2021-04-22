using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour {

    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public float gravity = -9.8f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private bool isGrounded;
    private Vector3 velocity;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public float rememberGroundedFor;
    private float lastTimeGrounded;

    public int defaultAdditionalJumps = 0;
    private int additionalJumps;

    public Animator animator;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        Move();
        Jump();
        BetterJump();
        Gravity();
        CheckIfGrounded();
    }

    void Move() {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        if(direction.magnitude >= 0.1f) {

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);

        }

        animator.SetFloat("hSpeed", direction.magnitude);
    }

    private void CheckIfGrounded() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(isGrounded && velocity.y < 0) {
            velocity.y = -2f;
            lastTimeGrounded = Time.time;
            additionalJumps = defaultAdditionalJumps;

            animator.SetBool("isGrounded", true);
        } else {
            animator.SetBool("isGrounded", false);
        }
    }

    private void Jump() {
        if(Input.GetButtonDown("Jump") && (isGrounded || Time.time - lastTimeGrounded <= rememberGroundedFor || additionalJumps >= 0)) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            additionalJumps--;
        }
    }

    private void BetterJump() {
        if(velocity.y < 0) {
            velocity += Vector3.up * gravity * (fallMultiplier - 1) * Time.deltaTime;
        } else if(velocity.y > 0 && !Input.GetButtonDown("Jump")) {
            velocity += Vector3.up * gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void Gravity() {
        velocity.y += 2f * gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        animator.SetFloat("vSpeed", velocity.y);
    }
}
