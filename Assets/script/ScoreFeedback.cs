using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreFeedback : MonoBehaviour
{
    float disappearTime;

    void Start()
    {
        disappearTime = Time.time + 1;
    }

    private void Update()
    {
        if (disappearTime <= Time.time)
        {
            Destroy(gameObject);
        }
    }

}
