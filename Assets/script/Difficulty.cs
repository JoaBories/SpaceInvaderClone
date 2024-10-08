using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Difficulty : MonoBehaviour
{
    public GameObject alienGroupY;
    public GameObject player;

    int alienGroupYChildCount;
    int alienCount = 0;
    int startAlienCount;

    bool firstLOD = false;
    bool secondLOD = false;
    bool thirdLOD = false;
    bool fourthLOD = false;


    void Start()
    {
        //startAlienCount = alienGroupY.GetComponent<AlienGroupY>().initiationList.Sum();
    }

    void Update()
    {
        alienCount = 0;
        alienGroupYChildCount = alienGroupY.transform.childCount;
        for (int i = 0; i < alienGroupYChildCount; i++)
        {
            alienCount += alienGroupY.transform.GetChild(i).childCount;
        }

        if (alienCount <= startAlienCount/2 && !firstLOD)
        {
        //    firstLOD = true;
        //    alienGroupY.GetComponent<AlienGroupY>().speed *= 1.5f;
        //}
        //else if (alienCount <= startAlienCount/4 && !secondLOD)
        //{
        //    secondLOD = true;
        //    alienGroupY.GetComponent<AlienGroupY>().speed *= 1.5f;
        //}
        //else if (alienCount <= startAlienCount/8 && !thirdLOD)
        //{
        //    thirdLOD = true;
        //    alienGroupY.GetComponent<AlienGroupY>().speed *= 2f;
        //}
        //else if (alienCount == 1 && !fourthLOD)
        //{
        //    fourthLOD = true;
        //    alienGroupY.GetComponent<AlienGroupY>().speed *= 3f;
        //}
    }
}
