/* Michael Gebhart
 * Cologne Game Lab
 * BA 1 - Ludic Game 2018 / 2019 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MovingEnemy
{
    void Start()
    {
        SetComponents();
        SetManager();
    }

    void Update()
    {
        if (growable && !fullyGrown) Grow();
        else if (!growable) fullyGrown = true;
        else if (movable) Move();
    }

    void LateUpdate()
    {
        if (respawnable) RespawnProcessing();
    }

    void SetManager()
    {
        enemyManager = GameObject.Find("BananaManager");
        this.transform.parent = enemyManager.transform;
    }

    protected void OnCollisionEnter2D(Collision2D col) //polygoncollider = whole body = player dies if colliding with it
    {
        if (col.gameObject.layer == 10 && fullyGrown) col.gameObject.SendMessage("Die", this.name); //if player (10)
    }

    //no OnTriggerEnter2D (jumping on top of bananas), due to design decisions
}