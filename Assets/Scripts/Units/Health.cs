using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Health : MonoBehaviour
{
    //init
    [SerializeField] AudioClip[] hurtAudioClips = null;
    [SerializeField] AudioClip[] dieAudioClips = null;
    [SerializeField] Sprite[] spritesByHealth = null;
    UIManager uim;

    SpriteRenderer sr;
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
    Slider healthBar;

    void Start()
    {
        uim = FindObjectOfType<UIManager>();
        healthBar = uim.healthBar;
        sr = transform.root.GetComponentInChildren<SpriteRenderer>();
        currentHealth = startingHealth;
        UpdateHealthBar();
        if (canMove)
        {
            rb = transform.root.GetComponentInChildren<Rigidbody2D>();
        }
        SelectDieSound();
    }

    private void UpdateHealthBar()
    {
        healthBar.maxValue = startingHealth;
        healthBar.minValue = 0;
        healthBar.value = currentHealth;
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
            BroadcastMessage("DyingActions");
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
        //Destroy(other);

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
        UpdateHealthBar();
        AdjustSpriteToHealthLevel();
    }

    private void AdjustSpriteToHealthLevel()
    {
        if (spritesByHealth.Length == 0) { return; }
        if (currentHealth <= startingHealth * .66f)
        {
            if (currentHealth <= startingHealth * .33f)
            {
                sr.sprite = spritesByHealth[2];
            }
            else
            {
                sr.sprite = spritesByHealth[1];
            }
        }
    }

    private void SelectHurtSound()
    {
        if (hurtAudioClips.Length == 0) { return; }
        int selectedSound = UnityEngine.Random.Range(0, hurtAudioClips.Length);
        chosenHurtSound = hurtAudioClips[selectedSound];
    }
}

