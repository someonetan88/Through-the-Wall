using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSpawn : MonoBehaviour
{
    public GameObject[] wallPrefabs;
    public Transform spawnPoint;
    public Transform playerPoint;
    public bool isMultiPlayer;
    public bool isLevel;
    public float initialMoveSpeed = 5f;

    public GameObject player;
    private float moveSpeed;
    private GameObject currentWall;
    private bool isSpawning = false;
    private int Index;

    void Start()
    {
        if (!isMultiPlayer)
            Instantiate(player, playerPoint.position, Quaternion.Euler(0, 180f, 0));
        else
        {
            Instantiate(player, new Vector3(-0.930249989f, 0, 0), Quaternion.Euler(0, 0, 0));
            Instantiate(player, new Vector3(4.45979977f, 0, 0), Quaternion.Euler(0, 0, 0));
        }
        moveSpeed = initialMoveSpeed;
        SpawnWall();
    }

    public void SpawnWall()
    {
        if (isSpawning || currentWall != null) return;

        isSpawning = true;

        int randomIndex = Random.Range(0, wallPrefabs.Length);
        GameObject selectedWallPrefab = wallPrefabs[randomIndex];
        Quaternion verticalRotation = Quaternion.identity;
        if (!isMultiPlayer)
            verticalRotation = Quaternion.Euler(-90, 0, 0);
        else
            verticalRotation = Quaternion.Euler(0, 0, 0);
        currentWall = Instantiate(selectedWallPrefab, spawnPoint.position, verticalRotation);

        moveTowardsPlayer movementScript = currentWall.AddComponent<moveTowardsPlayer>();
        movementScript.Initialize(playerPoint, moveSpeed);
        moveSpeed += 0.06f;

        isSpawning = false;
        Debug.Log("Wall spawned at: " + spawnPoint.position + ", Wall prefab: " + selectedWallPrefab.name + ", Wall speed: " + moveSpeed);
    }

    public void SpawnWallLevel()
    {
        if (isSpawning || currentWall != null) return;

        isSpawning = true;

        Index = 0;
        GameObject selectedWallPrefab = wallPrefabs[Index];
        Quaternion verticalRotation = Quaternion.identity;
        if (!isMultiPlayer)
            verticalRotation = Quaternion.Euler(-90, 0, 0);
        else
            verticalRotation = Quaternion.Euler(0, 0, 0);
        currentWall = Instantiate(selectedWallPrefab, spawnPoint.position, verticalRotation);

        moveTowardsPlayer movementScript = currentWall.AddComponent<moveTowardsPlayer>();
        movementScript.Initialize(playerPoint, moveSpeed);
        moveSpeed += 0.06f;

        isSpawning = false;
        Index++;
        Debug.Log("Wall spawned at: " + spawnPoint.position + ", Wall prefab: " + selectedWallPrefab.name + ", Wall speed: " + moveSpeed);
    }

    public void WallDestroyed()
    {
        if (currentWall != null)
        {
            Collider wallCollider = currentWall.GetComponent<Collider>();
            if (wallCollider != null)
            {
                wallCollider.enabled = false; // Nonaktifkan collider wall sebelum dihancurkan
            }

            Destroy(currentWall);
        }

        currentWall = null;

        if (isLevel && Index == wallPrefabs.Length - 1)
        {
            if (int.Parse(SceneManager.GetActiveScene().name) > Register.Instance.Level)
            {
                Register.Instance.Level = int.Parse(SceneManager.GetActiveScene().name);
            }
            SceneManager.LoadScene("Level");
        }
        else
        {
            Debug.Log("Wall destroyed. Spawning a new one...");
            StartCoroutine(SpawnWallWithDelay(0.5f)); // Delay sebelum spawn wall baru
        }
    }

    private IEnumerator SpawnWallWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnWall(); // Spawn wall baru setelah delay
    }
}
