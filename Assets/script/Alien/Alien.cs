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

    public AudioClip hitSound;

    private void Update()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (transform.position.y < -0.5f) spriteRenderer.color = red;
        else if (transform.position.y > 2) spriteRenderer.color = violet;
        else
        {
            spriteRenderer.color = Color.Lerp(red, violet, (transform.position.y + 0.5f) / 2.5f);
        }

    }

    private void OnDestroy()
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

    bool isAlienBelow()
    {
        posY = transform.position.y;
        if (posY > GameController.Instance.alienGroup.GetComponent<AlienGroup>().bottomAlienPos()) return true;
        else return false;
        
    }
}
