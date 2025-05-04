/******************************************************************************
 * Author: Brad Dixon
 * File Name: PlayerBehaviour.cs
 * Creation Date: 5/4/2025
 * Brief: Movement and player abilities/attacks
 * ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    InputActionMap actionMap;
    InputAction move, jump, attack;
    Rigidbody2D rb2D;
    Animator animator;

    [Header("PLAYER MOVEMENT")]
    [Tooltip("Speed of the player")]
    [SerializeField] private float playerSpeed;
    [Tooltip("Jump height of the player")]
    [SerializeField] private float jumpForce;

    [Space(5)]
    [Header("PLAYER COMBAT")]
    [Tooltip("How long the player is invincinble in seconds after being hit")]
    [SerializeField] private float invincibilityTime;
    [Tooltip("Time in seconds the player must wait before they can attack again")]
    [SerializeField] private float attackDelay;
    private bool attacking;
    [SerializeField] LayerMask attackLayers;

    /// <summary>
    /// Enables the player input
    /// </summary>
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        actionMap = GetComponent<PlayerInput>().currentActionMap;
        actionMap.Enable();
        move = actionMap.FindAction("Move");
        jump = actionMap.FindAction("Jump");
        attack = actionMap.FindAction("Attack");

        jump.started += Jump_started;
        attack.started += Attack_started;
        attacking = false;
    }

    /// <summary>
    /// Lets the player attack
    /// </summary>
    /// <param name="obj"></param>
    private void Attack_started(InputAction.CallbackContext obj)
    {
        animator.SetTrigger("Attack");
    }

    /// <summary>
    /// Makes the player jump
    /// </summary>
    /// <param name="obj"></param>
    private void Jump_started(InputAction.CallbackContext obj)
    {
        if (!attacking)
        {
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    public void TryAttack(float damage)
    {
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        //List<Collider2D> targets = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(attackLayers);

        foreach(Collider2D i in colliders)
        {
            //if (Physics2D.OverlapBox(i.bounds.center, i.bounds.extents, 0, attackLayers))
            //{
            //    Debug.Log("kjdlkasjlkasdjk");
            //}

            Collider2D[] targets = Physics2D.OverlapBoxAll(i.bounds.center, i.bounds.extents, 0, attackLayers);
            foreach(Collider2D j in targets)
            {
                j.gameObject.GetComponent<EnemyBehaviour>().TakeDamage(damage);
            }
        }
    }

    /// <summary>
    /// Moves the player left and right
    /// </summary>
    private void MovePlayer()
    {
        float moveDir;
        if (!attacking)
        {
            moveDir = move.ReadValue<float>();
        }
        else
        {
            moveDir = 0;
        }

        rb2D.velocity = new Vector2(moveDir * playerSpeed, rb2D.velocity.y);
    }
}
