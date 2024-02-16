using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullAI : MonoBehaviour
{
    public Rigidbody2D rb;
    SpriteRenderer sr;
    public int HealthPoints = 2;
    public float speed = 2;
    public GameObject PointA;
    public GameObject PointB;
    public Animator anim;
    public Transform currentPoint;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
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

}
