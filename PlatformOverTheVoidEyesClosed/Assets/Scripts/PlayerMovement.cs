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

    [SerializeField]private float maxMSpeed = 1.0f;
    [SerializeField] private float movementSpeed = 1.0f;
    [SerializeField] private float maxJSpeed = 1.0f;
    [SerializeField] private float jumpSpeed = 1.0f;
    [SerializeField] private float lookSpeed = 1.0f;
    [SerializeField] private float maxVerticalLook = 90.0f;

    // Collision checkers & getters
    [SerializeField]private int groundContacts = 0;
    private bool isCollidingLeft = false;
    private bool isCollidingRight = false;
    private bool isCollidingFront = false;
    private bool isCollidingBack = false;
    public bool IsGrounded { get { return groundContacts > 0; } }
    public bool IsCollidingLeft { get { return isCollidingLeft; } }
    public bool IsCollidingRight { get { return isCollidingRight; } }
    public bool IsCollidingFront { get { return isCollidingFront; } }
    public bool IsCollidingBack { get { return isCollidingBack; } }
    public bool MouseLookEnabled { get; set; }

    // UI stuff
    [SerializeField] private MenuHandler menuhandling;

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
    }

    private void OnDisable()
    {
        UpdateHandler.UpdateOccurred -= MouseLook;
        UpdateHandler.FixedUpdateOccurred -= Movement;
    }

    private void Movement() {
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

        //  Y Movement - Jump
        if (Input.GetButton("Jump") && IsGrounded)
        {
            //if (transform.position.y != lastYPosition) {
            requestedVector.y = jumpSpeed;
            //groundContacts = 0;
            //}
        }
        else
           requestedVector.y = rb.velocity.y;

        //requestedVector = new Vector3(Input.GetAxis("Horizontal")*movementSpeed, rb.velocity.y, Input.GetAxis("Vertical")*movementSpeed);
        if (requestedVector != Vector3.zero && movementSpeed != 0) {
            rb.velocity = requestedVector;
        }

        //Clamp the x and z axis
        requestedVector = rb.velocity;
        requestedVector.y = 0f;
        if (requestedVector.magnitude > maxMSpeed)
            requestedVector = Vector3.ClampMagnitude(requestedVector, maxMSpeed);
        //Clamp the y axis
        requestedVector.y = (rb.velocity.y > maxJSpeed) ? maxJSpeed : rb.velocity.y;

        if(requestedVector != rb.velocity)
            rb.velocity = requestedVector;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor")) {
            groundContacts += 1;
            Debug.Log("Collided with floor.");

            //Reset SoundProgression if incomplete
            if(SoundProgression_Manager.singleton != null && SoundProgression_Manager.singleton.IsFinished)
                SoundProgression_Manager.singleton.ResetProgress();
        }
        if (collision.gameObject.CompareTag("Step")) {
            groundContacts += 1;
            Debug.Log("Collided with step.");
        }
        if (collision.gameObject.CompareTag("DeathZone"))
        {
            // Play death sound
            transform.position = startVector;
            Debug.Log("Collidied with deathzone");
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Step"))
        {
            groundContacts -= 1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Goal")){
            menuhandling.OpenGoalPanel();
            Debug.Log("Collided with goal trigger.");
        }
    }
}
