using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
     private bool hasPassedThrough = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall") && !hasPassedThrough)
        {
            Debug.Log("Success! Ragdoll passed through the hole.");
            hasPassedThrough = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            Debug.Log("Failed! Ragdoll hit the wall.");
            // Stop the wall movement if the ragdoll hits it
            // var wallScript = collision.collider.GetComponent<moveTowardsPlayer>();
            // if (wallScript != null)
            // {
            //     wallScript.StopMovement();
            // }
        }
    }
}
