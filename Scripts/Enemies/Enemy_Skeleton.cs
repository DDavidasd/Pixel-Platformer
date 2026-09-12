using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    [Header("Move information")]
    [SerializeField] private float speed;
    [SerializeField] private float idleTime = 1.5f;
    private float idleTimeCounter;

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (idleTimeCounter <= 0)
        {
            rb.velocity = new Vector2(speed * facingDirection, rb.velocity.y);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        idleTimeCounter -= Time.deltaTime;

        CollisionChecks();

        if (wallDetected || !groundDetected)
        {
            idleTimeCounter = idleTime;
            Flip();
        }

        anim.SetFloat("xVelocity", rb.velocity.x);
    }
}
