using System;
using UnityEngine;

[Serializable]
public struct LevelSpecs
{
    public Vector2Int size;
    public Vector2 speed;
    public float shootCooldown;
    public int shieldNumber;
    public Vector2 shieldSize;

    public LevelSpecs(Vector2Int p1, Vector2 p2, float p3, int p4, Vector2 p5)
    {
        size = p1;
        speed = p2;
        shootCooldown = p3;
        shieldNumber = p4;
        shieldSize = p5;
    }
}
