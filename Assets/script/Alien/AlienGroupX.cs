using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienGroupX : MonoBehaviour
{
    public GameObject alienPrefab;
    public float alienNumber;
    public float alienDistance;
    public GameObject currentAlien;
    
    float alienPos;
    float scale;

    void Start()
    {
        scale = transform.localScale.x;
        if(alienNumber % 2 == 0)
        {
            alienPos = -(alienDistance/2 + scale/2) - (1 + alienDistance) * (alienNumber / 2 - 1);
        }
        else
        {
            alienPos = -(scale + alienDistance) * (alienNumber - 1) / 2; ;
        }
        for (int i = 0; i < alienNumber; i++)
        {
            currentAlien = Instantiate(alienPrefab, transform);
            currentAlien.transform.localPosition = new Vector3(alienPos, 0, 0);
            alienPos += (1 + alienDistance);
        }
    }

}
