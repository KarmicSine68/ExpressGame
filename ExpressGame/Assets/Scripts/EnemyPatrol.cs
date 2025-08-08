/******************************************************************************
 * Author: Jacob Nash
 * File Name: EnemyPatrol.cs
 * Creation Date: 6/2/2025
 * Brief: Movement of enemies when they don't detect the player
 * ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private GameObject pointA;
    [SerializeField] private GameObject pointB;
    private Rigidbody2D rb;
    private Transform currentPoint;
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float pointARange;
    [SerializeField] private float pointBRange;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = pointB.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Actual enemy patrol movement
        Vector2 point = currentPoint.position - transform.position;
        if(currentPoint == pointB.transform)
        {
            rb.velocity = new Vector2(patrolSpeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-patrolSpeed, 0);
        }

        //if the enemy gets to point B, it then moves back to point A
        if (Vector2.Distance(transform.position, currentPoint.position) < pointBRange && currentPoint == pointB.transform)
        {
            Flip();
            currentPoint = pointA.transform;
        }
        
        //if the enemy gets to point A, it then moves back to point B
        if (Vector2.Distance(transform.position, currentPoint.position) < pointARange && currentPoint == pointA.transform)
        {
            Flip();
            currentPoint = pointB.transform;
        }
    }

    //flips the character sprite when patrol from point B to point A
    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
