using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private Coroutine vibrationHappening;

    [Header("Effect System")]
    public GameObject gameCamera;
    public GameObject globalVolume;
    private LensDistortion lensDistortion;

    [Header("Game Objects")]
    public GameObject player;
    public GameObject alienGroup;
    public GameObject maxLeft;
    public GameObject maxRight;

    [Header("Shield System")]
    public List<GameObject> shields = new List<GameObject>();
    public GameObject shieldTop;
    public GameObject shieldPrefab;
    public float shieldPosy;
    GameObject currentShield;
    bool shieldBool;
    float shieldPos;
    float shieldDistance;

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
    public GameObject pressStartTextDisplay;
    public string state;

    [Header("Score System")]
    public GameObject scoreMultiplicatorDisplay;
    public GameObject inStreakScoreDisplay;
    public GameObject scoreTextDisplay;
    public GameObject timerBar;
    public GameObject secondeTimerBar;
    public GameObject missedTextDisplay;
    public float multiplicatorTime;
    bool streakBool = false;
    bool freazeMultiplicatorTimer = false;
    int inStreakScore;
    int score;
    float endOfMultiplicatorTime;
    float scoreMultiplicator;
    float maxScoreMultiplicator = 5;
    string scoreText;

    [Header("Sound System")]
    public AudioSource soundObject;
    public AudioClip hurtSound;
    public AudioClip startSound;
    public AudioClip finishWaveSound;


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
        levelDisplay.GetComponent<TextMeshProUGUI>().text = "ROUND " + levelNumber.ToString();

        if (scoreMultiplicator.ToString().Length == 3) scoreMultiplicatorDisplay.GetComponent<TextMeshProUGUI>().text = "X" + scoreMultiplicator.ToString()[0].ToString() + "." + scoreMultiplicator.ToString()[2].ToString();
        else scoreMultiplicatorDisplay.GetComponent<TextMeshProUGUI>().text = "X" + scoreMultiplicator.ToString()[0].ToString();
        
        scoreDisplay();
        inStreakScoreDisplayF();

        if (freazeMultiplicatorTimer) endOfMultiplicatorTime += Time.deltaTime;

        if (streakBool)
        {
            timerBar.SetActive(true);
            timerBar.GetComponent<Image>().fillAmount = (endOfMultiplicatorTime - Time.time) / multiplicatorTime;
        }
        else
        {
            timerBar.SetActive(false);
        }

        if (streakBool && endOfMultiplicatorTime - Time.time <= 0) endStreak();

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
                    else if (startTimerTime + 1.5f <= Time.time && startTimerText.GetComponent<TextMeshProUGUI>().text == "1")
                    {
                        canMove = true;
                        canShoot = true;
                        freazeMultiplicatorTimer = false;
                        startTimerText.GetComponent<Animator>().Play("textAppear");
                        startTimerText.GetComponent<TextMeshProUGUI>().text = "GO!";
                        timerBar.GetComponent<Animator>().Play("multiplicatorIdle");
                        if (vibrationHappening == null) StartCoroutine(vibration(1f, 1f, 1f));
                    }
                    else if (startTimerTime + 1 <= Time.time && startTimerText.GetComponent<TextMeshProUGUI>().text == "2")
                    {
                        startTimerText.GetComponent<Animator>().Play("textAppear");
                        startTimerText.GetComponent<TextMeshProUGUI>().text = "1";
                        if (vibrationHappening == null) StartCoroutine(vibration(0.5f, 0.5f, 0.5f));
                    }
                    else if (startTimerTime + 0.5f <= Time.time && startTimerText.GetComponent<TextMeshProUGUI>().text == "3")
                    {
                        startTimerText.GetComponent<Animator>().Play("textAppear");
                        startTimerText.GetComponent<TextMeshProUGUI>().text = "2";
                        if (vibrationHappening == null) StartCoroutine(vibration(0.5f, 0.5f, 0.5f));
                    }
                }

                if (alienGroup.GetComponent<AlienGroup>().bottomAlienPos() <= shieldTop.transform.position.y && shieldBool) desactivateShields();
                if (alienGroup.GetComponent<AlienGroup>().bottomAlienPos() <= earthLevel.transform.position.y) switchState("loose");
                if (!alienGroup.activeSelf)
                {
                    levelNumber++;
                    playSoundClip(finishWaveSound, transform);
                    if (levelNumber > levelSpecsList.Count) switchState("win");
                    else switchState("betweenLevel");
                }
                break;

            case "betweenLevel":
                if (controls.Gameplay.Start.triggered)
                {
                    playSoundClip(startSound, transform);
                    switchState("running");
                    startLevel(levelNumber);
                }
                break;

            case "win":
                alienGroup.SetActive(false);
                if (controls.Gameplay.Start.triggered)
                {
                    playSoundClip(startSound, transform);
                    switchState("running");
                    restart();
                }
                break;

            case "loose":
                alienGroup.SetActive(false);
                if (controls.Gameplay.Start.triggered)
                {
                    playSoundClip(startSound, transform);
                    switchState("running");
                    restart();
                }
                break;

            case "startMenu":
                if (controls.Gameplay.Start.triggered)
                {
                    playSoundClip(startSound, transform);
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
                pressStartTextDisplay.SetActive(false);
                nextLevelTextDisplay.SetActive(false);
                startTextDisplay.SetActive(false);
                restartTextDisplay.SetActive(false);
                startTimerText.SetActive(true);
                startTimerTime = Time.time;
                endTimer = false;
                startTimerText.GetComponent<Animator>().Play("textAppear");
                startTimerText.GetComponent<TextMeshProUGUI>().text = "3";
                if (vibrationHappening == null) StartCoroutine(vibration(0.5f, 0.5f, 0.5f));
                break;
            case "betweenLevel":
                score += (int) Math.Round(inStreakScore * scoreMultiplicator);
                inStreakScore = 0;
                canMove = false;
                canShoot = false;
                getLensDistorsionTo(0.4f, 0.1f);
                timerBar.GetComponent<Animator>().Play("multiplicatorIdleCalm");
                nextLevelTextDisplay.SetActive(true);
                pressStartTextDisplay.SetActive(true);
                freazeMultiplicatorTimer = true;
                break;
            case "startMenu":
                getLensDistorsionTo(0.4f, 0.1f);
                startTextDisplay.SetActive(true);
                pressStartTextDisplay.SetActive(true);
                break;
            case "win":
                getLensDistorsionTo(0.4f, 0.1f);
                restartTextDisplay.SetActive(true);
                pressStartTextDisplay.SetActive(true);
                endStreak();
                break;
            case "loose":
                getLensDistorsionTo(0.4f, 0.1f);
                restartTextDisplay.SetActive(true);
                pressStartTextDisplay.SetActive(true);
                endStreak();
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
        scoreTextDisplay.GetComponent<TextMeshProUGUI>().text = scoreText + score.ToString();
    }
    
    void inStreakScoreDisplayF()
    {
        if (inStreakScore < 10) scoreText = "00000";
        else if (inStreakScore < 100) scoreText = "0000";
        else if (inStreakScore < 1000) scoreText = "000";
        else if (inStreakScore < 10000) scoreText = "00";
        else if (inStreakScore < 100000) scoreText = "0";
        else if (inStreakScore < 1000000) scoreText = "";
        else scoreText = "bro how did you do that ? ";
        inStreakScoreDisplay.GetComponent<TextMeshProUGUI>().text = scoreText + inStreakScore.ToString();
    }

    void endStreak()
    {
        score += (int)Math.Round(inStreakScore * scoreMultiplicator);
        scoreMultiplicator = 1;
        inStreakScore = 0;
        endOfMultiplicatorTime = 0;
        streakBool = false;
    }

    void restart()
    {
        GameController.Instance.score = 0;
        levelNumber = 1;
        startLevel(levelNumber);
    }

    void startLevel(int levelNumber)
    {
        activateShields(levelSpecsList[levelNumber - 1].shieldNumber);
        player.GetComponent<Animator>().Play("playerSpawn");
        spawnAlien(levelSpecsList[levelNumber - 1].size, levelSpecsList[levelNumber - 1].speed ,levelSpecsList[levelNumber - 1].shootCooldown);
    }

    void spawnAlien(Vector2Int size, Vector2 speed, float alienShootCooldown)
    {
        alienGroup.SetActive(true);
        alienGroup.GetComponent<AlienGroup>().spawn(size, speed, alienShootCooldown);
    }

    public void shot()
    {
        scoreMultiplicatorIncrement(0.1f);
        if (scoreMultiplicator > maxScoreMultiplicator) scoreMultiplicator = maxScoreMultiplicator;
        inStreakScore += 10;
        if(vibrationHappening == null) vibrationHappening = StartCoroutine(vibration(0.2f, 0.5f, 0.5f));
        endOfMultiplicatorTime = Time.time + multiplicatorTime;
    }

    public void longShot()
    {
        scoreMultiplicatorIncrement(0.2f);
        if (scoreMultiplicator > maxScoreMultiplicator) scoreMultiplicator = maxScoreMultiplicator;
        inStreakScore += 50;
        if (vibrationHappening == null) vibrationHappening = StartCoroutine(vibration(0.2f, 0.5f, 1f));
        endOfMultiplicatorTime = Time.time + multiplicatorTime;
    }

    public void failedShot()
    {
        if(scoreMultiplicator > 1)
        {
            endStreak();
            StartCoroutine(missedDisplay());
        }
        
    }

    IEnumerator missedDisplay()
    {
        missedTextDisplay.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        missedTextDisplay.SetActive(false);
    }

    public void hitPlayer()
    {
        if (!player.GetComponent<player>().invincibility)
        {
            playSoundClip(hurtSound, transform);
            endStreak();
            player.gameObject.GetComponent<Animator>().Play("playerHit");
            StopAllCoroutines();
            vibrationHappening = StartCoroutine(vibration(0.2f, 0.2f, 2f));
        }
    }

    void scoreMultiplicatorIncrement(float increment)
    {
        streakBool = true;
        scoreMultiplicator = (float) Math.Round(increment + scoreMultiplicator, 2);

        if ((scoreMultiplicator*10)%10 == 0)
        {
            timerBar.GetComponent<Animator>().Play("multiplicatorNewInt");
        }
    }

    public void playSoundClip(AudioClip audioClip, Transform spawnTransform, float volume = 1f)
    {
        AudioSource audioSource = Instantiate(soundObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);

    }



    void desactivateShields()
    {
        if (shields.Count > 0)
        {
            if (shields[0].activeSelf && vibrationHappening == null) vibrationHappening = StartCoroutine(vibration(0.2f, 1f, 1f));
            int shieldcount = shields.Count;
            GameObject shield;
            for (int i = 0; i < shieldcount; i++)
            {
                shield = shields[0];
                shields.RemoveAt(0);
                Destroy(shield);
            }
        }
        shields.Clear();
        shieldBool = false;
    }

    void activateShields(int shieldNumber)
    {
        if(shieldBool) desactivateShields();

        shieldBool = true;
        if (shieldNumber == 1) shieldPos = 0;
        shieldDistance = (maxRight.transform.position.x - maxLeft.transform.position.x)/ (shieldNumber+1);
        shieldPos = maxLeft.transform.position.x + shieldDistance;

        for (int i = 0; i < shieldNumber; i++)
        {
            currentShield = Instantiate(shieldPrefab, transform);
            currentShield.transform.localPosition = new Vector3(shieldPos, shieldPosy, 0);
            shields.Add(currentShield);
            shieldPos += shieldDistance;
        }
    }
}
