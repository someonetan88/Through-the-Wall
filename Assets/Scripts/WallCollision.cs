using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class WallCollision : MonoBehaviour
{
    public Collider[] childColliders;
    private WallSpawner wallSpawner;
    public GameObject Defeat;

    private scoreManager scoreManager;

    void Start()
    {
        wallSpawner = FindObjectOfType<WallSpawner>();
        scoreManager = FindObjectOfType<scoreManager>();
        Time.timeScale = 1.0f;
    }

    void Update()
    {
        Debug.Log(childColliders.Length);
        int collided = 0;
        foreach (Collider collider in childColliders) 
        { 
            Lubang lubang = collider.gameObject.GetComponent<Lubang>();
            collided += lubang.collided;
        }

        Debug.Log(collided);
        if (collided == childColliders.Length) 
        {
            scoreManager.IncrementScore(1);
            AudioManager.Instance.PlaySFX("Right");
            wallSpawner.WallDestroyed();
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySFX("Wrong");
            Defeat.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
