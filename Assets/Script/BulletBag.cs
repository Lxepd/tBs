using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBag : MonoBehaviour
{
    private float moveTime = 2f;
    private float moveSpeed = .02f;
    private int dir = 1;

    float currentTime = 0;

    public int id;

    private Vector2 startPos;
    private void Start()
    {
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        currentTime += .1f;

        transform.position = new Vector2(transform.position.x, transform.position.y + dir * moveSpeed);
        if(currentTime >= moveTime)
        {
            dir *= -1;
            currentTime = 0;
        }
    }

    public void InitPos()
    {
        transform.position = startPos;
        currentTime = 0;
        dir = 1;
    }
}
