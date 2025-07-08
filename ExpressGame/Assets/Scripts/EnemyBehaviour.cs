/******************************************************************************
 * Author: Brad Dixon and Jacob Nash
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
    [SerializeField] private float enemySpeed;
    [SerializeField] private float distanceVariable;
    [SerializeField] private int damage;
    [SerializeField] private GameObject player;
    private float distance;

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
        Debug.Log("The enemy has: " + enemyHealth);
    }

    protected void Update()
    {
        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
        }

        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if(enemyHealth > 0 && distance < distanceVariable)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, enemySpeed * Time.deltaTime);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        player.gameObject.GetComponent<PunchingBagBehavior>().HitPunchingBag(damage);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Punching Bag was Hit!");
    }
}
