using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [Header("Score System")]
    public int score;
    string scoreText;
    public static Score Instance;

    void Start()
    {
        Instance = this;
        score = 0;
    }

    void Update()
    {
        if (score < 10) scoreText = "00000";
        else if (score < 100) scoreText = "0000";
        else if (score < 1000) scoreText = "000";
        else if (score < 10000) scoreText = "00";
        else if (score < 100000) scoreText = "0";
        else if (score < 1000000) scoreText = "";
        else scoreText = "bro what did you do ? ";
        gameObject.GetComponent<Text>().text = scoreText + score.ToString();
    }
}
