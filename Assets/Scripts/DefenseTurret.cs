using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(IFF))]
public class DefenseTurret : MonoBehaviour
{
    //init
    [SerializeField] GameObject weaponPrefab = null;
    [SerializeField] AudioClip[] firingSounds = null;
    UnitTracker ut;
    List<GameObject> targets = new List<GameObject>();

    //param
    public float timeBetweenShots = .25f;
    public float weaponSpeed = 20f;
    public float weaponLifetime = .45f;
    public float weaponDamage = .5f;
    public float searchRange = 10f;
    float bulletOffset = .4f;

    //hood
    int ownIFF;
    float timeSinceLastShot = 0;
    float attackRange;
    AudioClip selectedFiringSound;
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        ut = GameObject.FindObjectOfType<UnitTracker>();
        ut.AddUnitToTargetableList(gameObject);
        ownIFF = GetComponent<IFF>().GetIFFAllegiance();
        attackRange = weaponLifetime * weaponSpeed;
        selectedFiringSound = SelectSoundFromArray(firingSounds);
    }

    // Update is called once per frame
    void Update()
    {
        ScanForTarget();
        FireWeaponAtTarget();
    }

    private void ScanForTarget()
    {
        targets = ut.FindTargetsWithinSearchRange(gameObject, searchRange);
        target = targets[0];       
    }

    private void FireWeaponAtTarget()
    {
        timeSinceLastShot -= Time.deltaTime;
        if (!target) { return; }
        Vector3 dir = target.transform.position - transform.position;
        float diff = dir.magnitude;
        //Debug.Log("distance: " + diff + ". AtkRng: " + attackRange);
        if (diff <= attackRange && timeSinceLastShot <= 0)
        {
            AudioSource.PlayClipAtPoint(selectedFiringSound, transform.position);
            selectedFiringSound = SelectSoundFromArray(firingSounds);
            float zAng = Vector3.SignedAngle(Vector2.up, dir, Vector3.forward);
            Quaternion rot = Quaternion.Euler(0, 0, zAng);
            GameObject bullet = Instantiate(weaponPrefab, transform.position + (dir*bulletOffset),rot) as GameObject;
            bullet.GetComponent<Rigidbody2D>().velocity = weaponSpeed * bullet.transform.up;
            DamageDealer dd = bullet.GetComponent<DamageDealer>();
            dd.SetDamage(weaponDamage);
            dd.SetAttackSource(gameObject);
            Destroy(bullet, weaponLifetime);
            timeSinceLastShot = timeBetweenShots;
        }
    }

    private AudioClip SelectSoundFromArray(AudioClip[] audioArray)
    {
        if (audioArray.Length == 0)
        {
            return null;
        }
        int random = UnityEngine.Random.Range(0, audioArray.Length);
        return audioArray[random];
    }

    private void OnDestroy()
    {
        ut.RemoveUnitFromTargetableList(gameObject);
    }
}


