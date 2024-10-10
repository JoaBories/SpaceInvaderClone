using UnityEngine;
using UnityEngine.InputSystem;

public class player : MonoBehaviour
{
    Controls controls;
    bool movingRight;
    bool movingLeft;

    public GameObject MaxLeft;
    public GameObject MaxRight;
    public GameObject bulletPrefab;

    public float speed;
    public bool canMove = true;
    public bool invincibility = false;

    GameObject currentBullet;

    void Start()
    {
        currentBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        currentBullet.SetActive(false);
        controls = new Controls();
        controls.Gameplay.Enable();
    }

    void Update()
    {
        if (canMove)
        {
            if (controls.Gameplay.Right.inProgress)
            {
                if (transform.position.x < MaxRight.transform.position.x)
                {
                    transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
                }
            }
            if (controls.Gameplay.Left.inProgress)
            {
                if (transform.position.x > MaxLeft.transform.position.x)
                {
                    transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
                }
            }
            if (controls.Gameplay.Shoot.triggered)
            {
                if (!currentBullet.activeSelf)
                {
                    Destroy(currentBullet);
                    currentBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                }
            }
        }
    }

    public void respawn()
    {
        transform.position = new Vector3(0, -4, 0); 
        canMove = true;
        invincibility = true;
    }

    public void removeInvincibility()
    {
        invincibility = false;
    }

    
}
