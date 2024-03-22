using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    protected SpriteRenderer sr;
    protected Animator anim;
    [SerializeField] public AudioClip hitSound;
    [SerializeField] public AudioClip deathSound;


    protected int health;
    protected int maxHealth;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        if (maxHealth <= 0)
        {
            maxHealth = 10;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
