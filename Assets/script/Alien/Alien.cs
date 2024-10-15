using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    public GameObject scoreTextPrefab;
    public Color red;
    public Color violet;
    SpriteRenderer spriteRenderer;

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
        Instantiate(scoreTextPrefab, transform.position, Quaternion.identity);
    }

}
