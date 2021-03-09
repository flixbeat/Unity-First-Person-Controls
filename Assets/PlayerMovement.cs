using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerAudio playerAudio;

    // for movement / controls
    [SerializeField] CharacterController controller;
    private float speed;
    private float baseSpeed;

    // for gravity
    private float gravity;
    private Vector3 velocity;
    private float groundDistance;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    bool isGrounded;

    // jump
    private float jumpHeight;
    private bool isLandSoundPlayed;

    // footsteps
    private float interval;
    private float secPassed;

    // Start is called before the first frame update
    void Start()
    {
        playerAudio = GetComponent<PlayerAudio>();
        baseSpeed = 3;
        gravity = -20f;
        groundDistance = 0.4f;
        jumpHeight = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        // create a small sphere collider on the bottom area of the player controller
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // disable land sound
        if(isGrounded == false) isLandSoundPlayed = false;

        // check if grounded
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // implement gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // store movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // move relative to the camera
        Vector3 movement = (transform.right * x) + (transform.forward * Mathf.Clamp(z, -0.5f, 1.0f));

        // implemnent run (forward only)
        speed = Input.GetAxis("Sprint") * 5 + baseSpeed;

        // implement crouch
        controller.height = 2 - Input.GetAxis("Crouch");
        groundCheck.localPosition = new Vector3(0, -1 + (Input.GetAxis("Crouch") / 2), 0f);

        if (Input.GetAxis("Crouch") > 0)
        {
            isLandSoundPlayed = true;
            speed = baseSpeed - 1.5f;
        }

        // implement movement
        controller.Move(movement * speed * Time.deltaTime);

        // implement jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            StartCoroutine(DisableLandSoundFlag());
        }

        // land sound
        if (isGrounded && !isLandSoundPlayed)
        {
            playerAudio.PlayLandSound();
            isLandSoundPlayed = true;
        }

        // check if moving
        bool isMoving = (x > 0.5f || x < -0.5f) || (z > 0.5f || z < -0.5f);
        if (isMoving && isGrounded) PlayFootStepSound();

    }

    // disable sound flag when jumped
    IEnumerator DisableLandSoundFlag()
    {
        yield return new WaitForSeconds(.2f);
        isLandSoundPlayed = false;
    }

    // method to play footstep sound
    void PlayFootStepSound()
    {
        // increase seconds passed
        secPassed += Time.deltaTime;

        // footstep for sprinting
        if (Input.GetAxis("Sprint") > 0.7f) interval = 0.4f;
        else interval = 0.6f; // when walking

        // when crouching
        if (Input.GetAxis("Crouch") > 0.7f) interval = 0.8f;

        // play footstep sound
        if (secPassed > interval)
        {
            playerAudio.PlayFootStepSound(Input.GetAxis("Crouch"));
            secPassed = 0f;
        }
    }
}
