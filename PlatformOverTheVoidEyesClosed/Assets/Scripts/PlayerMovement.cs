using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 velocityVector = Vector3.zero;
    private Vector3 requestedVector = Vector3.zero;
    [SerializeField] private float movementSpeed = 1.0f;
    [SerializeField] private float jumpSpeed = 1.0f;

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

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        UpdateHandler.FixedUpdateOccurred += Movement;
    }

    private void OnDisable()
    {
        UpdateHandler.FixedUpdateOccurred -= Movement;
    }

    private void Movement() {
        // Get player input
        requestedVector = new Vector3(Input.GetAxis("Horizontal")*movementSpeed, rb.velocity.y, Input.GetAxis("Vertical")*movementSpeed);
        // Move with CharacterController 
        if(requestedVector != Vector3.zero && movementSpeed != 0)
            rb.velocity = (requestedVector);
        // If player wants to Jump
        if (Input.GetButton("Jump")&&isGrounded)
        {
            rb.AddForce(Vector3.up*jumpSpeed);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor")) {
            isGrounded = true;
            Debug.Log("Collided with floor.");
        }
    }
}
