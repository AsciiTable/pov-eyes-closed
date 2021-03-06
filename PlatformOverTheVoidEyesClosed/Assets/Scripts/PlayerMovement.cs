using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Transform trans;
    private Transform camTrans;
    private BoxCollider col;
    private PhysicMaterial frictionless;
    [SerializeField] private Vector3 requestedVector = Vector3.zero;
    private Vector3 lookVector = Vector3.zero;
    private Vector3 startVector = Vector3.zero;

    [SerializeField] private float maxMSpeed = 1.0f;
    [SerializeField] private float movementSpeed = 1.0f;
    [SerializeField] private float maxJSpeed = 1.0f;
    [SerializeField] private float jumpSpeed = 1.0f;
    [SerializeField] private float lookSpeed = 1.0f;
    [SerializeField] private float maxVerticalLook = 90.0f;

    // Collision checkers & getters
    [SerializeField]public int groundContacts = 0;
    [SerializeField] private int wallContacts = 0;
    private bool walkingIntoWall = false;
    private bool isCollidingLeft = false;
    private bool isCollidingRight = false;
    private bool isCollidingFront = false;
    private bool isCollidingBack = false;
    public bool IsGrounded { get { return groundContacts > 0; } }
    public bool TouchingWall{ get => wallContacts > 0;
        set { wallContacts = (value ? 1 : -1) + (wallContacts < 0 ? 0 : wallContacts); } }
    public bool IsCollidingLeft { get { return isCollidingLeft; } }
    public bool IsCollidingRight { get { return isCollidingRight; } }
    public bool IsCollidingFront { get { return isCollidingFront; } }
    public bool IsCollidingBack { get { return isCollidingBack; } }
    public bool MouseLookEnabled { get; set; }
    private bool movementEnabled = true; // This is for times when we can't use timescale

    // UI stuff
    [SerializeField] private MenuHandler menuhandling;
    #region Audio Sources
    [SerializeField] private float soundVolume = 1f;
    [SerializeField] private float goalVolume = 1f;
    [SerializeField] private AudioSource clearSFX;
    [SerializeField] private AudioSource deathSFX;
    [SerializeField] private AudioSource jumpSFX;
    [SerializeField] private AudioSource landSFX;
    [SerializeField] private AudioSource landStepSFX;
    [SerializeField] private AudioSource landPlatSFX;
    [SerializeField] private AudioSource wallBumpSFX;

    private float timePassedWalkingIntoWall = 0f;
    #endregion

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        trans = this.GetComponent<Transform>();
        col = GetComponent<BoxCollider>();
        frictionless = col.material;
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
        
            //  Y Movement - Jump
            if (Input.GetButton("Jump") && IsGrounded)
            {
                //Make player's material frictionless before jumping
                col.material = frictionless;

                //if (transform.position.y != lastYPosition) {
                requestedVector.y = jumpSpeed;
                jumpSFX.volume = soundVolume * GameSettingHandler.SFXVolume;
                jumpSFX.Play();
                //groundContacts = 0;
                //}
            }
            else
            {
                requestedVector.y = rb.velocity.y;
                if(IsGrounded)
                    col.material = null;
            }
                

            //requestedVector = new Vector3(Input.GetAxis("Horizontal")*movementSpeed, rb.velocity.y, Input.GetAxis("Vertical")*movementSpeed);
            if (requestedVector != Vector3.zero && movementSpeed != 0)
            {
                rb.velocity = requestedVector;
            }

            //Clamp the x and z axis
            requestedVector = rb.velocity;
            requestedVector.y = 0f;
            if (requestedVector.magnitude > maxMSpeed)
                requestedVector = Vector3.ClampMagnitude(requestedVector, maxMSpeed);
            //Clamp the y axis
            requestedVector.y = (rb.velocity.y > maxJSpeed) ? maxJSpeed : rb.velocity.y;

            rb.velocity = requestedVector;
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
        if (TouchingWall)
        {
            if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            {
                walkingIntoWall = true;
            }
            else
                walkingIntoWall = false;

            if (walkingIntoWall && Time.time - timePassedWalkingIntoWall >= 1f)
            {
                wallBumpSFX.volume = soundVolume * GameSettingHandler.SFXVolume;
                wallBumpSFX.Play();
                timePassedWalkingIntoWall = Time.time;
            }
        }
        else
            walkingIntoWall = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor")) {
            if (!IsGrounded)
            {
                landSFX.volume = soundVolume * GameSettingHandler.SFXVolume;
                landSFX.Play();
            }
            groundContacts += 1;
            Debug.Log("Collided with floor.");
            if(SoundProgression_Manager.singleton != null)
                SoundProgression_Manager.singleton.FallDown();
        }
        if (collision.gameObject.CompareTag("Platform"))
        {
            //Change pitch based on y axis
            int level = Mathf.FloorToInt(collision.transform.position.y) / 2;
            landPlatSFX.pitch = 1f + (level * 0.5f);

            landPlatSFX.volume = soundVolume * GameSettingHandler.SFXVolume;
            landPlatSFX.Play();
            groundContacts += 1;
            Debug.Log("Collided with platform.");
        }
        if (collision.gameObject.CompareTag("Wall")) {
            TouchingWall = true;
            timePassedWalkingIntoWall = Time.time;

            wallBumpSFX.volume = soundVolume * GameSettingHandler.SFXVolume;
            wallBumpSFX.Play();
            Debug.Log("Collided with wall.");
        }
        if (collision.gameObject.CompareTag("DeathZone"))
        {
            deathSFX.volume = soundVolume * GameSettingHandler.SFXVolume;
            deathSFX.Play();
            StartCoroutine(DeathCooldown());
            Debug.Log("Collidied with deathzone");
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Platform"))
        {
            groundContacts -= 1;
            if (groundContacts < 0)
                groundContacts = 0;
        }
        if (collision.gameObject.CompareTag("Wall")) {
            TouchingWall = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Step"))
        {
            if((other.transform.parent.position.y > 0) || !IsGrounded)
            {
                //Change pitch based on y axis
                int level = Mathf.FloorToInt(other.transform.parent.position.y) / 2;
                landStepSFX.pitch = 1f + (level * 0.5f);

                landStepSFX.volume = soundVolume * GameSettingHandler.SFXVolume;
                landStepSFX.Play();
            }
            groundContacts += 1;
            Debug.Log("Collided with step.");


        }
        if (other.gameObject.CompareTag("Goal")){
            menuhandling.OpenGoalPanel();
            clearSFX.volume = goalVolume * GameSettingHandler.BGMVolume;
            clearSFX.Play();
            Debug.Log("Collided with goal trigger.");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Step"))
        {
            groundContacts -= 1;
            if (groundContacts < 0)
                groundContacts = 0;
        }
    }

    IEnumerator DeathCooldown() {
        movementEnabled = false;
        yield return new WaitForSeconds(2);
        movementEnabled = true;
        transform.position = startVector;
    }
}
