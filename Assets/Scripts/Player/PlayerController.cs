using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

//ensures that these components are attached to the gameobject
[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    private float xInput;
    public bool TestMode = false;
    public Rigidbody2D projectile;
    public float projectileSpeed;
    public Transform spawnPointLeft;
    public Transform spawnPointRight;
    public Transform spawnPointTop;
    public Transform spawnPointBottom;
    public Projectile projectilePrefab;
    public GameObject obj;
    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;
    private int _lives;
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpCounter;
    private float wallJumpDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    // movement Vars
    [SerializeField] private float speed = 7.0f;
    [SerializeField] private float JumpForce = 30.0f;

    [SerializeField] private bool isGrounded;
    [SerializeField] private Transform GroundCheck;
    [SerializeField] private LayerMask isGroundLayer;
    [SerializeField] private float groundCheckRadius = 0.02f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    
    [SerializeField] private int _maxLives = 5;
    public int lives
    {
        get => _lives;

        set
        {
            //if (_lives > value)
            //we lost a life
            //respawn now

            _lives = value;

            //if (_lives > maxLives)
            //weve increased passed the max so we should only set it to the max
            //_lives = maxLives

            //if (_lives <= 0)
            //GG no re
            if (TestMode) Debug.Log("Lives have been set to: " + _lives.ToString());
        }
    }
    
    
   

    //Components
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;

    //public float speed = 7.0f;
    // Start is called before the first frame update
    void Start()
    {
        
        //Component references grabbed through script
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (projectileSpeed <= 0)
        {
            projectileSpeed = 7.0f;
        }
        if (!spawnPointLeft || !spawnPointRight || !spawnPointTop || !spawnPointBottom || !projectilePrefab)
        {

        }
        if (speed <= 0)
        {
            speed = 7.0f;
            if (TestMode) Debug.Log("Speed has been set to a default value of 7.0f " + gameObject.name);
        }

        if (groundCheckRadius <= 0)
        {
            groundCheckRadius = 0.02f;
            if (TestMode) Debug.Log("groungCheckRadius Defualted to 0.02f");
        }

        if (GroundCheck == null)
        {
            GameObject obj = new GameObject();
            obj.transform.SetParent(gameObject.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.name = "GroundCheck";
            GroundCheck = obj.transform;
            if (TestMode) Debug.Log("Groundcheck Object is created" + obj.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        
        xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");
        AnimatorClipInfo[] clipifo = anim.GetCurrentAnimatorClipInfo(0);
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, isGroundLayer);

        if (!isWallSliding) rb.velocity = new Vector2(xInput * speed, rb.velocity.y);
        //rb.AddForce();
        

        if (Input.GetButtonDown("Fire1"))
        {
            if (clipifo[0].clip.name == "ShootUp")
            {
                Fire(Vector2.up, spawnPointTop);
            }
            else if ((clipifo[0].clip.name == "ShootDown"))
            {
                Fire(Vector2.down, spawnPointBottom);
            }
            else
            {

                if (sr.flipX)
                {
                    anim.SetTrigger("IsCasting");
                    Fire(Vector2.left, spawnPointLeft);
                }
                else
                {
                    anim.SetTrigger("IsCasting");
                    Fire(Vector2.right, spawnPointRight);
                }
             
            }

        }
       


        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
        }
    
        // anim.GetBool will return true or false
        anim.SetBool("IsGrounded", isGrounded);

        anim.SetFloat("Speed", Mathf.Abs(xInput)); 

        //if (xInput > 0) sr.flipX = false; // moving right it will flip to look right
        //if (xInput < 0) sr.flipX = true; // moving left it will flip to look left
        if (xInput != 0) sr.flipX = (xInput < 0);
        
        if (yInput > 0)
        {
            anim.SetBool("IsLookingUp", true);
            anim.SetBool("IsLookingDown", false);


        }
        else if (yInput < 0)
        {
            anim.SetBool("IsLookingDown", true);
            anim.SetBool("IsLookingUp", false);
        }
        else
        {
            anim.SetBool("IsLookingDown", false);
            anim.SetBool("IsLookingUp", false);
        }

        WallSlide();
        WallJump();
    }
    public void Fire(Vector2 dir, Transform spawnPoints)
    {
        Vector2 vel = dir * projectileSpeed;
        Projectile curProjectile = Instantiate(projectilePrefab, spawnPoints.position, spawnPoints.rotation);
        curProjectile.xVel = vel.x * 2;
        curProjectile.yVel = vel.y * 2;
        if (curProjectile.xVel < 0)
        {
            curProjectile.GetComponent<SpriteRenderer>().flipX = true;
        }
        
    }

    private bool IsWalled()
    {
        anim.SetBool("IsSliding", true);
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }
    private void WallSlide()
    {
        float yInput = Input.GetAxisRaw("Vertical");
        xInput = Input.GetAxisRaw("Horizontal");
        if (IsWalled() && !isGrounded && xInput != 0)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
            anim.SetBool("IsSliding", false);
        }

        if (Input.GetButtonDown("Jump") && wallJumpCounter > 0f)
        {
            isWallJumping = true;

            //rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            Vector2 forceVector = sr.flipX ? Vector2.right * 150 : Vector2.left * 150;


            rb.AddForce(forceVector + (Vector2.up * JumpForce), ForceMode2D.Impulse);
            isWallJumping = false;

        }
        Invoke(nameof(StopWallJumping), wallJumpDuration);
    }
    private void StopWallJumping()
    {
        isWallJumping = false;
    }
    private void WallJump()
    {
        if(isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -xInput;
            wallJumpCounter = wallJumpingTime;
            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpCounter -= Time.deltaTime;

        }

    }
}