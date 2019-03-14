/* Michael Gebhart
 * Cologne Game Lab
 * BA 1 - Ludic Game 2018 / 2019 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour {

    /* this class is a Lobster's child object, always located in front of it. If a player touches this trigger, the Lobster is told to attack*/
    void OnTriggerEnter2D(Collider2D col)
    {
       if (col.gameObject.layer == 10)
            gameObject.SendMessageUpwards("Attack");
    }
}
