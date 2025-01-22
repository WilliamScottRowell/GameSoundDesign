using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    // Movement variables
    public float mouseSensitivity = 500f;
    public float speed = 5f;

    public Transform playerBody;
    public CharacterController controller;

    // Gravity variables
    public Vector3 velocity;
    public float gravity = -9.81f;
    private float groundDistance = 0.1f; // Small buffer for ground detection

    // Jumping variables
    public float jumpHeight = 1.5f;
    private bool isGrounded;

    // Audio
    public AudioSource jumpSound;

    void Update()
    {
        ProcessGroundCheck(); // Check if player is on the ground
        PlayerMover(); // Handle movement
        ProcessJumping(); // Handle jumping
        ApplyGravity(); // Handle gravity
    }

    void PlayerMover()
    {
        // Turn player based on mouse movement
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        playerBody.Rotate(Vector3.up * mouseX);

        // Move player based on keyboard input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
    }

    void ApplyGravity()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small force to keep grounded detection stable
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void ProcessJumping()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isGrounded = false; // Temporarily set to false to prevent double jumps

            if (jumpSound != null)
            {
                jumpSound.Play();
            }
        }
    }

    void ProcessGroundCheck()
    {
        isGrounded = controller.isGrounded; // Use built-in Unity function

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Ensures stick to ground
        }
    }
}
