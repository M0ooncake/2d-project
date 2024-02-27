using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goat : EnemyBase
{
    public float projectileFireRate;

    float timeSinceLastFire = 0;
    float DistThreshold = 3.0f;

    protected override void Start()
    {
        base.Start();


        if (projectileFireRate <= 0)
            projectileFireRate = 2.0f;

        //do any addional stuff that is need for my subclass.
    }

    private void Update()
    {
        if (!GameManager.Instance.PlayerInstance) return;
        //replace all the references to player.x or y to GameManager.Instance.PlayerInstance.transform.position.x or y
        GameObject player = GameObject.Find("Player");
        AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0);

        float difference = GameManager.Instance.PlayerInstance.transform.position.y - gameObject.transform.position.y;
        if (curPlayingClips[0].clip.name != "GoatFire" && difference < 10)
        {
            if (Time.time >= timeSinceLastFire + projectileFireRate)
            {

                anim.SetTrigger("Fire");
                timeSinceLastFire = Time.time;
            }
        }
        // Find the player GameObject


        if (player != null) return;
  
        if (GameManager.Instance.PlayerInstance.transform.position.x < gameObject.transform.position.x)
        {
            sr.flipX = true;
        }
        else sr.flipX = false;

        //if (player.transform.position.x > )
    }
}