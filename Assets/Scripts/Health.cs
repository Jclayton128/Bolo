using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Health : MonoBehaviour
{
    //init
    [SerializeField] AudioClip[] hurtAudioClips = null;
    [SerializeField] AudioClip[] dieAudioClips = null;
    AudioClip chosenHurtSound;
    AudioClip chosenDieSound;
    Rigidbody2D rb;

    //param
    public float startingHealth = 1;
    public float armorLevel = 0;
    public bool canMove = false;

    //hood
    bool isDying = false;
    public float currentHealth;
    GameObject ownerOfLastDamageDealerToBeHitBy;

    void Start()
    {
        currentHealth = startingHealth;
        if (canMove)
        {
            rb = transform.root.GetComponent<Rigidbody2D>();
        }
        SelectDieSound();
    }

    private void SelectDieSound()
    {
        if (dieAudioClips.Length == 0) { return; }
        int selectedSound = UnityEngine.Random.Range(0, dieAudioClips.Length);
        chosenDieSound = dieAudioClips[selectedSound];
    }

    // Update is called once per frame
    void Update()
    {
        LiveOrDie();
    }

    private void LiveOrDie()
    {
        if (currentHealth <= 0)
        {
            isDying = true;
            if (chosenDieSound)
            {
                AudioSource.PlayClipAtPoint(chosenDieSound, transform.position);
            }
            Destroy(transform.root.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject otherGO = other.gameObject;
        HandleDamage(otherGO);
    }

    private void HandleDamage(GameObject other)
    {
        DamageDealer dd = other.GetComponent<DamageDealer>();
        if (!dd) { return; }
        if (dd.GetAttackSource())
        {
            ownerOfLastDamageDealerToBeHitBy = dd.GetAttackSource();
        }
        if (dd.GetKnockBackAmount() != 0)
        {
            rb.AddForce(dd.GetKnockBackAmount() * dd.GetComponent<Rigidbody2D>().velocity.normalized, ForceMode2D.Impulse);
        }

        float incomingDamage = dd.GetDamage();
        if (incomingDamage == 0) { return; }

        ModifyHealth(incomingDamage * -1);
        Destroy(other);

    }

    public void ModifyHealth(float amount)
    {
        SelectHurtSound();
        if (chosenHurtSound)
        {
            AudioSource.PlayClipAtPoint(chosenHurtSound, transform.position);
        }

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, -1, startingHealth);
    }

    private void SelectHurtSound()
    {
        if (hurtAudioClips.Length == 0) { return; }
        int selectedSound = UnityEngine.Random.Range(0, hurtAudioClips.Length);
        chosenHurtSound = hurtAudioClips[selectedSound];
    }
}

