using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float movementSmoothing = .05f;
    private Vector2 velocity = Vector2.zero;
    public void Move(Vector2 move)
    {
        Vector3 targetVelocity = move;
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);

    }

   

}
