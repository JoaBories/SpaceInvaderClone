using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Effect System")]
    public GameObject gameCamera;

    [Header("Game Objects")]
    public GameObject player;
    public GameObject alienGroup;

    [Header("Shield System")]
    public List<GameObject> shields = new List<GameObject>();
    public GameObject shieldTop;

    [Header("Level System")]
    public GameObject levelDisplay;
    public GameObject nextLevelTextDisplay;
    public List<LevelSpecs> levelSpecsList = new List<LevelSpecs>();
    int maxLevel = 99;
    int levelNumber;


    [Header("Misc")]
    public GameObject startTextDisplay;
    public bool running;
    public bool isBetweenLevel;
    public bool isInStartMenu;

    void Start()
    {
        levelNumber = 1;
        running = false;
        isBetweenLevel = false;
        isInStartMenu = true;
        startTextDisplay.SetActive(true);
        nextLevelTextDisplay.SetActive(false);
    }

    void Update()
    {

        if (running)
        {
            if (alienGroup.GetComponent<AlienGroup>().bottomAlienPos() <= shieldTop.transform.position.y) desactivateShields();
            if (!alienGroup.activeSelf)
            {
                levelNumber++;
                running = false;
                isBetweenLevel = true;
            }
        }
        else if (isBetweenLevel)
        {
            nextLevelTextDisplay.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Return))
            {
                startLevel(levelNumber);
                isBetweenLevel=false;
            }
        }
        else if (isInStartMenu)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                startLevel(levelNumber);
                isInStartMenu = false;
            }
            
            if (Input.GetKeyDown(KeyCode.UpArrow)) if(levelNumber < maxLevel) levelNumber++;
            if (Input.GetKeyDown(KeyCode.DownArrow)) if(levelNumber > 1) levelNumber--;
            
        }

        levelDisplay.GetComponent<Text>().text = levelNumber.ToString();
    }

    void startLevel(int levelNumber)
    {
        nextLevelTextDisplay.SetActive(false);
        startTextDisplay.SetActive(false) ;
        running = true;
        activateShields();
        spawnAlien(levelSpecsList[levelNumber - 1].size, levelSpecsList[levelNumber - 1].speed ,levelSpecsList[levelNumber - 1].shootCooldown);
    }

    void spawnAlien(Vector2Int size, Vector2 speed, float alienShootCooldown)
    {
        alienGroup.SetActive(true);
        alienGroup.GetComponent<AlienGroup>().spawn(size, speed, alienShootCooldown);
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
