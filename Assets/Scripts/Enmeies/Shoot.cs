using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public bool TestMode = false;

    SpriteRenderer sr;

    [SerializeField] float initalXVelocity = 7.0f;
    [SerializeField] float initalYVelocity = 7.0f;

    public Transform spawnPointLeft;
    public Transform spawnPointRight;

    public Projectile projectilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (!spawnPointLeft || !spawnPointRight || !projectilePrefab)
        {
            if (TestMode) Debug.Log("Set default values Shoot Script. On object " + gameObject.name);
        }

    }

    public void Fire()
    {
        if (!sr.flipX)
        {
            Projectile curProjectile = Instantiate(projectilePrefab, spawnPointRight.position, spawnPointRight.rotation);
            curProjectile.xVel = initalXVelocity;
            curProjectile.yVel = initalYVelocity;
        }
        else
        {
            Projectile curProjectile = Instantiate(projectilePrefab, spawnPointLeft.position, spawnPointLeft.rotation);
            curProjectile.xVel = -initalXVelocity;
            curProjectile.yVel = initalYVelocity;
        }
    }
}
