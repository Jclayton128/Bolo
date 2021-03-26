using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonAttack : Attack
{
    //init

    //param
    float timeBetweenAttacks = 1.0f;
    float weaponSpeed = 10f;
    float weaponLifetime = .75f;
    float offset = .5f;

    //hood
    float timeSinceLastAttack = 0;
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastAttack -= Time.deltaTime;
    }

    public override void LMBDown()
    {
        if (timeSinceLastAttack < 0)
        {
            AudioSource.PlayClipAtPoint(selectedFiringSound, transform.position);
            GameObject shell = Instantiate(projectilePrefab, transform.position + (transform.up * offset), transform.rotation) as GameObject;
            shell.GetComponent<Rigidbody2D>().velocity = shell.transform.up * weaponSpeed;
            Destroy(shell, weaponLifetime);


            timeSinceLastAttack = timeBetweenAttacks;
        }

    }

    public override void LMBUp()
    {
        
    }

    public override void RMBDown()
    {
        
    }

    public override void RMBUp()
    {
        
    }



}
