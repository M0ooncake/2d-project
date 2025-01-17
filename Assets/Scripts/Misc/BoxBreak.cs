using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;



public class BoxBreak : MonoBehaviour
{
    public Collectable DiamondPrefab;
    public Collectable EmeraldPrefab;
    public Collectable FairyPrefab;
    public Collectable PearlPrefab;
    

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
        if (collision.CompareTag("PlayerProjectile"))
        {
            double spawnChance = Random.value * 100;
            if (spawnChance < 50)
            {
                //do nothing
            } 
            else if (spawnChance > 50 && spawnChance < 70) 
            {
                //spawn a diamond OR a emerald, using another random value
                double spawnGem = Random.value * 100;
                switch (spawnGem)
                {
                    case < 50:
                        //spawn diamond
                        //collision destory(collision.gameObject)
                        Collectable curDiamond = Instantiate(DiamondPrefab, transform.position, transform.rotation);
                        break;
                    case > 50:
                        //spawn emerald
                        Collectable curEmerald = Instantiate(EmeraldPrefab, transform.position, transform.rotation);
                        break;
                }
            }
            else if (spawnChance > 70 && spawnChance < 90)
            {
                //spawn a pearl
                Collectable curPearl = Instantiate(PearlPrefab, transform.position, transform.rotation);
            } 
            else if (spawnChance > 90)
            {
                //spawn a fairy
                Collectable curFairy = Instantiate(FairyPrefab, transform.position, transform.rotation);
            }
            Destroy(gameObject);
        }
        
    }
    private void OnDestroy()
    {
        /*double spawnChance = Random.value * 100;
        if (spawnChance < 50)
        {
            //do nothing
        } 
        else if (spawnChance > 50 && spawnChance < 70) 
        {
            //spawn a diamond OR a emerald, using another random value
            double spawnGem = Random.value * 100;
            switch (spawnGem)
            {
                case < 50:
                    //spawn diamond
                    //collision destory(collision.gameObject)
                    Collectable curDiamond = Instantiate(DiamondPrefab, transform.position, transform.rotation);
                    break;
                case > 50:
                    //spawn emerald
                    Collectable curEmerald = Instantiate(EmeraldPrefab, transform.position, transform.rotation);
                    break;
            }
        }
        else if (spawnChance > 70 && spawnChance < 90)
        {
            //spawn a pearl
            Collectable curPearl = Instantiate(PearlPrefab, transform.position, transform.rotation);
        } 
        else if (spawnChance > 90)
        {
            //spawn a fairy
            Collectable curFairy = Instantiate(FairyPrefab, transform.position, transform.rotation);
        }*/
    } //transform.position
    //transform.rotataion
}
