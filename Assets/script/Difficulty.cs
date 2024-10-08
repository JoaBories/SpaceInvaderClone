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
        startAlienCount = alienGroupY.GetComponent<AlienGroupY>().initiationList.Sum();
    }

    void Update()
    {
        alienCount = 0;
        alienGroupYChildCount = alienGroupY.transform.childCount;
        for (int i = 0; i < alienGroupYChildCount; i++)
        {
            alienCount += alienGroupY.transform.GetChild(i).childCount;
        }
        Debug.Log(alienCount);  

        if (alienCount <= startAlienCount/2 && !firstLOD)
        {
            Debug.Log("first");
            firstLOD = true;
            alienGroupY.GetComponent<AlienGroupY>().speed *= 1.5f;
        }
        else if (alienCount <= startAlienCount/4 && !secondLOD)
        {
            Debug.Log("deuz");
            secondLOD = true;
            alienGroupY.GetComponent<AlienGroupY>().speed *= 1.5f;
        }
        else if (alienCount <= startAlienCount/8 && !thirdLOD)
        {
            Debug.Log("troiz");
            thirdLOD = true;
            alienGroupY.GetComponent<AlienGroupY>().speed *= 2f;
        }
        else if (alienCount == 1 && !fourthLOD)
        {
            Debug.Log("fourth");
            fourthLOD = true;
            alienGroupY.GetComponent<AlienGroupY>().speed *= 3f;
        }
    }
}
