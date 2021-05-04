using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonAttack : Attack
{
    //init


    //param
    float timeBetweenAttacks = 0.3f;
    float weaponSpeed = 10f;
    float weaponLifetime = .75f;
    float weaponDamage = 1f;
    float offset = .5f;
    float energyCost = 40f;

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

    public override void AttackCommence()
    {

        if (timeSinceLastAttack < 0 && energy.GetCurrentEnergy() >= energyCost)
        {
            sh.SpikeLoudnessDueToAttack();
            AudioSource.PlayClipAtPoint(selectedFiringSound, transform.position);
            GameObject shell = Instantiate(projectilePrefab, transform.position + (transform.up * offset), transform.rotation) as GameObject;
            shell.GetComponent<Rigidbody2D>().velocity = shell.transform.up * weaponSpeed;
            DamageDealer dd = shell.GetComponent<DamageDealer>();
            dd.SetAttackSource(transform.root.gameObject);
            dd.SetDamage(weaponDamage);

            Destroy(shell, weaponLifetime);

            timeSinceLastAttack = timeBetweenAttacks;
            energy.ModifyCurrentEnergy(-1 * energyCost);
        }

    }

    public override void AttackRelease()
    {
        
    }

    public override float GetAttackRange()
    {
        float range = weaponLifetime * weaponSpeed;
        return range;
    }


}
