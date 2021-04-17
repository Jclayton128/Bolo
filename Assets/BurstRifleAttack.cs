using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstRifleAttack : Attack
{
    //init

    //param
    float timeBetweenVolleys = 1.3f;
    float timeBetweenShots = 0.2f;
    float weaponSpeed = 10f;
    float weaponLifetime = .25f;
    float weaponDamage = .3f;
    float offset = .5f;

    //hood
    float timeSinceLastAttack = 0;
    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        timeSinceLastAttack -= Time.deltaTime;
    }

    public override void AttackCommence()
    {
        if (timeSinceLastAttack < 0)
        {
            Invoke("CreatePartOfBurstAttack", timeBetweenShots);
            Invoke("CreatePartOfBurstAttack", timeBetweenShots * 2);
            Invoke("CreatePartOfBurstAttack", timeBetweenShots * 3);

            timeSinceLastAttack = timeBetweenVolleys;
        }
    }

    private void CreatePartOfBurstAttack()
    {
        AudioSource.PlayClipAtPoint(selectedFiringSound, transform.position);
        GameObject shell = Instantiate(projectilePrefab, transform.position + (transform.up * offset), transform.rotation) as GameObject;
        shell.GetComponent<Rigidbody2D>().velocity = shell.transform.up * weaponSpeed;
        DamageDealer dd = shell.GetComponent<DamageDealer>();
        dd.SetAttackSource(transform.root.gameObject);
        dd.SetDamage(weaponDamage);

        Destroy(shell, weaponLifetime);
    }

    public override void AttackRelease()
    {
        throw new System.NotImplementedException();
    }

    public override float GetAttackRange()
    {
        float range = weaponLifetime * weaponSpeed;
        return range;
    }
}
