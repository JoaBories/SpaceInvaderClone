using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct ListWrapper<T>
{
    public List<T> list;
}
public class AlienGroupY : MonoBehaviour
{
    public GameObject groupXPrefab;

    public GameObject maxRight;
    public GameObject maxLeft;

    public float speed;
    public int groupXNumber;
    public float groupXDistance;
    public List<int> initiationList;
    public float alienDistance;
    
    float scale;
    float width;
    float downDistance = 0.5f;
    float firstgroupXPos;
    float groupXPos;
    bool right = true;
    GameObject currentGroupX;


    void Start()
    {
        scale = transform.localScale.x;
        width = (initiationList.Max() - 1) * (scale + alienDistance*scale) + scale;

        firstgroupXPos = 0;
        groupXPos = firstgroupXPos;
        foreach (int alienNumber in initiationList)
        {
            currentGroupX = Instantiate(groupXPrefab, transform);
            currentGroupX.transform.localPosition = new Vector3(0, groupXPos, 0);
            currentGroupX.GetComponent<AlienGroupX>().alienNumber = alienNumber;
            currentGroupX.GetComponent<AlienGroupX>().alienDistance = alienDistance;
            groupXPos -= (1 + groupXDistance);
        }
    }

    void Update()
    {
        if ((transform.position.x + width / 2 >= maxRight.transform.position.x && right) || (transform.position.x - width / 2 <= maxLeft.transform.position.x && !right))
        {
            if (right) right = false;
            else right = true;
            transform.position -= new Vector3(0, downDistance, 0);
        }
        else
        {
            if (right) transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            else transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        }

    }
}
