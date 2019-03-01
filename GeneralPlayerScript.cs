﻿using UnityEngine;
using System.Collections;

public class GeneralPlayerScript : MonoBehaviour
{
    public Camera Camera; //reference to the camera
    public CharacterController playercon; // reference to the player character
    public GameObject player; //reference to the player's gameobject(used to destroy it later)
    public float yrotation = 60.0f; // speed multiplier for y axis rotation
    public float xrotation = 60.0f; // speed multiplier for x axis rotation
    public float forwards = 5f;// speed multiplier for moving forwards
    public float backwards = -4f;// speed multiplier for moving backwards
    public float strafe = 5f;// speed multiplier for moving	left or right
    public bool isGrounded = false;//useless bool value that I can't get to function, not really needed, but it would be help to figure out any issues with movement/gravity
    public float jumpHeight = 0f;// base height for jumping, to be used in either a move function, or a lerped translate
    public float speed = 10f;//base speed multiplier for player movement
    public float gravity = 9.8f;//base speed for gravity
    private Vector3 move = Vector3.zero;//resets motion to zero
    public CapsuleCollider playercollider;// reference to a capsule collider on the player if I ever decide to add one for calculating forces and stuff
	
	void Update ()// Update is called once per frame
    {
        float y = yrotation * -Input.GetAxis("Mouse Y");// sets the float y to use the same value as yrotation and then multiplies that by the movement of the y axis of the mouse
        float x = xrotation * Input.GetAxis("Mouse X");// sets the float x to use the same value as xrotation and then multiplies that by the movement of the x axis of the mouse
        move = new Vector3(Input.GetAxis("Strafe"), 0, Input.GetAxis("Forwards")); //creates a new Vector3 for the values of movement
        move = transform.TransformDirection(move); //this basically isolates the movement to axes relative to the player instead of world axes
        move = move * speed; //multiplies the value of move by the multiplier for player's speed
        move.y = move.y + (jumpHeight - gravity);//moves the player down by gravity multiplied by Time.deltaTime
        Camera.transform.Rotate(-y * Time.deltaTime, 0, 0);//rotates vertical rotation of the camera but not the character
        playercon.transform.Rotate(0, x * Time.deltaTime, 0);// rotates the character and the camera horizontally, because the camera is a child of the character
        playercon.Move(move * Time.deltaTime * speed);// Moves the player. Movement is equal to base move speed * Time.deltaTime * the speed multiplier
        if (playercon.isGrounded == true)//if the player is resting on a surface with a collider
        {
            jumpHeight = 0f;// set   jump height to 0
            StopCoroutine("gravitySpeed");//stops adding speed to gravity when you touch the ground
            gravity = 9.8f;// resets gravity to 1500 u/s 
            if (Input.GetKey(KeyCode.Space))// if you press space while grounded
            {
                Debug.Log("jump attempted");//console message telling me when I attempt to jump
                StartCoroutine("jumping");
            }
        }
        if (playercon.isGrounded == false)//if the player is not touching a surface with collision
        {
            StartCoroutine("gravitySpeed");//starts increasing gravity exponentially
        }
    }
    private IEnumerator gravitySpeed()
    {
        yield return new WaitForSecondsRealtime(0.0166f);//waits for 60th of a second realtime
        gravity = gravity + 0.163f;// adds ((change in time since last frame update)+gravity) to gravity every 60th of a second for as long as the player is not grounded
    }
    private IEnumerator jumping()
    {
        gravity = 0f;//sets gravity to 0 at the start of the jump
        jumpHeight = 10f;//sets jumpHeight to 10
        yield return new WaitForSecondsRealtime(0.25f);//waits for a quarter of a second realtime
        jumpHeight = 9.9f;//sets jumpHeight to 9.9
        gravity = 9.8f; //sets gravity to 1 for a slower decline
    }
    void OnCollisionEnter(Collision collisionInfo)//Updates when you begin to touch something
    {
        Debug.Log("collisionInfo.gameObject.tag");//returns the tag of what you collide with
        if (collisionInfo.gameObject.tag == "ground")// if you collide with the ground
        {
            isGrounded = true;//you are grounded, unsure if keeping the isGrounded boolean would change anything or not because the player controller already has its own private grounded bool, but whatever
        }
    }
    void OnCollisionExit(Collision collisionInfo)// updates when you stop touching a collider
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
        }
    }
}