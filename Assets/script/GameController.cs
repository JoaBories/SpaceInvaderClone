using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject shieldPrefab;
    public List<GameObject> shieldEmplacements = new List<GameObject>();
    List<GameObject> shields = new List<GameObject>();
    GameObject currentShield;

    void Start()
    {
        spawnShields();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawnShields()
    {
        foreach(GameObject shieldEmplacement in shieldEmplacements)
        {
            currentShield = Instantiate(shieldPrefab, transform);
            currentShield.transform.position = shieldEmplacement.transform.position;
            shields.Add(currentShield);
        }
    }
}
