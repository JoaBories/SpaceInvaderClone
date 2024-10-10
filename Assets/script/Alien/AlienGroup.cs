using System.Collections.Generic;
using UnityEngine;

public class AlienGroup : MonoBehaviour
{
    public Vector3 initialPosition;
    public Vector2 baseSpeed;

    [Header("movement")]
    public GameObject maxRight;
    public GameObject maxLeft;
    Vector2 globalSpeed;
    bool goingRight = true;

    [Header("difficulty")]
    public List<Vector2> inRoundDifficultyLevelList = new List<Vector2> {new Vector2(0, 1.5f), new Vector2(0, 3f), new Vector2(1, 6f) };
    int alienN;
    int initialAlienCount;
    int nextInRoundDifficultyLevel;
    bool inRoundMaxDifficulty;

    [Header("spawn")]
    public GameObject alienPrefab;
    public GameObject colPrefab;
    public Vector2 distance;
    float xPos;
    float yPos;
    float scale;
    float width;
    float height;
    GameObject currentAlien;
    GameObject currentCol;
    List<GameObject> colList = new List<GameObject>();

    private void Start()
    {
        globalSpeed = baseSpeed;
        transform.position = initialPosition;
    }


    void Update()
    {
        alienN = alienCount();
        if ( alienN == 0 ) gameObject.SetActive(false);

        //movement
        if (goingRight && (rightColPos() + (scale/2) >= maxRight.transform.position.x) || (!goingRight && (leftColPos() - (scale / 2) <= maxLeft.transform.position.x)))
        {
            goingRight = !goingRight;
            transform.position -= new Vector3(0, globalSpeed.y, 0);
        }
        else
        {
            if (goingRight) transform.position += new Vector3(globalSpeed.x * Time.deltaTime, 0, 0);
            else transform.position -= new Vector3(globalSpeed.x * Time.deltaTime, 0, 0);
        }

        if(alienN <= inRoundDifficultyLevelList[nextInRoundDifficultyLevel][0] && !inRoundMaxDifficulty)
        {
            globalSpeed.x = inRoundDifficultyLevelList[nextInRoundDifficultyLevel][1];
            if(nextInRoundDifficultyLevel != inRoundDifficultyLevelList.Count-1) nextInRoundDifficultyLevel++;
            else inRoundMaxDifficulty = true;
        }
        
    }

    public void spawn(Vector2Int alienGroupSize,Vector2 speed, float alienShootCooldown)
    {
        //clear
        baseSpeed = speed;
        globalSpeed = baseSpeed;
        transform.position = initialPosition;
        goingRight = true;
        yPos = 0;
        scale = transform.localScale.x;
        initialAlienCount = alienGroupSize.x * alienGroupSize.y;
        nextInRoundDifficultyLevel = 0;
        inRoundMaxDifficulty = false;
        colList.Clear();
        for(int i=0; i< transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        //setup in round difficulty trigger depend to the alien number
        inRoundDifficultyLevelList[0] = new Vector2(initialAlienCount / 2, inRoundDifficultyLevelList[0].y);
        inRoundDifficultyLevelList[1] = new Vector2(initialAlienCount / 4, inRoundDifficultyLevelList[1].y);

        //spawn column
        if (alienGroupSize.x == 1) xPos = 0;
        else if (alienGroupSize.x % 2 == 0) xPos = -((distance.x / 2) + (scale / 2) + ((scale + distance.x) * ((alienGroupSize.x / 2) - 1)));
        else xPos = -(scale + distance.x) * ((alienGroupSize.x - 1) / 2);

        for (int i = 0; i < alienGroupSize.x; i++)
        {
            currentCol = Instantiate(colPrefab, transform);
            currentCol.transform.localPosition = new Vector3(xPos, 0, 0);
            currentCol.GetComponent<BoxCollider2D>().size = new Vector2((distance.x+scale)*2, 30);
            currentCol.GetComponent<AlienCol>().shootCooldown = alienShootCooldown;
            colList.Add(currentCol);
            xPos += (scale + distance.x);
        }

        //spawn alien in column
        for (int j = 0; j < alienGroupSize.y; j++)
        {
            foreach (GameObject col in colList)
            {
                currentAlien = Instantiate(alienPrefab, col.transform);
                currentAlien.transform.localPosition = new Vector3(0, yPos, 0);
            }

            yPos -= (scale + distance.y);
        }
    }

    public int alienCount()
    {
        int n = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            n += transform.GetChild(i).childCount;
        }
        return n;
    }

    public float bottomAlienPos()
    {
        if (transform.childCount != 0)
        {
            if (transform.GetChild(0).transform.childCount != 0)
            {
                float minpos = transform.GetChild(0).transform.GetChild(0).transform.position.y;
                for (int colIndex = 1; colIndex < transform.childCount; colIndex++)
                {
                    for (int alienIndex = 1; alienIndex < transform.GetChild(colIndex).childCount; alienIndex++)
                    {
                        if (transform.GetChild(colIndex).transform.GetChild(alienIndex).transform.position.y < minpos) minpos = transform.GetChild(colIndex).transform.GetChild(alienIndex).transform.position.y;
                    }
                }
                return minpos;
            }
        }
        return 3;
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
