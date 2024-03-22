using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public enum PickupType
    {
        Powerup,
        Life,
        Score,
    }

    [SerializeField] PickupType currentPickup;
    [SerializeField] AudioClip pickupSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController pc = collision.GetComponent<PlayerController>();
            switch (currentPickup)
            {
                case PickupType.Powerup:
                    break;
                case PickupType.Life:
                    //GameManager.Instantiate.lives
                    //and a bunch of other stuff to make the particle to be displayed around the player
                    break;
                case PickupType.Score:
                    break;
            }
            GetComponent<AudioSource>().PlayOneShot(pickupSound);
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject, pickupSound.length);
        }
    }
    
}
