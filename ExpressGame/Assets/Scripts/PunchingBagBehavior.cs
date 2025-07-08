/******************************************************************************
 * Author: Jacob Nash
 * File Name: PunchingBagBehavior.cs
 * Creation Date: 6/3/2025
 * Brief: Mock script for the enemy to have something to attack
 * ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchingBagBehavior : MonoBehaviour
{
    [SerializeField] private float punchingBagHealth;
    [SerializeField] private float punchingBagMaxHealth;

    // Start is called before the first frame update
    void Start()
    {
        punchingBagHealth = punchingBagMaxHealth;
    }

    public void Update()
    {
        if (punchingBagHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void HitPunchingBag(int damage)
    {
        punchingBagHealth--;
    }
}
