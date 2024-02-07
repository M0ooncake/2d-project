using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEditor.Tilemaps;
using UnityEngine;

//ensures that these components are attached to the gameobject
[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public bool TestMode = false;
    public Rigidbody2D projectile;
    public float projectileSpeed;
    public Transform spawnPointLeft;
    public Transform spawnPointRight;
    public Transform spawnPointTop;
    public Transform spawnPointBottom;

    public Projectile projectilePrefab;
    public GameObject obj;
   
// movement Vars
    [SerializeField] private float speed = 7.0f;
    [SerializeField] private float JumpForce = 30.0f;

    [SerializeField] private bool isGrounded;
    [SerializeField] private Transform GroundCheck;
    [SerializeField] private LayerMask isGroundLayer;
    [SerializeField] private float groundCheckRadius = 0.02f;

    private int _lives;
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
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");
        AnimatorClipInfo[] clipifo = anim.GetCurrentAnimatorClipInfo(0);
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, isGroundLayer);

        rb.velocity = new Vector2(xInput * speed, rb.velocity.y);
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
}
