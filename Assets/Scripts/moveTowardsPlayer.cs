using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveTowardsPlayer : MonoBehaviour
{
    private Transform player;
    private float moveSpeed;

    public void Initialize(Transform playerTransform, float speed)
    {
        player = playerTransform;
        moveSpeed = speed;
        StartCoroutine(MoveWall());
    }

    private IEnumerator MoveWall()
    {
        while (transform.position.y <= 1.4f)
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            yield return null;
        }
        float targetY = transform.position.y;
        while (Vector3.Distance(new Vector3(transform.position.x, targetY, transform.position.z), new Vector3(player.position.x, targetY, player.position.z)) > 0.1f)
        {
            Vector3 targetPosition = new Vector3(player.position.x, targetY, player.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

}
