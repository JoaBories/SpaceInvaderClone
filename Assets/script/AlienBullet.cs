using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AlienBullet : MonoBehaviour
{
    public float minHeight;
    public float speed;
    public Color red;
    public Color violet;
    SpriteRenderer spriteRenderer;

    private void Update()
    {
        if (transform.position.y > minHeight) transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
        else Destroy(gameObject);

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (transform.position.y < -0.5f) spriteRenderer.color = red;
        else if (transform.position.y > 2) spriteRenderer.color = violet;
        else
        {
            spriteRenderer.color = Color.Lerp(red, violet, (transform.position.y + 0.5f) / 2.5f);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameController.Instance.hitPlayer();
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("ShieldBlock"))
        {
            Destroy(gameObject);
            collision.gameObject.SetActive(false);
        }
    }
}
