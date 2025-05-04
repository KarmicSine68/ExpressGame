/******************************************************************************
 * Author: Brad Dixon
 * File Name: EnemyBehaviour.cs
 * Creation Date: 5/4/2025
 * Brief: Basic template for enemies
 * ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("ENEMY STATS")]
    [SerializeField] private float enemyHealth;
    [SerializeField] private float enemyMaxHealth;

    private void Awake()
    {
        enemyHealth = enemyMaxHealth;
    }

    /// <summary>
    /// Makes the enemy take damage
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {
        enemyHealth -= damage;
    }

    protected void Update()
    {
        if(enemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
