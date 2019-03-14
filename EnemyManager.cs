/* Michael Gebhart
 * Cologne Game Lab
 * BA 1 - Ludic Game 2018 / 2019 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    /* Keeps count of every enemy killed for each enemy type, overall killed counter gets incremented */

    public int killed;

    void Start () { killed = 0; }

    void Killcount(string enemytype)
    {
        killed++;
       // Debug.Log(killed+" "+enemytype+"s killed");
    }

    int enemieskilled()
    {
        return killed;
    }
}