using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public struct HighScore
{
    public int score;
    public string playerName;
    public int level;

    public HighScore(int p1, string p2, int p3)
    {
        score = p1;
        playerName = p2;
        level = p3;
    }
}
