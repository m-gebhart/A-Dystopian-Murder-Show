/* Michael Gebhart
 * Cologne Game Lab
 * BA 1 - Ludic Game 2018 / 2019 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryEnemy : Enemy {

    /* More stationary enemies were actually planned, but ended up with one type: the Pie Cannon*/

    public bool jumpable; //if player can jump on top or not

    protected void OnCollisionEnter2D(Collision2D col)
    {
        if (!jumpable && col.gameObject.layer == 10)
            col.gameObject.SendMessage("Die"); //kills player if deadly
        else if (jumpable && col.gameObject.layer == 10) 
                col.gameObject.SendMessage("GroundContact"); //behaves like a platform
    }
}
