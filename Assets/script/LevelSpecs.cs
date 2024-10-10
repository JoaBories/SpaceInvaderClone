using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct LevelSpecs
{
    public Vector2Int size;
    public Vector2 speed;
    public float shootCooldown;

    public LevelSpecs(Vector2Int p1, Vector2 p2, float p3)
    {
        size = p1;
        speed = p2;
        shootCooldown = p3;
    }
}
