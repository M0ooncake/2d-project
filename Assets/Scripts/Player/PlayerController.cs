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
    public bool isPaused = false;
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
    public GameObject wallSlideParticlesPrefab;
    public GameObject jumpParticlePreFab;
    private bool isLeftWalled;
    private bool isRightWalled;
    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;
    private bool isWallSlideParticlesActive = false;
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.02f;
    private float wallJumpCounter;
    private float wallJumpDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(90f, 16f);

    // movement Vars
    [SerializeField] private float speed = 7.0f;
    [SerializeField] private float JumpForce = 30.0f;

    [SerializeField] private bool isGrounded;
    [SerializeField] private Transform GroundCheck;
    [SerializeField] private LayerMask isGroundLayer;
    [SerializeField] private float groundCheckRadius = 0.02f;
    [SerializeField] private Transform RightWallCheck;
    [SerializeField] private Transform LeftWallCheck;
    [SerializeField] private LayerMask wallLayer;

    [SerializeField] AudioClip shootSound;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip stompSound;



    //Components
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;
    AudioSource audioSource;
    CanvasManager canvasManager;

    //public float speed = 7.0f;
    // Start is called before the first frame update
    void Start()
    {
        
        //Component references grabbed through script
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        

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
    private bool canJump = true; // Flag to control jump input

    // Update is called once per frame
    void Update()
    {
        AnimatorClipInfo[] clipinfo = anim.GetCurrentAnimatorClipInfo(0);


        if (Time.timeScale == 0.0f || clipinfo[0].clip.name == "death")
        {
            //do nothing
            //and exit the fucntion

            return;
        }
        
        xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");
        
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, isGroundLayer);

        if (!isWallSliding) rb.velocity = new Vector2(xInput * speed, rb.velocity.y);
        //rb.AddForce();


        if (Input.GetButtonDown("Fire1"))
        {
            if (clipinfo[0].clip.name == "ShootUp")
            {
                Fire(Vector2.up, spawnPointTop);
            }
            else if ((clipinfo[0].clip.name == "ShootDown"))
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
            audioSource.PlayOneShot(jumpSound);
            

            StartCoroutine(spawnJumpParticle());



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
        audioSource.PlayOneShot(shootSound);
        if (curProjectile.xVel < 0)
        {
            curProjectile.GetComponent<SpriteRenderer>().flipX = true;
        }
        
    }

    private bool IsWalled()
    {
        anim.SetBool("IsSliding", true);

        // Check if either left or right wall check is colliding with a wall
        isLeftWalled = Physics2D.OverlapCircle(LeftWallCheck.position, 0.2f, wallLayer);
        isRightWalled = Physics2D.OverlapCircle(RightWallCheck.position, 0.2f, wallLayer);

        if (isLeftWalled)
        {
            sr.flipX = false;
        }
        if (isRightWalled)
        {
            sr.flipX = true;
        }

        return isLeftWalled || isRightWalled;
    }
    private void WallSlide()
    {
        float yInput = Input.GetAxisRaw("Vertical");
        xInput = Input.GetAxisRaw("Horizontal");

        if (IsWalled() && !isGrounded && xInput != 0)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            // Spawn particles here
            if (!isWallSlideParticlesActive)
            {
                StartCoroutine(SpawnWallSlideParticles());
            }
        }
        else
        {
            isWallSliding = false;
            anim.SetBool("IsSliding", false);
        }

        if (Input.GetButtonDown("Jump") && wallJumpCounter > 0f)
        {
            isWallJumping = true;

            // Calculate the direction of the wall jump
            wallJumpingDirection = isLeftWalled ? 1f : -1f;

            // Calculate the force vector for wall jumping
            Vector2 forceVector = Vector2.right * wallJumpingDirection * wallJumpingPower.x;

            // Apply the force for wall jumping smoothly over time
            StartCoroutine(ApplyWallJumpForceSmoothly(forceVector, wallJumpingPower.y, wallJumpingTime));

            // Reset wall jumping counter
            wallJumpCounter = 0f;
        }

        Invoke(nameof(StopWallJumping), wallJumpDuration);
    }

    private IEnumerator ApplyWallJumpForceSmoothly(Vector2 forceVector, float upForce, float duration)
    {
        float elapsed = 0f;
        Vector2 initialVelocity = rb.velocity;
        Vector2 targetVelocity = forceVector + Vector2.up * upForce;

        while (elapsed < duration)
        {
            // Calculate the smoothed velocity using Vector3.SmoothDamp
            Vector2 smoothedVelocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref initialVelocity, duration);

            // Apply the smoothed velocity to the rigidbody
            rb.velocity = smoothedVelocity;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the final velocity is the target velocity
        rb.velocity = targetVelocity;

        isWallJumping = false;
    }

    // Coroutine to spawn wall slide particles
    private IEnumerator SpawnWallSlideParticles()
    {
        isWallSlideParticlesActive = true; // Set flag to true

        // Determine the position to spawn the wall slide particles
        Vector2 spawnPosition = isLeftWalled ? LeftWallCheck.position : RightWallCheck.position;
        spawnPosition.y -= 0.5f;

        // Spawn wall slide particles prefab at the determined position
        GameObject obj = Instantiate(wallSlideParticlesPrefab, spawnPosition, Quaternion.identity);
        obj.transform.SetParent(gameObject.transform);

        // Continue the coroutine until the player stops wall sliding
        while (isWallSliding)
        {
            yield return null;
        }

        // Destroy the particle object when the player stops wall sliding
        Destroy(obj);
        isWallSlideParticlesActive = false; // Set flag to false after particles have finished
    }

    private IEnumerator spawnJumpParticle()
    {
        //add spawning of jump particle and destruction of particle 
        
        // Use the player's position to determine the spawn position
        Vector2 spawnPos = GameManager.Instance.PlayerInstance.transform.position;

        // Subtract a fixed value from the Y position to place the particle below the player
        spawnPos.y -= 0.0f; // Adjust this value as needed

        // Spawn the jump particle at the calculated position
        GameObject obj = Instantiate(jumpParticlePreFab, spawnPos, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
        Destroy(obj);
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
    //trigger functions are called most other times - but would still require at least one object to be physics enabled
    private void OnTriggerEnter2D(Collider2D collision)
    {
        AnimatorClipInfo[] clipinfo = anim.GetCurrentAnimatorClipInfo(0);
        canvasManager = GetComponent<CanvasManager>();

        if ((collision.CompareTag("EnemyProjectile") && clipinfo[0].clip.name != "death") || (collision.CompareTag("Enemy") && clipinfo[0].clip.name != "death"))
        {
            GameManager.Instance.lives--;
            //canvasManager.UpdateLifeText(GameManager.Instance.lives);
            anim.SetTrigger("Death");
            rb.velocity = Vector2.zero;
        }
        
       
        
        if (collision.CompareTag("Fairy"))
        {
            GameManager.Instance.lives++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {

    }

    //Collision functions are only called - when one of the two objects is a dynamic rigidbody
    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
    }
}
