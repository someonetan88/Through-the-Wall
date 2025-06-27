using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WallSpawner : MonoBehaviour
{
    public GameObject[] wallPrefabs;
    public Transform spawnPoint;
    public Transform playerPoint;
    public bool isMultiPlayer;
    public bool isLevel;
    public float initialMoveSpeed = 5f;
    
    public GameObject player;
    public bool test;
    public GameObject Defeat;

    public GameObject bodyPartButtons;
    public FixedJoystick joystick;
    public GameObject bodyPartButtons2;
    public FixedJoystick joystick2;

    private float moveSpeed;
    private GameObject currentWall;
    private bool isSpawning = false;
    private int Index = 0;

    void Start()
    {
        Defeat = GameObject.Find("Defeat");
        print(Defeat);
        if(Defeat != null)
            Defeat.SetActive(false);
        if(!test)
        player = Register.Instance.playerGender;
        
        AudioManager.Instance.PlayMusic("Two");
        moveSpeed = initialMoveSpeed;
        if(!isLevel)
            SpawnWall();
        else
            SpawnWallLevel();

        bodyPartButtons = GameObject.Find("ButtonChar");
        joystick = GameObject.Find("Fixed Joystick").GetComponent<FixedJoystick>();
        if(isMultiPlayer){
            bodyPartButtons2 = GameObject.Find("ButtonChar2");
            joystick2 = GameObject.Find("Fixed Joystick2").GetComponent<FixedJoystick>();
        }

        if (!isMultiPlayer)
            {
                GameObject playerOne = Instantiate(player, playerPoint.position, Quaternion.Euler(0, 180f, 0));
                playerOne.GetComponent<RagdollController>().joystick = joystick;
                playerOne.GetComponent<RagdollController>().bodyPartButtons = bodyPartButtons;
                playerOne.GetComponent<RagdollController>().playerTwo = false;
                playerOne.GetComponent<RagdollController>().Init();
            }
            else
            {
                GameObject playerOne = Instantiate(player, new Vector3(-2.06f, playerPoint.position.y, playerPoint.position.z), Quaternion.Euler(0, 180f, 0));
                GameObject playerTwo = Instantiate(player, new Vector3(2.13f, playerPoint.position.y, playerPoint.position.z), Quaternion.Euler(0, 180f, 0));
                playerOne.GetComponent<RagdollController>().joystick = joystick;
                playerOne.GetComponent<RagdollController>().bodyPartButtons = bodyPartButtons;
                playerOne.GetComponent<RagdollController>().playerTwo = false;
                playerTwo.GetComponent<RagdollController>().joystick = joystick2;
                playerTwo.GetComponent<RagdollController>().bodyPartButtons = bodyPartButtons2;
                playerTwo.GetComponent<RagdollController>().playerTwo = true;
                playerOne.GetComponent<RagdollController>().Init();
                playerTwo.GetComponent<RagdollController>().Init();
            }

    }

    public void SpawnWall()
    {
        if (isSpawning || currentWall != null) return;

        isSpawning = true;

        int randomIndex = Random.Range(0, wallPrefabs.Length);
        GameObject selectedWallPrefab = wallPrefabs[randomIndex];
        Quaternion verticalRotation = Quaternion.identity;
        verticalRotation = Quaternion.Euler(-90, 0, 0);
        currentWall = Instantiate(selectedWallPrefab, spawnPoint.position, verticalRotation);
        WallCollision col = currentWall.GetComponent<WallCollision>();
        if (col == null)
            col = currentWall.GetComponentInChildren<WallCollision>();
        col.Defeat = Defeat;

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

        GameObject selectedWallPrefab = wallPrefabs[Index];
        Quaternion verticalRotation = Quaternion.identity;
        verticalRotation = Quaternion.Euler(-90, 0, 0);
        currentWall = Instantiate(selectedWallPrefab, spawnPoint.position, verticalRotation);
        WallCollision col = currentWall.GetComponent<WallCollision>();
        if (col == null)
            col = currentWall.GetComponentInChildren<WallCollision>();
        col.Defeat = Defeat;

        moveTowardsPlayer movementScript = currentWall.AddComponent<moveTowardsPlayer>();
        movementScript.Initialize(playerPoint, moveSpeed);
        moveSpeed += 0.06f;

        Index++;
        isSpawning = false;
        Debug.Log("Wall spawned at: " + spawnPoint.position + ", Wall prefab: " + selectedWallPrefab.name + ", Wall speed: " + moveSpeed);
    }

    public void WallDestroyed()
    {
        if (currentWall != null)
        {
            Collider wallCollider = currentWall.GetComponent<Collider>();
            if (wallCollider != null)
            {
                wallCollider.enabled = false;
            }

            Destroy(currentWall);
        }

        currentWall = null;

        if (isLevel && Index == wallPrefabs.Length)
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
            StartCoroutine(SpawnWallWithDelay(0.5f));
        }
    }

    private IEnumerator SpawnWallWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!isLevel)
            SpawnWall();
        else
            SpawnWallLevel();
    }
}
