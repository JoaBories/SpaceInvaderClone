using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject player;
    GameObject alienGroup;

    [Header("Shield System")]
    public List<GameObject> shields = new List<GameObject>();
    public GameObject shieldTop;

    [Header("Level System")]
    public GameObject levelDisplay;
    public GameObject alienGroupPrefab;
    public GameObject alienGroupPlacement;
    int maxLevel = 10;
    int levelNumber;

    [Header("Misc")]
    public bool running;


    void Start()
    {
        levelNumber = 1;
        running = false;
    }

    void Update()
    {


        if (running)
        {
            if (alienGroup.GetComponent<AlienGroup>().bottomAlienPos() <= shieldTop.transform.position.y) desactivateShields();
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.Backspace)) startLevel(levelNumber);
            
            if (Input.GetKeyDown(KeyCode.UpArrow)) if(levelNumber < maxLevel) levelNumber++;
            if (Input.GetKeyDown(KeyCode.DownArrow)) if(levelNumber > 1) levelNumber--;
            
            levelDisplay.GetComponent<Text>().text = levelNumber.ToString();
        }
    }


    void startLevel(int levelNumber)
    {
        running = true;
        activateShields();
        spawnAlien(6, 6, 1f, 0.5f);
    }

    void spawnAlien(int colNumber, int lignNumber, float xDistance, float yDistance)
    {
        alienGroup = Instantiate(alienGroupPrefab, alienGroupPlacement.transform.position, Quaternion.identity);
        alienGroup.GetComponent<AlienGroup>().spawn(colNumber, lignNumber, xDistance, yDistance);
    }

    void desactivateShields()
    {
        foreach (GameObject shield in shields)
        {
            shield.SetActive(false);
        }
    } 

    void activateShields()
    {
        foreach(GameObject shield in shields)
        {
            shield.SetActive(true);
            for (int i = 0; i < shield.transform.childCount; i++)
            {
                shield.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}
