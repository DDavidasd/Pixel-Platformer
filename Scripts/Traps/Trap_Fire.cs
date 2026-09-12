using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Fire : Trap
{
    private Animator anim;

    public bool isActive;
    public bool hasSwitcher;
    public float fireCD;

    private void Start()
    {
        anim = GetComponent<Animator>();

        if(!hasSwitcher)
            InvokeRepeating("FireSwitch", 0, fireCD);
    }

    private void Update()
    {
        anim.SetBool("isActive", isActive);
    }

    public void FireSwitch()
    { 
        isActive = !isActive;
    }

    public void FireSwitchCooldown(float sec)
    {
        //FireSwitch();
        CancelInvoke();
        isActive = false;
        Invoke("FireSwitch", sec);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive)
        {
            base.OnTriggerEnter2D(collision);
        }  
    }
}
