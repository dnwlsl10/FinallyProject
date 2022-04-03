using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : MonoBehaviour
{

    public System.Action onCreateEnemy;


    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("test");
        if (other.gameObject.CompareTag("EnemySpawnZone"))
        {
            onCreateEnemy();
        }   
    }
}
