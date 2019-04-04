using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GeneralPlayerScript : MonoBehaviour
{
    public Camera Camera; //reference to the camera
    public CharacterController playercon; // reference to the player character
    public GameObject player; //reference to the player's gameobject(used to destroy it later)
    public float xrotation; // speed multiplier for x axis rotation
    public float forwards = 7f;// speed multiplier for moving forwards
    public float backwards = -6f;// speed multiplier for moving backwards
    public float strafe = 7f;// speed multiplier for moving	left or right
    public bool isGrounded = false;//useless bool value that I can't get to function, not really needed, but it would be help to figure out any issues with movement/gravity
    public bool running;
    public float sensitivity = 1f;
    public float yrotation;
    public float jumpHeight = 0f;// base height for jumping, to be used in either a move function, or a lerped translate
    public float speed = 10f;//base speed multiplier for player movement
    public float gravity = 9.8f;//base speed for gravity
    private Vector3 move = Vector3.zero;//resets motion to zero
    public CapsuleCollider playercollider;// reference to a capsule collider on the player if I ever decide to add one for calculating forces and stuff
    
   void Update () // Update is called once per frame
    {
        
        // sets the float y to use the same value as yrotation and then multiplies that by the movement of the y axis of the mouse
        xrotation += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;// sets the float x to use the same value as xrotation and then multiplies that by the movement of the x axis of the mouse
        move = new Vector3(Input.GetAxis("Strafe") * speed, 0, Input.GetAxis("Forwards") * speed); //creates a new Vector3 for the values of movement
        move = transform.TransformDirection(move); //this basically isolates the movement to axes relative to the player instead of world axes
        yrotation += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        yrotation = Mathf.Clamp(yrotation, -80f, 90f);

        move.y = move.y + (jumpHeight - gravity);//moves the player down by gravity multiplied by Time.deltaTime

        Camera.transform.rotation = Quaternion.Euler(-yrotation, xrotation, 0f);//rotates vertical rotation of the camera but not the character
        playercon.transform.rotation = Quaternion.Euler(0f, xrotation, 0f);// rotates the character and the camera horizontally, because the camera is a child of the character
        playercon.Move(move * Time.deltaTime * speed);// Moves the player. Movement is equal to base move speed * Time.deltaTime * the speed multiplier 
        Cursor.lockState = CursorLockMode.Locked;//locks the cursor
        Cursor.visible = false;//hides the cursor
        if (Input.GetKey(KeyCode.LeftShift) && playercon.isGrounded == true)//if left shift is pressed and player is grounded
        {
            running = true;//running
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))//if you release the left shift key
        {
            running = false;//not running
        }
        if (running == false)//if not running
        {
            speed = 10f; //sets speed to 9
        }
        if (running == true)//if running
        {
            speed = 25f; //sets speed to 15
        }
       
        if (playercon.isGrounded == true)//if the player is resting on a surface with a collider
        {
            jumpHeight = 0f;// set   jump height to 0
            StopCoroutine("gravitySpeed");//stops adding speed to gravity when you touch the ground
            gravity = 9.8f;// resets gravity to 9.8 u/s 
            if (Input.GetKey(KeyCode.Space))// if you press space while grounded
            {
                Debug.Log("jump attempted");//console message telling me when I attempt to jump
                StartCoroutine("jumping");//starts the coroutine for jumping
            }
        }
        if (playercon.isGrounded == false)//if the player is not touching a surface with collision
        {
            StartCoroutine("gravitySpeed");//starts increasing gravity exponentially
        }
        if (Input.GetKey("escape"))//if you press the escape key
        {
            Application.Quit();// quits the game
        }
    }
    private void LateUpdate()
    {
        
    }
    private IEnumerator gravitySpeed()
    {
        yield return new WaitForSecondsRealtime(0.0167f);//waits for 60th of a second realtime
        gravity = gravity + (0.016666f * gravity);// adds ((change in time since last frame update)+gravity) to gravity every 60th of a second for as long as the player is not grounded
    }
    private IEnumerator jumping()
    {
        gravity = 4f;//sets gravity to 0 at the start of the jump
        jumpHeight = 9.8f;//sets jumpHeight to 10
        yield return new WaitForSecondsRealtime(0.1f);//waits for a quarter of a second realtime
        jumpHeight = 13;//sets jumpHeight to 9.9
        gravity = 9.8f; //sets gravity to 1 for a slower decline
    }
    private void OnCollisionEnter(Collision collisionInfo)//Updates when you begin to touch something
    {
        Debug.Log("collisionInfo.gameObject.tag");//returns the tag of what you collide with
        if (collisionInfo.gameObject.tag == "ground")// if you collide with the ground
        {
            isGrounded = true;//you are grounded, unsure if keeping the isGrounded boolean would change anything or not because the player controller already has its own private grounded bool, but whatever
        }
    }
    private void OnCollisionExit(Collision collisionInfo)// updates when you stop touching a collider
    {
        Debug.Log("collisionInfo.gameObject.tag");//tells you what you stopped touching
        if (collisionInfo.gameObject.tag == "ground")//if you stop touching the ground
        {
            isGrounded = false;//you are not grounded
        }
    }
    private void OnTriggerEnter(Collider collisionInfo)//if you enter a collider marked as a trigger
    {
        if (collisionInfo.gameObject.tag == "extremoenemy")//if you enter the trigger area of the "extremoenemy"
        {
            Destroy(player, 0f);//you get destroyed immediately
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }
    }
}