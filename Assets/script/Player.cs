using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public GameObject MaxLeft;
    public GameObject MaxRight;
    public GameObject bulletPrefab;

    public float speed;
    public bool canMove = true;
    public bool invincibility = false;

    GameObject currentBullet;

    void Start()
    {
        currentBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        currentBullet.SetActive(false);
    }

    void Update()
    {
        if (canMove)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                if (transform.position.x < MaxRight.transform.position.x)
                {
                    transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
                }
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (transform.position.x > MaxLeft.transform.position.x)
                {
                    transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!currentBullet.activeSelf)
                {
                    Destroy(currentBullet);
                    currentBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                }
            }
        }
    }

    public void respawn()
    {
        transform.position = new Vector3(0, -4, 0); 
        canMove = true;
        invincibility = true;
    }

    public void removeInvincibility()
    {
        invincibility = false;
    }

    
}
