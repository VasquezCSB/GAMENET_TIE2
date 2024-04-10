using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CloudType
{
    right,
    left
}
public class CloudPlatform : MonoBehaviour
{
    public float speed = 2f;
    public float distance = 10f;
    public CloudType cloudType;

    private Vector3 startPos;
    private bool movingRight = true;
    private bool movingLeft = true;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (cloudType == CloudType.right)
        {
            MoveRight();
        } else if(cloudType == CloudType.left)
        {
            MoveLeft();
        }
    }

    public void MoveRight()
    {
        if (movingRight)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        if (Vector3.Distance(startPos, transform.position) >= distance)
        {
            movingRight = !movingRight;
        }
    }

    public void MoveLeft()
    {
        if (movingLeft)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }

        if (Vector3.Distance(startPos, transform.position) >= distance)
        {
            movingLeft = !movingLeft;
        }
    }
}
