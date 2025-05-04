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
    InputAction move, jump;
    Rigidbody2D rb2D;

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

    /// <summary>
    /// Enables the player input
    /// </summary>
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        actionMap = GetComponent<PlayerInput>().currentActionMap;
        actionMap.Enable();
        move = actionMap.FindAction("Move");
        jump = actionMap.FindAction("Jump");

        jump.started += Jump_started;
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
    /// Moves the player left and right
    /// </summary>
    private void MovePlayer()
    {
        float moveDir = move.ReadValue<float>();

        rb2D.velocity = new Vector2(moveDir * playerSpeed, rb2D.velocity.y);
    }
}
