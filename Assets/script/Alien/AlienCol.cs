using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienCol : MonoBehaviour
{
    public GameObject bulletPrefab;

    public float shootCooldown;
    public int shootOdd;
    GameObject currentBullet;
    bool shooting;
    float nextShoot;


    void Start()
    {
    }

    void Update()
    {
        if(transform.childCount == 0)
        {
            Destroy(gameObject);
        }

        if (shooting && Time.time > nextShoot)
        {       
            int randInt = Random.Range(0,shootOdd);
            if (randInt == 0)
            {
                Instantiate(bulletPrefab, new Vector3(transform.position.x, bottomAlienPos(), 0), Quaternion.identity);
            }
            nextShoot = Time.time + shootCooldown;
        }
    }

    float bottomAlienPos()
    {
        float minPos = transform.GetChild(0).position.y;
        for (int i = 1; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).position.y < minPos) minPos = transform.GetChild(i).position.y;
        }
        return minPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            shooting = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            shooting = false;
        }
    }
}
