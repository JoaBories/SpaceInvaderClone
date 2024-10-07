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

    float width;
    float downDistance = 0.5f;
    double firstgroupXPos;
    double groupXPos;
    bool right = true;
    GameObject currentGroupX;


    void Start()
    {
        width = (initiationList.Max() - 1) * (0.8f + 0.5f) + 0.8f;

        firstgroupXPos = 0;
        groupXPos = firstgroupXPos;
        foreach (int alienNumber in initiationList)
        {
            currentGroupX = Instantiate(groupXPrefab, transform);
            currentGroupX.transform.localPosition = new Vector3(0, (float)groupXPos, 0);
            currentGroupX.GetComponent<AlienGroupX>().alienNumber = alienNumber;
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
