// Handles the player movement and camera in both FirstPerson and RTS modes.
// ==================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLYMovement : MonoBehaviour {

    public bool fPersonMode, rtsMode;
    public KeyCode sprintKey = KeyCode.LeftShift, jumpKey = KeyCode.Space, rtsRotLeft = KeyCode.Q, rtsRotRight = KeyCode.E;
    public float 
        moveSpeed = 5, 
        xSpeed = 500, 
        ySpeed = 400, 
        yUpperLimit = 45, 
        yLowerLimit = -60f, 
        jumpSpeed = 10, 
        gravStrength = 25;
    public GameObject plyCam;

    CharacterController charControl;
    Vector3 moveDir = Vector3.zero;
    float speed = 0, rotX = 0, rotY = 0;

    // ==================================================
    void Start ()
    {
        if (fPersonMode) { charControl = GetComponent<CharacterController>(); }
    }

    // ==================================================
    void Update ()
    {
        if (Input.GetKey(sprintKey)) { speed = moveSpeed * 2; } else { speed = moveSpeed; }
        if (fPersonMode) { FirstPerson(); }
        if (rtsMode) { RTS(); }
    }

    // ==================================================
    void FirstPerson()
    {
        // Set mouse mode.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Calculate look direction.
        rotX += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
        rotY += Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
        rotY = Mathf.Clamp(rotY, yLowerLimit, yUpperLimit);

        // Apply to camera & body.
        plyCam.transform.localEulerAngles = new Vector3(-rotY, 0, 0);
        transform.eulerAngles = new Vector3(0, rotX, 0);

        // Calculate movement.
        if (charControl.isGrounded)
        {
            moveDir = transform.TransformDirection( new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical") ) * speed);
            if (Input.GetKey(jumpKey)) { moveDir.y = jumpSpeed; }
        }
        moveDir.y -= gravStrength * Time.deltaTime;

        // Apply movement over time.
        charControl.Move(moveDir * Time.deltaTime);
    }

    // ==================================================
    void RTS()
    {
        // Set mouse mode.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Calculate movement over time.
        moveDir += transform.right * (Input.GetAxis("Horizontal") * speed * Time.deltaTime);
        moveDir += transform.forward * (Input.GetAxis("Vertical") * speed * Time.deltaTime);

        // Apply movement.
        transform.position = moveDir;

        // Calculate rotation & apply over time.
        if (Input.GetKey(rtsRotLeft)) { transform.Rotate(0, -ySpeed * Time.deltaTime, 0); }
        if (Input.GetKey(rtsRotRight)) { transform.Rotate(0, ySpeed * Time.deltaTime, 0); }
    }
}
