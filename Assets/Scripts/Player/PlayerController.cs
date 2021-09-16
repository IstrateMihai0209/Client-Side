using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Controller and State variables")]
    private CharacterController controller;

    public enum State
    {
        Regular,
        Vault,
        Sliding,
    }

    private State state;

    [Header("Movement")]
    public float x;
    public float z;
    public bool sprinting;
    public bool vaultState;
    bool spaceKey;
    bool crouch;

    private bool hitCeiling;
    private float standingHeight;
    private float crouchHeight;
    private float defaultStandingHeight = 3f;
    private float defaultCrouchHeight = 1.6f;
    private Vector3 velocity;
    public Vector3 moveDirection = Vector3.zero;
    public float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float sprintingMaxSpeed;
    [SerializeField] private float defaultVelocity;

    [Header("Vaulting")]
    private bool canVault;
    private Vector3 vaultOver;
    [SerializeField] private float vaultSpeed;
    [SerializeField] private Transform vaultCheck;

    [Header("Sliding")]
    private float slideDistance;
    private float slideSpeed;
    [SerializeField] private float defaultSlideSpeed;

    [Header("Air movement variables and checks for collisions with ceilings")]
    private float gravity;
    private float ceilingRadius;
    [SerializeField] private float defaultGravity;
    [SerializeField] private float jumpHeight;
    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private LayerMask ceilingMask;

    [Header("Others")]
    [SerializeField] private GameObject cam;
    public bool moving;

    void Start()
    {
        //Initialize values
        controller = GetComponent<CharacterController>();
        state = State.Regular;
        gravity = defaultGravity;
        speed = maxSpeed;
        standingHeight = defaultStandingHeight;
        crouchHeight = defaultCrouchHeight;
        slideSpeed = defaultSlideSpeed;
        ceilingRadius = 0.4f;

        //DEBUG:
        Debug.LogWarning("Don't forget to improve the vaulting system!");
    }

    void Update()
    {
        switch (state)
        {
            case State.Regular:
                RegularMovement();
                break;

            case State.Vault:
                VaultMovement();
                break;

            case State.Sliding:
                SlidingMovement();
                break;
        }

        SendPlayerPosition(); //Send position information to server
    }

    private void RegularMovement()
    {
        #region Function variables
        x = Input.GetAxis("Horizontal"); //move on the X-axis
        z = Input.GetAxis("Vertical"); //move on the Z-axis

        sprinting = Input.GetKey(KeyCode.LeftShift); //increases the speed variable
        spaceKey = Input.GetKeyDown(KeyCode.Space) || Input.GetAxis("Mouse ScrollWheel") < 0f; //Mouse scrollwheel only for testing purposes
        crouch = Input.GetKey(KeyCode.LeftControl) || hitCeiling; //true when the player presses Control and also when the trigger hits the ceiling

        //A sphere that basically checks when it touches a ceiling (an object from the ceiling layer)
        hitCeiling = Physics.CheckSphere(ceilingCheck.position, ceilingRadius, ceilingMask);
        #endregion

        if (controller.isGrounded)
        {
            //Handles moving and direction
            moveDirection = transform.right * x + transform.forward * z;

            #region Vault Check
            //Check for object in front of player
            if (Physics.Raycast(vaultCheck.position, transform.TransformDirection(Vector3.forward), 2f) && !crouch)
            {
                Debug.DrawRay(vaultCheck.position, transform.TransformDirection(Vector3.forward) * 2f, Color.magenta);

                //Check for an object upwards in front of the player and don't allow vaulting if an object is found
                if (Physics.Raycast(vaultCheck.position + new Vector3(0f, vaultCheck.localPosition.y * 1.7f, 0f), transform.TransformDirection(Vector3.forward), 2f))
                {
                    Debug.DrawRay(vaultCheck.position + new Vector3(0f, vaultCheck.localPosition.y * 1.7f, 0f), transform.TransformDirection(Vector3.forward) * 2f, Color.red);
                    canVault = false;
                }
                else
                {
                    //Allow vaulting and update the position where to vault
                    canVault = true;
                    vaultOver = transform.position + new Vector3(0f, 2.5f, 0f);
                }
            }
            else canVault = false;

            //Change to Vault state
            if (canVault && (x != 0 || z != 0) && spaceKey)
            {
                vaultState = true;
                state = State.Vault;
            }

            #endregion

            #region Handle Crouching and Sliding
            if (sprinting && crouch && speed == sprintingMaxSpeed)
            {
                //If it's at maximum speed it can do a slide
                state = State.Sliding;
            }
            else Crouching(crouch); //Check if it's crouching or not
            #endregion

            #region Sprinting
            if (!crouch)
            {
                //Accelerate only if the player goes forward
                if (sprinting && speed < sprintingMaxSpeed && Input.GetKey(KeyCode.W))
                {
                    speed += 15f * Time.deltaTime;
                    if (speed > sprintingMaxSpeed) speed = sprintingMaxSpeed;
                }
            }
            #endregion

            #region Jumping
            //Jump
            if (spaceKey && !crouch)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * gravity);
            }

            //Reset variables after jumping is over
            if (velocity.y < defaultVelocity)
            {
                velocity.y = defaultVelocity;
                gravity = defaultGravity;
            }
            #endregion

            //For animations
            if (x != 0 || z != 0) moving = true;
            else moving = false;
        }

        if (!controller.isGrounded)
        {
            float strafeVelocity = 0.6f; //Doesn't allow too much strafing
            moving = true;

            //When the player falls from a high altitude, the speed of the falling gets bigger and bigger
            velocity.y += gravity * Time.deltaTime;
            gravity -= 7f * Time.deltaTime;

            //Limit the strafing mid-air
            if ((z != 0f || x != 0f))
                moveDirection += (transform.right * x / strafeVelocity + transform.forward * z / strafeVelocity) * Time.deltaTime;

            //Fall down if hits a ceiling
            if (hitCeiling)
            {
                velocity.y = defaultVelocity;
                gravity = defaultGravity;
            }
        }

        //If it doesn't matter if the player is on ground or not:
        //Stop sprinting if any of the cases are true
        if ((!sprinting || !Input.GetKey(KeyCode.W)) && speed > maxSpeed)
        {
            speed -= 8f * Time.deltaTime;
        }

        #region Moving diagonally
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);
        #endregion

        controller.Move(moveDirection * speed * Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);
    }

    private void VaultMovement()
    {
        Vector3 dir = vaultOver - transform.position; //where to vault
        Crouch(); //It makes vaulting feel better

        //If true it means that the player climbed and it should go back to regular state
        if (Mathf.Ceil(transform.position.y) == Mathf.Ceil(vaultOver.y))
        {
            velocity.y = defaultVelocity;
            vaultState = false;
            state = State.Regular;
        }

        controller.Move(dir * vaultSpeed * Time.deltaTime);
    }

    private void Crouching(bool _crouch)
    {
        if (_crouch)
        {
            speed = maxSpeed / 3f; //Slow down

            Crouch();
        }
        else
        {
            //Start accelerating if necessary
            if (speed < maxSpeed)
            {
                speed += 10f * Time.deltaTime;
                if (speed > maxSpeed) speed = maxSpeed;
            }

            StandUp();
        }
    }

    private void SlidingMovement()
    {
        float duration = 2f;
        hitCeiling = Physics.CheckSphere(ceilingCheck.position, ceilingRadius, ceilingMask);
        Crouch();
        slideDistance += 3f * Time.deltaTime; //used to know when the sliding should be over
        slideSpeed -= 4f * Time.deltaTime; //slow down

        //If sliding is over
        if (slideDistance >= duration)
        {
            //Reset variables and change back to the regular state
            speed = maxSpeed / 8f;
            slideSpeed = defaultSlideSpeed;
            slideDistance = 0f;
            state = State.Regular;
        }

        //Apply gravity even in this state
        velocity.y += gravity * Time.deltaTime;
        gravity -= 7f * Time.deltaTime;

        controller.Move(moveDirection * slideSpeed * Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);
    }

    private void Crouch()
    {
        //Decrease height and camera position smoothly (also the hitCeiling check and changes the center)
        controller.height = Mathf.Lerp(controller.height, crouchHeight, 7f * Time.deltaTime);
        ceilingCheck.transform.localPosition = new Vector3(0f, crouchHeight, 0f);
        cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, new Vector3(0f, crouchHeight, 0f), Time.deltaTime * 7f);
        controller.center = Vector3.Lerp(controller.center, new Vector3(0f, crouchHeight / 2f, 0f), 7f * Time.deltaTime);
    }

    private void StandUp()
    {
        //Increase height and camera position smoothly (also the hitCeiling check and changes the center)
        controller.height = Mathf.Lerp(controller.height, standingHeight, 10f * Time.deltaTime);
        ceilingCheck.transform.localPosition = new Vector3(0f, standingHeight, 0f);
        cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, new Vector3(0f, standingHeight, 0f), Time.deltaTime * 7f);
        controller.center = Vector3.Lerp(controller.center, new Vector3(0f, standingHeight / 2, 0f), 7f * Time.deltaTime);
    }

    private void SendPlayerPosition()
    {
        ClientSend.PlayerPosition(transform.position, transform.rotation);
    }
}