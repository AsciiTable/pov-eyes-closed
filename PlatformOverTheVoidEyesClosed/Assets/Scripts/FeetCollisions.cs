using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetCollisions : MonoBehaviour
{
    public PlayerMovement player;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            player.isGrounded = true;
            Debug.Log("Collided with floor.");
        }
        if (collision.gameObject.CompareTag("Step"))
        {
            player.isGrounded = true;
            Debug.Log("Collided with step.");
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Step"))
        {
            player.isGrounded = false;
        }
    }
}
