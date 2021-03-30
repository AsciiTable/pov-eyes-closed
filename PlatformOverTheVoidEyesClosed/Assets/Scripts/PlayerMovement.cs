using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Transform trans;
    private Transform camTrans;
    private Vector3 requestedVector = Vector3.zero;
    private Vector3 lookVector = Vector3.zero;
    private Vector3 startVector = Vector3.zero;

    [SerializeField]private float maxSpeed = 10.0f;
    [SerializeField] private float movementSpeed = 1.0f;
    [SerializeField] private float jumpSpeed = 1.0f;
    [SerializeField] private float lookSpeed = 1.0f;
    [SerializeField] private float maxVerticalLook = 90.0f;

    // Collision checkers & getters
    [SerializeField]public bool isGrounded = true;
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
    private bool movementEnabled = true; // This is for times when we can't use timescale

    // UI stuff
    [SerializeField] private MenuHandler menuhandling;
    #region Audio Sources
    [SerializeField] private AudioSource clearSFX;
    [SerializeField] private AudioSource deathSFX;
    [SerializeField] private AudioSource jumpSFX;
    [SerializeField] private AudioSource landSFX;
    [SerializeField] private AudioSource wallBumpSFX;
    private bool walkingIntoWall = false;
    private float timePassedWalkingIntoWall = 0f;
    #endregion

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        trans = this.GetComponent<Transform>();
        camTrans = GameObject.FindGameObjectWithTag("MainCamera").transform;
        Cursor.lockState = CursorLockMode.Locked;
        lookVector = rb.rotation.eulerAngles;
        MouseLookEnabled = true;
        startVector = transform.position;
    }

    private void OnEnable()
    {
        UpdateHandler.UpdateOccurred += MouseLook;
        UpdateHandler.FixedUpdateOccurred += Movement;
        UpdateHandler.FixedUpdateOccurred += CheckContinuousCollisions;
    }

    private void OnDisable()
    {
        UpdateHandler.UpdateOccurred -= MouseLook;
        UpdateHandler.FixedUpdateOccurred -= Movement;
        UpdateHandler.FixedUpdateOccurred -= CheckContinuousCollisions;
    }

    private void Movement() {
        if (movementEnabled) {
            // X & Z Movement w/ Mouse Rotation
            requestedVector = Input.GetAxis("Horizontal") * movementSpeed * camTrans.right;
            requestedVector += Input.GetAxis("Vertical") * movementSpeed * camTrans.forward;
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
            if (requestedVector != Vector3.zero && movementSpeed != 0)
            {
                rb.velocity = requestedVector;
            }

            //  Y Movement - Jump
            if (Input.GetButton("Jump") && isGrounded)
            {
                //if (transform.position.y != lastYPosition) {
                rb.AddForce(Vector3.up * jumpSpeed);
                isGrounded = false;
                //}
                jumpSFX.Play();
            }
            if (rb.velocity.magnitude > maxSpeed)
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
    }

    private void MouseLook() 
    {
        if (MouseLookEnabled) {
            lookVector.y += Input.GetAxis("Mouse X") * lookSpeed;
            lookVector.x += Input.GetAxis("Mouse Y") * -lookSpeed;
            lookVector.x = Mathf.Clamp(lookVector.x, -maxVerticalLook, maxVerticalLook);
            camTrans.eulerAngles = lookVector;
        }
    }

    private void CheckContinuousCollisions() {
        if (walkingIntoWall) {
            if (Time.time - timePassedWalkingIntoWall >= 1f) {
                wallBumpSFX.Play();
                timePassedWalkingIntoWall = Time.time;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor")) {
            landSFX.Play();
            isGrounded = true;
            Debug.Log("Collided with floor.");
        }
        if (collision.gameObject.CompareTag("Step")) {
            landSFX.Play();
            isGrounded = true;
            Debug.Log("Collided with step.");
        }
        if (collision.gameObject.CompareTag("Wall")) {
            walkingIntoWall = true;
            timePassedWalkingIntoWall = Time.time;
            wallBumpSFX.Play();
            Debug.Log("Collided with wall.");
        }
        if (collision.gameObject.CompareTag("DeathZone"))
        {
            deathSFX.Play();
            StartCoroutine(DeathCooldown());
            Debug.Log("Collidied with deathzone");
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Step"))
        {
            isGrounded = false;
        }
        if (collision.gameObject.CompareTag("Wall")) {
            walkingIntoWall = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Goal")){
            menuhandling.OpenGoalPanel();
            clearSFX.Play();
            Debug.Log("Collided with goal trigger.");
        }
    }

    IEnumerator DeathCooldown() {
        movementEnabled = false;
        yield return new WaitForSeconds(2);
        movementEnabled = true;
        transform.position = startVector;
    }
}
