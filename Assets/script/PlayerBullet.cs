using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float maxHeight;
    public float speed;

    void Update()
    {
        if (transform.position.y < maxHeight) transform.position += new Vector3(0, speed * Time.deltaTime, 0);
        else
        {
            gameObject.SetActive(false);
            GameController.Instance.failedShot();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Alien"))
        {
            collision.gameObject.GetComponent<Alien>().destroyByBullet = true;
            Destroy(collision.gameObject);
            gameObject.SetActive(false);
        }
        if (collision.gameObject.CompareTag("ShieldBlock"))
        {
            collision.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }


}
