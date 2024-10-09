using System.Collections.Generic;
using UnityEngine;

public class AlienGroup : MonoBehaviour
{
    //movement
    [Header("movement")]
    public GameObject maxRight;
    public GameObject maxLeft;
    public float xSpeed;
    public float ySpeed;
    bool goingRight = true;

    //difficulty
    int alienN;
    int initialAlienCount;
    List<Vector2> inRoundDifficultyLevelList = new List<Vector2> {new Vector2(2, 1.5f), new Vector2(4, 3f), new Vector2(1, 6f) };
    int nextInRoundDifficultyLevel;
    bool inRoundMaxDifficulty;


    //spawn
    [Header("spawn")]
    public GameObject alienPrefab;
    public GameObject colPrefab;
    public int initialColNumber;
    public int initialLignNumber;
    public float yDistance;
    public float xDistance;
    
    float xPos;
    float yPos;
    float scale;
    float width;
    float height;

    GameObject currentAlien;
    GameObject currentCol;
    List<GameObject> colList = new List<GameObject>();

    void Start()
    {
        spawn(initialColNumber, initialLignNumber, xDistance, yDistance);
    }

    void Update()
    {
        alienN = alienCount();
        if ( alienN == 0 ) Destroy(gameObject);

        //movement
        if (goingRight && (rightColPos() + (scale/2) >= maxRight.transform.position.x) || (!goingRight && (leftColPos() - (scale / 2) <= maxLeft.transform.position.x)))
        {
            goingRight = !goingRight;
            transform.position -= new Vector3(0, ySpeed, 0);
        }
        else
        {
            if (goingRight) transform.position += new Vector3(xSpeed * Time.deltaTime, 0, 0);
            else transform.position -= new Vector3(xSpeed * Time.deltaTime, 0, 0);
        }

        if(alienN <= inRoundDifficultyLevelList[nextInRoundDifficultyLevel][0] && !inRoundMaxDifficulty)
        {
            xSpeed = inRoundDifficultyLevelList[nextInRoundDifficultyLevel][1];
            if(nextInRoundDifficultyLevel != inRoundDifficultyLevelList.Count-1) nextInRoundDifficultyLevel++;
            else inRoundMaxDifficulty = true;
        }

        
    }

    void spawn(int colNumber, int lignNumber, float xDistance, float yDistance)
    {
        yPos = 0;
        scale = transform.localScale.x;
        initialAlienCount = colNumber * lignNumber;
        nextInRoundDifficultyLevel = 0;
        inRoundMaxDifficulty = false;

        for(int i=0 ; i < inRoundDifficultyLevelList.Count; i++)
        {
            if (inRoundDifficultyLevelList[i].x != 1)
            {
                inRoundDifficultyLevelList[i] = new Vector2(initialAlienCount / inRoundDifficultyLevelList[i].x, inRoundDifficultyLevelList[i].y);
            }
        }

        if (colNumber == 1) xPos = 0;
        else if (colNumber % 2 == 0) xPos = -((xDistance / 2) + (scale / 2) + ((scale + xDistance) * ((colNumber / 2) - 1)));
        else xPos = -(scale + xDistance) * ((colNumber - 1) / 2);

        for (int i = 0; i < colNumber; i++)
        {
            currentCol = Instantiate(colPrefab, transform);
            currentCol.transform.localPosition = new Vector3(xPos, 0, 0);
            currentCol.GetComponent<BoxCollider2D>().size = new Vector2((xDistance+scale)*2, 30);
            colList.Add(currentCol);
            xPos += (scale + xDistance);
        }

        for (int j = 0; j < lignNumber; j++)
        {
            foreach (GameObject col in colList)
            {
                currentAlien = Instantiate(alienPrefab, col.transform);
                currentAlien.transform.localPosition = new Vector3(0, yPos, 0);
            }

            yPos -= (scale + yDistance);
        }
    }

    int alienCount()
    {
        int n = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            n += transform.GetChild(i).childCount;
        }
        return n;
    }

    float bottomAlienPos()
    {
        if (transform.childCount != 0)
        {
            float minpos = transform.GetChild(0).transform.GetChild(0).transform.localPosition.y;
            for (int colIndex = 0; colIndex < transform.childCount; colIndex++)
            {
                for (int alienIndex = 1; alienIndex < transform.GetChild(colIndex).childCount; alienIndex++)
                {
                    if (transform.GetChild(colIndex).transform.GetChild(alienIndex).transform.localPosition.y < minpos) minpos = transform.GetChild(colIndex).transform.GetChild(alienIndex).transform.localPosition.y;
                }
            }
            return minpos;
        }
        return 0;
    }

    float leftColPos()
    {
        if (transform.childCount != 0)
        {
            float minpos = transform.GetChild(0).transform.position.x;
            for (int colIndex = 1; colIndex < transform.childCount; colIndex++)
            {
                if (transform.GetChild(colIndex).transform.position.x < minpos) minpos = transform.GetChild(colIndex).transform.position.x;
            }
            return minpos;
        }
        return 0;
    }

    float rightColPos()
    {
        if (transform.childCount != 0)
        {
            float maxpos = transform.GetChild(0).transform.position.x;
            for (int colIndex = 1; colIndex < transform.childCount; colIndex++)
            {
                if (transform.GetChild(colIndex).transform.position.x > maxpos) maxpos = transform.GetChild(colIndex).transform.position.x;
            }
            return maxpos;
        }
        return 0;
    }
}
