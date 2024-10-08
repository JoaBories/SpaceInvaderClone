using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienGroup : MonoBehaviour
{
    public GameObject alienPrefab;

    public GameObject maxRight;
    public GameObject maxLeft;
    public float xSpeed;
    public float ySpeed;
    bool goingRight = true;

    
    
    //spawn
    public List<int> initiationList;

    public float yDistance;
    public float xDistance;
    
    float xPos;
    float yPos;

    float scale;

    float width;
    float height;

    GameObject currentAlien;

    void Start()
    {
        spawn(initiationList, xDistance, yDistance);
    }

    void Update()
    {

        if (goingRight && (rightAlienPos() + (scale/2) >= maxRight.transform.position.x) || (!goingRight && (leftAlienPos() - (scale / 2) <= maxLeft.transform.position.x)))
        {
            goingRight = !goingRight;
            transform.position -= new Vector3(0, ySpeed, 0);
        }
        else
        {
            if (goingRight) transform.position += new Vector3(xSpeed * Time.deltaTime, 0, 0);
            else transform.position -= new Vector3(xSpeed * Time.deltaTime, 0, 0);
        }
    }

    void spawn(List<int> spawnList, float xDistance, float yDistance)
    {
        yPos = 0;
        scale = transform.localScale.x;

        foreach (int alienNumber in spawnList)
        {
            if (alienNumber == 1) xPos = 0;
            else if (alienNumber % 2 == 0) xPos = -((xDistance / 2) + (scale/2) + ((scale + xDistance) * ((alienNumber / 2) - 1)));
            else xPos = -(scale + xDistance) * ((alienNumber -1)/ 2);

            for (int i = 0; i < alienNumber; i++)
            {
                currentAlien = Instantiate(alienPrefab, transform);
                currentAlien.transform.localPosition = new Vector3(xPos, yPos, 0);
                xPos += (scale + xDistance);
            }

            yPos -= (scale + yDistance);
        }
    }

    float bottomAlienPos()
    {
        float minpos = transform.GetChild(0).transform.localPosition.y;
        for (int alienIndex = 1; alienIndex < transform.childCount; alienIndex++)
        {
            if(transform.GetChild(alienIndex).transform.localPosition.y < minpos) minpos = transform.GetChild(alienIndex).transform.localPosition.y;
        }
        return minpos;
    }

    float leftAlienPos()
    {
        float minpos = transform.GetChild(0).transform.position.x;
        for (int alienIndex = 1; alienIndex < transform.childCount; alienIndex++)
        {
            if (transform.GetChild(alienIndex).transform.position.x < minpos) minpos = transform.GetChild(alienIndex).transform.position.x;
        }
        return minpos;
    }

    float rightAlienPos()
    {
        float maxpos = transform.GetChild(0).transform.position.x;
        for (int alienIndex = 1; alienIndex < transform.childCount; alienIndex++)
        {
            if (transform.GetChild(alienIndex).transform.position.x > maxpos) maxpos = transform.GetChild(alienIndex).transform.position.x;
        }
        return maxpos;
    }
}
