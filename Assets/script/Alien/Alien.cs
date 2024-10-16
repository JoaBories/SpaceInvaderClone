using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    public GameObject TextPrefabShot;
    public GameObject TextPrefabLongShot;
    public Color red;
    public Color violet;
    SpriteRenderer spriteRenderer;
    float posY;
    int colAlienCount;

    public AudioClip hitSound;
    public bool destroyByBullet = false;


    private void Update()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (transform.position.y < -0.5f) spriteRenderer.color = red;
        else if (transform.position.y > 1) spriteRenderer.color = violet;
        else
        {
            spriteRenderer.color = Color.Lerp(red, violet, (transform.position.y + 0.5f) / 2.5f);
        }

    }

    private void OnDestroy()
    {
        if (destroyByBullet)
        {
            GameController.Instance.playSoundClip(hitSound, transform);
            if (transform.position.y >= 1.5 && isAlienBelow())
            {
                GameController.Instance.longShot();
                Instantiate(TextPrefabLongShot, transform.position, Quaternion.identity);
            }
            else
            {
                GameController.Instance.shot();
                Instantiate(TextPrefabShot, transform.position, Quaternion.identity);
            }
        }
    }

    bool isAlienBelow()
    {
        posY = transform.position.y;
        colAlienCount = transform.parent.childCount;
        if (colAlienCount > 1)
        {
            for (int i = 0; i < colAlienCount; i++)
            {
                if(transform.parent.GetChild(i).transform.position.y < posY)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
