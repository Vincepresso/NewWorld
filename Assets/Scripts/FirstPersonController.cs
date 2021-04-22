using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class FirstPersonController : MonoBehaviour
{

    public CharacterController controller;
    public float speed = 3f;
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

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Move();
        Jump();
        BetterJump();
        Gravity();
        CheckIfGrounded();

    }
    private void CheckIfGrounded() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(isGrounded && velocity.y < 0) {
            velocity.y = -2f;
            lastTimeGrounded = Time.time;
            additionalJumps = defaultAdditionalJumps;
        }
    }

    private void Move() {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
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
    }

}
