using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    Controls controls;
    bool startPressed;

    [Header("Effect System")]
    public GameObject gameCamera;
    public GameObject globalVolume;
    private LensDistortion lensDistortion;

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
        globalVolume.GetComponent<Volume>().profile.TryGet(out lensDistortion);
        Instance = this;
        levelNumber = 1;
        score = 0;
        switchState("startMenu");
        controls = new Controls();
        controls.Gameplay.Enable();
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
                if (controls.Gameplay.Start.triggered)
                {
                    switchState("running");
                    startLevel(levelNumber);
                }
                break;

            case "win":
                if (controls.Gameplay.Start.triggered)
                {
                    switchState("running");
                    restart();
                }
                break;

            case "loose":
                if (controls.Gameplay.Start.triggered)
                {
                    switchState("running");
                    restart();
                }
                break;

            case "startMenu":
                if (controls.Gameplay.Start.triggered)
                {
                    switchState("running");
                    startLevel(levelNumber);
                }
                break;
        }
    }

    void switchState(string nexState)
    {
        state = nexState;
        switch (nexState)
        {
            case "running":
                getLensDistorsionTo(0.1f, 0.1f);
                nextLevelTextDisplay.SetActive(false);
                startTextDisplay.SetActive(false);
                restartTextDisplay.SetActive(false);
                break;
            case "betweenLevel":
                getLensDistorsionTo(0.3f, 0.1f);
                nextLevelTextDisplay.SetActive(true);
                break;
            case "startMenu":
                getLensDistorsionTo(0.3f, 0.1f);
                startTextDisplay.SetActive(true);
                break;
            case "win":
                getLensDistorsionTo(0.3f, 0.1f);
                restartTextDisplay.SetActive(true);
                break;
            case "loose":
                getLensDistorsionTo(0.3f, 0.1f);
                restartTextDisplay.SetActive(true);
                break;
        }
    }

    void getLensDistorsionTo(float goal, float time)
    {
        StartCoroutine(getLensDistorsionToCoroutine(goal, Math.Abs(goal - lensDistortion.intensity.value) / 100, time / 100));
    }

    IEnumerator getLensDistorsionToCoroutine(float goal, float step, float timeInterval)
    {
        while (Math.Abs(lensDistortion.intensity.value - goal) >= step*10)
        {
            if(goal > lensDistortion.intensity.value) lensDistortion.intensity.value += step;
            else lensDistortion.intensity.value -= step;
            yield return new WaitForSeconds(timeInterval);
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

    void restart()
    {
        GameController.Instance.score = 0;
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
