using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienGroupX : MonoBehaviour
{
    public GameObject alienPrefab;

    public float alienNumber;
    
    float alienDistance = 0.5f;
    double alienPos;

    public GameObject currentAlien;

    void Start()
    {
        if(alienNumber % 2 == 0)
        {
            alienPos = -(0.25f + 0.5f) - (1 + alienDistance) * (alienNumber / 2 - 1);
            for (int i = 0; i < alienNumber; i++)
            {
                currentAlien = Instantiate(alienPrefab, transform);
                currentAlien.transform.localPosition = new Vector3(((float) alienPos), 0, 0);
                alienPos += (1 + alienDistance);
            }
        }
        else
        {
            alienPos = -(1 + 0.5f) * (alienNumber - 1) / 2; ;
            for (int i = 0; i < alienNumber; i++)
            {
                currentAlien = Instantiate(alienPrefab, transform);
                currentAlien.transform.localPosition = new Vector3(((float)alienPos), 0, 0);
                alienPos += (1 + alienDistance);
            }
        }

    }

    void Update()
    {

    }
}
