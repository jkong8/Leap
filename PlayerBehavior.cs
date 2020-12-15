/*AUTHOR: JEFFERY KONG, JEREMY TUCKER
 * NOTE: CONTROLS ALL ASPECT OF THE MAIN CHARACTER*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    //Other Scripts
    [SerializeField]
    GameBehavior gameBehavior;

    //Components
    Rigidbody2D rb;
    Transform _transform;
    BoxCollider2D boxCollider;
    SpriteRenderer spriteRenderer;
    [SerializeField]
    LayerMask Groundlayer;
    [SerializeField]
    LayerMask Hazardlayer;
    [SerializeField]
    LayerMask Finishlayer;
    [SerializeField]
    Animator animator;
    public audiomanager audiomanager;

    //Variables
    float moveSpeed = 5f;
    Vector2 wallJumpRight = new Vector2(2, 4);
    Vector2 wallJumpLeft = new Vector2(-2, 4);
    Vector3 startPosition; //Needs to be initialized in start method
    bool WallJumpReady = true;
    bool dashReady = false;
    string currentDirection;

    void Start()
    {
        //Initializing
        rb = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
        boxCollider = GetComponent<BoxCollider2D>();
        startPosition = _transform.position;
        gameBehavior = GameObject.Find("GameManager").GetComponent<GameBehavior>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audiomanager = GameObject.Find("audiomanager").GetComponent<audiomanager>();
    }

    
    void Update()
    {
        updateDirection();
        isGrounded();
        isOnWall();
        Debug.Log(animator.GetBool("IsJumping"));
    }

    //Fixed Update for Movements and Stuff
    private void FixedUpdate()
    {
        ControlMovement();
    }

    //Controls Movement, Jumping, Wall Jumping, and Dashing of Character
    private void ControlMovement()
    {
        //Player Inputs
        float xInput = Input.GetAxis("Horizontal");
        bool space = Input.GetKey(KeyCode.Space);
        bool jKey = Input.GetKey(KeyCode.J);

        //Changes Animation When Moving
        animator.SetFloat("Speed", Mathf.Abs(xInput));

        //Horizontal Movement
        Vector2 movement = new Vector2(xInput, 0);
        rb.position += movement * moveSpeed * Time.deltaTime;
        

        //Dashing on J
        if (jKey && dashReady && !isGrounded())
        {
            Debug.Log("J Pressed");
            dashReady = false;
            switch (currentDirection)
            {
                case "right":
                    rb.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
                    break;
                case "left":
                    rb.AddForce(Vector2.right * -10, ForceMode2D.Impulse);
                    break;

            }
            //Play oneshot
            audiomanager.Audio.PlayOneShot(audiomanager.dash);
        }

        //Pressing Space
        if (space)
        {
            
            //If grounded Jump
            if (isGrounded())
            {
                
                //Play one shot
                audiomanager.Audio.PlayOneShot(audiomanager.jump);
                
                rb.AddForce(Vector2.up * 3f, ForceMode2D.Impulse);
                
            }
            else if(WallJumpReady)
            {
                //Wall Jumping
                switch (isOnWall())
                {
                    //IMPLEMENT WALL JUMPING HERE
                    case "left":
                        WallJumpReady = false;
                        rb.AddForce(wallJumpRight, ForceMode2D.Impulse);
                        break;
                    case "right":
                        WallJumpReady = false;
                        rb.AddForce(wallJumpLeft, ForceMode2D.Impulse);
                        break;
                }
            }
        }


    }

    //Returns True if Player is Grounded by layer
    private bool isGrounded()
    {
        Vector3 offset = new Vector3(0.25f, 0, 0);
        //Uses three rayscast to check for grounded
        RaycastHit2D raycastMiddle = Physics2D.Raycast(boxCollider.bounds.center, Vector2.down, boxCollider.bounds.extents.y + 0.2f, Groundlayer);
        RaycastHit2D raycastLeft = Physics2D.Raycast(boxCollider.bounds.center - offset, Vector2.down, boxCollider.bounds.extents.y + 0.2f, Groundlayer);
        RaycastHit2D raycastRight = Physics2D.Raycast(boxCollider.bounds.center + offset, Vector2.down, boxCollider.bounds.extents.y + 0.2f, Groundlayer);

        //One raycast for finish layers, if collision, set completion bool to true in GameBehavior script
        RaycastHit2D raycastFinish = Physics2D.Raycast(boxCollider.bounds.center, Vector2.down, boxCollider.bounds.extents.y + 0.2f, Finishlayer);
        if(raycastFinish.collider != null)
        {
            //Move to next level
            gameBehavior.nextLevel();
        }

        Color color;
        if (raycastMiddle.collider != null || raycastLeft.collider != null || raycastRight.collider != null)
        {
            color = Color.green;
            //Sets Animator Parameter
            if(animator.GetBool("IsJumping") != false)
            {
                animator.SetBool("IsJumping", false);
            }
            Debug.DrawRay(boxCollider.bounds.center, Vector2.down * boxCollider.bounds.extents.y, color);
            //Resets Wall Jump Availability
            WallJumpReady = true;
            dashReady = true;
            return true;
        }else
        {
            color = Color.red;
            //Sets Animator Parameter
            if (animator.GetBool("IsJumping") != true)
            {
                animator.SetBool("IsJumping", true);
            }
            Debug.DrawRay(boxCollider.bounds.center, Vector2.down * boxCollider.bounds.extents.y, color);
            return false;
        }
    }

    //Returns a string if the player is connected to a wall
    private string isOnWall()
    {
        Vector3 offset = new Vector3(0, 0.45f, 0);
        //Uses three rayscast to check for contact with ground layer on LEFT side
        RaycastHit2D LraycastMiddle = Physics2D.Raycast(boxCollider.bounds.center, Vector2.left, boxCollider.bounds.extents.x, Groundlayer);
        RaycastHit2D LraycastLeft = Physics2D.Raycast(boxCollider.bounds.center - offset, Vector2.left, boxCollider.bounds.extents.x, Groundlayer);
        RaycastHit2D LraycastRight = Physics2D.Raycast(boxCollider.bounds.center + offset, Vector2.left, boxCollider.bounds.extents.x, Groundlayer);

        //Uses three rayscast to check for contact with ground layer on RIGHT side
        RaycastHit2D RraycastMiddle = Physics2D.Raycast(boxCollider.bounds.center, Vector2.right, boxCollider.bounds.extents.x, Groundlayer);
        RaycastHit2D RraycastLeft = Physics2D.Raycast(boxCollider.bounds.center - offset, Vector2.right, boxCollider.bounds.extents.x, Groundlayer);
        RaycastHit2D RraycastRight = Physics2D.Raycast(boxCollider.bounds.center + offset, Vector2.right, boxCollider.bounds.extents.x, Groundlayer);

        Color color;
        //If one of the raycast is colliding
        if (LraycastMiddle.collider != null || LraycastLeft.collider != null || LraycastRight.collider != null)
        {
            color = Color.green;
            Debug.DrawRay(boxCollider.bounds.center, Vector2.left * boxCollider.bounds.extents.y, color);
            //Resets Wall Jump Availability
            WallJumpReady = true;
            return "left";
        }

        if (RraycastMiddle.collider != null || RraycastLeft.collider != null || RraycastRight.collider != null)
        {
            color = Color.green;
            Debug.DrawRay(boxCollider.bounds.center, Vector2.right * boxCollider.bounds.extents.y, color);
            //Resets Wall Jump Availability
            WallJumpReady = true;
            return "right";
        }
        return null;
    }

    private void updateDirection()
    {
        if(Input.GetAxis("Horizontal") > 0)
        {
            spriteRenderer.flipX = false;
            currentDirection = "right";
            Debug.Log(currentDirection);
        }
        else if(Input.GetAxis("Horizontal") < 0)
        {
            spriteRenderer.flipX = true;
            currentDirection = "left";
            Debug.Log(currentDirection);
        }
    }

    //When player collides with a hazard
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.layer == 9)
        {
            Debug.Log("Hazard!");
            //Move character to start position
            _transform.position = startPosition;
            audiomanager.Audio.PlayOneShot(audiomanager.hit);
          
        }
    }

}
