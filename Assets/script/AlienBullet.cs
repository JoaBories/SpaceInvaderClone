using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AlienBullet : MonoBehaviour
{
    public float minHeight;
    public float speed;

    void Update()
    {
        if (transform.position.y > minHeight) transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
        else Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!collision.gameObject.GetComponent<player>().invincibility)
            {
                collision.gameObject.GetComponent<Animator>().Play("playerHit");
                collision.gameObject.GetComponent<player>().canMove = false;
            }
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("ShieldBlock"))
        {
            Destroy(gameObject);
            collision.gameObject.SetActive(false);
        }
    }
}
