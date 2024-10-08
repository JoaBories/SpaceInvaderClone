using System.Collections;
using System.Collections.Generic;
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
}
