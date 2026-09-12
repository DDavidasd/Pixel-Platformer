using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Saw_Extended : Trap
{
    private Animator anim;
    [SerializeField] private Transform[] movePoint;
    [SerializeField] private float speed;

    private int movePointIndex;
    private bool moveForward = true;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("isActive", true);
        transform.position = movePoint[0].position;
    }


    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint[movePointIndex].position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, movePoint[movePointIndex].position) < 0.15f)
        {
            if(movePointIndex == 0)
                moveForward = true;

            if(moveForward)
                movePointIndex++;
            else
                movePointIndex--;

            if (movePointIndex >= movePoint.Length)
            {
                movePointIndex = movePoint.Length - 1;
                moveForward = false;
            }
        }
    }
}
