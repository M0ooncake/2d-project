using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullAI : EnemyBase, IEnemy
{
    public Rigidbody2D rb;
    //SpriteRenderer sr;
    //public int HealthPoints = 2;
    public float speed = 2;
    public GameObject PointA;
    public GameObject PointB;
    //public Animator anim;
    public Transform currentPoint;
    public Transform target;
    public int Skullhealth = 2;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        currentPoint = PointB.transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        

        Vector2 point = currentPoint.position - transform.position;
        if(currentPoint == PointB.transform)
        {
            rb.velocity = new Vector2(speed, 0);
        }
        else
        {
            rb.velocity = new Vector2 (-speed, 0);
        }
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == PointB.transform)
        {
            flip();
            currentPoint = PointA.transform;
        }
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == PointA.transform)
        {
            flip();
            currentPoint = PointB.transform;
        }
        
    }
    
    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(PointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(PointB.transform.position, 0.5f);
        Gizmos.DrawLine(PointA.transform.position, PointB.transform.position); 
    }

    public void TakeDamage(int damage)
    {
        //ceate a dmg fucntiuon that will delete the path points it uses too
        Skullhealth -= damage;
        GetComponent<AudioSource>().PlayOneShot(hitSound);
        if (Skullhealth <= 0)
        {
            GetComponent<AudioSource>().PlayOneShot(deathSound);
            //GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject.transform.parent.gameObject, deathSound.length);
            

        }
        //base.TakeDamage(damage);
    }

}
