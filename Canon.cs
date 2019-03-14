/* Michael Gebhart
 * Cologne Game Lab
 * BA 1 - Ludic Game 2018 / 2019 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : StationaryEnemy {

    public bool rightwards;
    public float frequencyTime;
    public float pieSpeed;
    public float explosionTime;
    private float explosionTimer;
    public float offsetTime;
    private bool offset;
    private float pieTimer;
    private bool pieShot;
    public PieAttack piePrefab;

	void Start () {
        SetCannonComponents();
    }
	
	void Update () {
        if (respawnable) RespawnProcessing();
        CheckFrequency();
        if (growable && !fullyGrown) Grow();
        else if (!growable) fullyGrown = true;
    }


    //spawn
    void SetCannonComponents()
    {
        SetComponents();
        SetManager();
        if (rightwards) transform.localRotation = Quaternion.Euler(0, 180, 0);
        if (offsetTime != 0) offset = true; else { offset = false; }
        pieTimer = 0f;
        explosionTimer = explosionTime;
        audioSource.volume *= Volumes.effectsVol;
    }

    void SetManager()
    {
        enemyManager = GameObject.Find("/CannonManager");
        this.transform.parent = enemyManager.transform;
    }

    //intervals between shots of pies
    void CheckFrequency()
    {
        if (isAlive && offset) //cooldown / wait until next shot
        {
            offsetTime -= Time.deltaTime;
            if (offsetTime < 0f) offset = false;
        }
        else if (isAlive && !offset) //pie is instantiated
        {
            pieTimer += Time.deltaTime;
            if (pieTimer >= frequencyTime)
            {
                animator.SetBool("isShooting", true);
                PieAttack newPie = GameObject.Instantiate(piePrefab);
                newPie.transform.position = this.transform.position;
                newPie.speed = pieSpeed;
                audioSource.Play();
                pieShot = true;
                pieTimer = 0f;
            }
            else if (pieShot)
                SetAnimation(); //explosion animation
        }
    }

    void SetAnimation()
    {
        explosionTimer -= Time.deltaTime;
        if (explosionTimer < 0f)
        {
            animator.SetBool("isShooting", false);
            explosionTimer = explosionTime;
            pieShot = false;
        }
    }
}