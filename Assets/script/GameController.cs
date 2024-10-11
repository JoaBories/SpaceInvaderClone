using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
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
    public GameObject startTimerText;
    public List<LevelSpecs> levelSpecsList = new List<LevelSpecs>();
    bool endTimer;
    float startTimerTime;
    int levelNumber;
    public bool canShoot;
    public bool canMove;


    [Header("Game State System")]
    public GameObject nextLevelTextDisplay;
    public GameObject startTextDisplay;
    public GameObject restartTextDisplay;
    public GameObject earthLevel;
    public string state;

    [Header("Score System")]
    public GameObject scoreMultiplicatorDisplay;
    public GameObject intMultiplicatorDisplay;
    public GameObject floatMultiplicatorDisplay;
    public GameObject scoreTextDisplay;
    public int score;
    public float scoreMultiplicator;
    float maxScoreMultiplicator = 10;
    string scoreText;

    private Coroutine vibrationHappening;

    void Start()
    {
        globalVolume.GetComponent<Volume>().profile.TryGet(out lensDistortion);
        Instance = this;
        levelNumber = 1;
        score = 0;
        switchState("startMenu");
        controls = new Controls();
        controls.Gameplay.Enable();
        canMove = false;
        canShoot = false;
        scoreMultiplicator = 1f;
    }

    void Update()
    {
        levelDisplay.GetComponent<Text>().text = levelNumber.ToString();

        if (scoreMultiplicator.ToString().Length == 3)
        {
            intMultiplicatorDisplay.GetComponent<Text>().text = scoreMultiplicator.ToString()[0].ToString();
            floatMultiplicatorDisplay.GetComponent<Text>().text = "." + scoreMultiplicator.ToString()[2].ToString();
        } 
        else
        {
            intMultiplicatorDisplay.GetComponent<Text>().text = scoreMultiplicator.ToString()[0].ToString();
            floatMultiplicatorDisplay.GetComponent<Text>().text = "";
        }
        scoreDisplay();

        if (scoreMultiplicator <= 1f) scoreMultiplicatorDisplay.SetActive(false);
        else scoreMultiplicatorDisplay.SetActive(true);

        switch (state)
        {
            case "running":
                if (!endTimer)
                {
                    if (startTimerTime + 2f <= Time.time)
                    {
                        startTimerText.SetActive(false);
                        endTimer = true;
                    }
                    else if (startTimerTime + 1.5f <= Time.time && startTimerText.GetComponent<Text>().text == "1")
                    {
                        canMove = true;
                        canShoot = true;
                        startTimerText.GetComponent<Animator>().Play("textAppear");
                        startTimerText.GetComponent<Text>().text = "GO!";
                        if(vibrationHappening == null) StartCoroutine(vibration(0.2f, 1f, 1f));
                    }
                    else if (startTimerTime + 1 <= Time.time && startTimerText.GetComponent<Text>().text == "2")
                    {
                        startTimerText.GetComponent<Animator>().Play("textAppear");
                        startTimerText.GetComponent<Text>().text = "1";
                        if (vibrationHappening == null) StartCoroutine(vibration(0.2f, 0, 0.5f));
                    }
                    else if (startTimerTime + 0.5f <= Time.time && startTimerText.GetComponent<Text>().text == "3")
                    {
                        startTimerText.GetComponent<Animator>().Play("textAppear");
                        startTimerText.GetComponent<Text>().text = "2";
                        if (vibrationHappening == null) StartCoroutine(vibration(0.2f, 0.5f, 0));
                    }
                }

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
                alienGroup.SetActive(false);
                if (controls.Gameplay.Start.triggered)
                {
                    switchState("running");
                    restart();
                }
                break;

            case "loose":
                alienGroup.SetActive(false);
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
                getLensDistorsionTo(0.2f, 1f);
                nextLevelTextDisplay.SetActive(false);
                startTextDisplay.SetActive(false);
                restartTextDisplay.SetActive(false);
                startTimerText.SetActive(true);
                startTimerTime = Time.time;
                endTimer = false;
                startTimerText.GetComponent<Animator>().Play("textAppear");
                startTimerText.GetComponent<Text>().text = "3";
                if (vibrationHappening == null) StartCoroutine(vibration(0.2f, 0, 0.5f));
                break;
            case "betweenLevel":
                canMove = false;
                canShoot = false;
                getLensDistorsionTo(0.4f, 0.1f);
                nextLevelTextDisplay.SetActive(true);
                break;
            case "startMenu":
                getLensDistorsionTo(0.4f, 0.1f);
                startTextDisplay.SetActive(true);
                break;
            case "win":
                getLensDistorsionTo(0.4f, 0.1f);
                restartTextDisplay.SetActive(true);
                break;
            case "loose":
                getLensDistorsionTo(0.4f, 0.1f);
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

    public IEnumerator vibration(float time, float highFrequency, float lowFrequency)
    {
        Gamepad.current.SetMotorSpeeds(highFrequency, lowFrequency);
        yield return new WaitForSeconds(time);
        Gamepad.current.SetMotorSpeeds(0, 0);
        vibrationHappening = null;
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
        player.GetComponent<Animator>().Play("playerSpawn");
        spawnAlien(levelSpecsList[levelNumber - 1].size, levelSpecsList[levelNumber - 1].speed ,levelSpecsList[levelNumber - 1].shootCooldown);
    }

    void spawnAlien(Vector2Int size, Vector2 speed, float alienShootCooldown)
    {
        alienGroup.SetActive(true);
        alienGroup.GetComponent<AlienGroup>().spawn(size, speed, alienShootCooldown);
    }

    public void killedAlien()
    {
        scoreMultiplicatorIncrement(0.1f);
        if (scoreMultiplicator > maxScoreMultiplicator) scoreMultiplicator = maxScoreMultiplicator;
        score += 10;
        if(vibrationHappening == null) vibrationHappening = StartCoroutine(vibration(0.2f, 0.5f, 0.5f));
    }

    public void hitPlayer()
    {
        scoreMultiplicator = 1;
        if (!player.GetComponent<player>().invincibility)
        {
            player.gameObject.GetComponent<Animator>().Play("playerHit");
            StopAllCoroutines();
            vibrationHappening = StartCoroutine(vibration(2f, 0.2f, 0.2f));
        }
    }

    void scoreMultiplicatorIncrement(float increment)
    {
        scoreMultiplicator = (float) Math.Round(increment + scoreMultiplicator, 2);
        Debug.Log(scoreMultiplicator * 10 + " et entier : " + (scoreMultiplicator * 10) % 10);

        if ((scoreMultiplicator*10)%10 == 0)
        {
            scoreMultiplicatorDisplay.GetComponent<Animator>().Play("multiplicatorNewInt");
        }
    }

    void desactivateShields()
    {
        if (shields[0].activeSelf) if (vibrationHappening == null) vibrationHappening = StartCoroutine(vibration(0.2f, 1f, 1f));
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
