using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    
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
    public List<LevelSpecs> levelSpecsList = new List<LevelSpecs>();
    int levelNumber;

    [Header("Game State System")]
    public GameObject nextLevelTextDisplay;
    public GameObject startTextDisplay;
    public GameObject restartTextDisplay;
    public GameObject earthLevel;
    public string state;

    [Header("Score System")]
    public GameObject scoreTextDisplay;
    public int score;
    string scoreText;

    void Start()
    {
        Instance = this;
        levelNumber = 1;
        score = 0;
        switchState("startMenu");
    }

    void Update()
    {
        levelDisplay.GetComponent<Text>().text = levelNumber.ToString();
        scoreDisplay();

        switch (state)
        {
            case "running":
                if (alienGroup.GetComponent<AlienGroup>().bottomAlienPos() <= shieldTop.transform.position.y) desactivateShields();
                if (alienGroup.GetComponent<AlienGroup>().bottomAlienPos() <= earthLevel.transform.position.y) switchState("loose");
                if (!alienGroup.activeSelf)
                {
                    levelNumber++;
                    if (levelNumber > levelSpecsList.Count) switchState("win");
                    else switchState("betweenLevel");
                }
                break;

            case "betweenLevel":
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    switchState("running");
                    startLevel(levelNumber);
                }
                break;

            case "win":
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    switchState("running");
                    restart();
                }
                break;

            case "loose":
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    switchState("running");
                    restart();
                }
                break;

            case "startMenu":
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    switchState("running");
                    startLevel(levelNumber);
                }
                break;
        }

    }

    void scoreDisplay()
    {
        if (score < 10) scoreText = "00000";
        else if (score < 100) scoreText = "0000";
        else if (score < 1000) scoreText = "000";
        else if (score < 10000) scoreText = "00";
        else if (score < 100000) scoreText = "0";
        else if (score < 1000000) scoreText = "";
        else scoreText = "bro how did you do that ? ";
        scoreTextDisplay.GetComponent<Text>().text = scoreText + score.ToString();
    }
    void switchState(string nexState)
    {
        state = nexState;
        switch (nexState)
        {
            case "running":
                nextLevelTextDisplay.SetActive(false);
                startTextDisplay.SetActive(false);
                restartTextDisplay.SetActive(false);
                break;
            case "betweenLevel":
                nextLevelTextDisplay.SetActive(true);
                break;
            case "startMenu":
                startTextDisplay.SetActive(true);
                break;
            case "win":
                restartTextDisplay.SetActive(true);
                break;
            case "loose":
                restartTextDisplay.SetActive(true);
                break;
        }
    }

    void restart()
    {
        Score.Instance.score = 0;
        levelNumber = 1;
        startLevel(levelNumber);
    }

    void startLevel(int levelNumber)
    {
        activateShields();
        player.GetComponent<Animator>().Play("playerInvincibility");
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
