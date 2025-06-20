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
    private bool attacking, canAttack;
    private int comboIndex;
    [SerializeField] LayerMask attackLayers;

    //Bool used to determine if the player has attack delay after an attack
    [Space(5)]
    public bool HaveAttackDelay;

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
        canAttack = true;
    }

    /// <summary>
    /// Lets the player attack
    /// </summary>
    /// <param name="obj"></param>
    private void Attack_started(InputAction.CallbackContext obj)
    {
        //animator.SetTrigger("Attack");
        if (canAttack)
        {
            canAttack = false;
            Debug.Log("starting attack");
            ComboAttack();
        }
    }

    /// <summary>
    /// Executes the three hit combo
    /// </summary>
    private void ComboAttack()
    {
        //tells the animator to play the appropriate animation
        switch(comboIndex)
        {
            case 0:
                animator.SetTrigger("Combo1");
                break;
            case 1:
                attacking = true;
                animator.SetTrigger("Combo2");
                break;
            case 2:
                attacking = true;
                animator.SetTrigger("Combo3");
                comboIndex = 0;
                break;
            default:
                Debug.Log("combo index out of bounds");
                break;
        }
    }

    /// <summary>
    /// How much time the player has to wait after attacking before they can attack again
    /// </summary>
    /// <param name="dTime"></param>
    /// <returns></returns>
    private IEnumerator AttackDelay(float dTime)
    {
        float t = 0;
        while (t < dTime)
        {
            yield return new WaitForSeconds(.1f);
            t += .1f;
        }
        canAttack = true;
        Debug.Log("Ready to attack");
    }

    /// <summary>
    /// How much time the player has between attacks before the combo ends
    /// </summary>
    /// <returns></returns>
    private IEnumerator ComboTime(float cTime)
    {
        ++comboIndex;
        float t = 0;
        //Temp variable used to reset the combo if they don't attack in time
        int cIndex = 0;

        while(t < cTime)
        {
            if(attacking)
            {
                attacking = false;
                cIndex = 1;
                break;
            }

            yield return new WaitForSeconds(.1f);
            t += .1f;
        }

        if (cIndex == 0)
        {
            Debug.Log("combo dropped");
        }
        else
        {
            Debug.Log("combo continued");
        }

        comboIndex *= cIndex;
    }

    /// <summary>
    /// Used by the animator to let the player attack again
    /// </summary>
    /// <param name="t"></param>
    public void PlayerCanAttackAgain(float t)
    {
        if (HaveAttackDelay)
        {
            StartCoroutine(AttackDelay(t));
        }
        else
        {
            canAttack = true;
        }
    }

    /// <summary>
    /// Makes the player jump
    /// </summary>
    /// <param name="obj"></param>
    private void Jump_started(InputAction.CallbackContext obj)
    {  
        rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    /// <summary>
    /// Function called by the animation to see if an attack connects
    /// </summary>
    /// <param name="damage"></param>
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
        moveDir = move.ReadValue<float>();

        rb2D.velocity = new Vector2(moveDir * playerSpeed, rb2D.velocity.y);
    }
}
