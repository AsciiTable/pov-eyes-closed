using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Transform trans;
    private Vector3 requestedVector = Vector3.zero;
    private Vector3 lookVector = Vector3.zero;
    private float lastYPosition = 0f;
    
    [SerializeField] private float movementSpeed = 1.0f;
    [SerializeField] private float jumpSpeed = 1.0f;
    [SerializeField] private float lookSpeed = 1.0f;
    [SerializeField] private float maxVerticalLook = 90.0f;

    // Collision checkers & getters
    private bool isGrounded = true;
    private bool isCollidingLeft = false;
    private bool isCollidingRight = false;
    private bool isCollidingFront = false;
    private bool isCollidingBack = false;
    public bool IsGrounded { get { return isGrounded; } }
    public bool IsCollidingLeft { get { return isCollidingLeft; } }
    public bool IsCollidingRight { get { return isCollidingRight; } }
    public bool IsCollidingFront { get { return isCollidingFront; } }
    public bool IsCollidingBack { get { return isCollidingBack; } }
    public bool MouseLookEnabled { get; set; }

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        trans = this.GetComponent<Transform>();
        Cursor.lockState = CursorLockMode.Locked;
        lookVector = rb.rotation.eulerAngles;
        MouseLookEnabled = true;
        lastYPosition = transform.position.y;
    }

    private void OnEnable()
    {
        UpdateHandler.UpdateOccurred += MouseLook;
        UpdateHandler.FixedUpdateOccurred += Movement;
    }

    private void OnDisable()
    {
        UpdateHandler.UpdateOccurred -= MouseLook;
        UpdateHandler.FixedUpdateOccurred -= Movement;
    }

    private void Movement() {
        // X & Z Movement w/ Mouse Rotation
        requestedVector = Input.GetAxis("Horizontal") * movementSpeed * transform.right;
        requestedVector += Input.GetAxis("Vertical") * movementSpeed * transform.forward;
        /*
        requestedVector.y = 0;
        //Check if movement will hit wall
        RaycastHit hit;
        if (rb.SweepTest(requestedVector, out hit, 1.2f))
        {
            // If so, stop the movement
            requestedVector = Vector3.zero;
        }
        */

        requestedVector.y = rb.velocity.y;
        //requestedVector = new Vector3(Input.GetAxis("Horizontal")*movementSpeed, rb.velocity.y, Input.GetAxis("Vertical")*movementSpeed);
        if(requestedVector != Vector3.zero && movementSpeed != 0)
            rb.velocity = requestedVector;

        //  Y Movement - Jump
        if (Input.GetButton("Jump")&&isGrounded)
        {
            rb.AddForce(Vector3.up*jumpSpeed);
            isGrounded = false;
        }

        if (transform.position.y != lastYPosition)
        {
            Debug.Log("Last: " + lastYPosition);
            Debug.Log("This: " + transform.position.y)
            isGrounded = false;
            lastYPosition = trans.position.y;
        }
        
           
        
    }

    private void MouseLook() 
    {
        if (MouseLookEnabled) {
            lookVector.y += Input.GetAxis("Mouse X") * lookSpeed;
            lookVector.x += Input.GetAxis("Mouse Y") * -lookSpeed;
            lookVector.x = Mathf.Clamp(lookVector.x, -maxVerticalLook, maxVerticalLook);
            trans.eulerAngles = lookVector;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor")) {
            isGrounded = true;
            lastYPosition = trans.position.y;
            Debug.Log("Collided with floor.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Step") || other.gameObject.gameObject.CompareTag("Floor")) {
            isGrounded = true;
            lastYPosition = trans.position.y;
            Debug.Log("Collided with floor.");
        }
    }
}
