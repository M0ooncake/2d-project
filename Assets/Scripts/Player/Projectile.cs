using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public float lifeTime;
    // Start is called before the first frame update
    [HideInInspector]

    public float yVel; // to make project go up or down
    public float xVel; // to make porject go side to side
    void Start()
    {
        if (lifeTime <= 0) lifeTime = 2.0f;

        GetComponent<Rigidbody2D>().velocity = new Vector2(xVel, yVel);
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && gameObject.CompareTag("PlayerProjectile"))
        {
            IEnemy enemy = collision.gameObject.GetComponent<IEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(1);
            }
        }

        if (collision.transform.parent != null && 
            (collision.transform.parent.name != "Player" || collision.transform.parent.name != "PlayerProjectile"))
        {
            Destroy(gameObject);
        }

        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Box"))
        {
            Destroy(gameObject);
        }
    }
}
