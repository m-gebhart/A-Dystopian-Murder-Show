/* Michael Gebhart
 * Cologne Game Lab
 * BA 1 - Ludic Game 2018 / 2019 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeScript : MonoBehaviour {

    /* located at the ends of every platform, telling back-and-forth moving enemies to turn around */
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.StartsWith("Banana")  || col.gameObject.name.StartsWith("Lobster"))
            col.gameObject.SendMessage("ChangeDirection");
    }
}