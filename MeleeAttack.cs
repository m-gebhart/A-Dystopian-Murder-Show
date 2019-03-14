/* Michael Gebhart
 * Cologne Game Lab
 * BA 1 - Ludic Game 2018 / 2019 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour {

    /*This is the melee attack trigger instantied if an attack is executed*/

    public float attackingTime;
    public bool friendly; //whether the attack is instantiated by Hardy (friendly) or a Lobster (!friendly)
	
    void Start()
    {
        if (friendly)
            gameObject.layer = 17; //layer: friendly attack by Hardy
        else if (!friendly)
            gameObject.layer = 16; //layer: hostile attack by a Lobster
    }

	void Update ()
    {
        attackingTime -= Time.deltaTime;
        if (attackingTime < 0f)
        {
            gameObject.SendMessageUpwards("SetMeleeAttack");
            Destroy(this.gameObject);
        }
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if ((friendly && col.gameObject.layer > 10 && col.gameObject.layer < 17))
        {
            gameObject.SendMessageUpwards("CheckAttackReward", col.gameObject.layer);
            col.gameObject.SendMessage("Die"); //enemy gets killed
        }
        else if (!friendly && col.gameObject.layer == 10)
            col.gameObject.SendMessage("Die", this.transform.parent.name); //player gets killed
    }
}