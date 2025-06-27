using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagChange : MonoBehaviour
{
   public string newTag = "Player"; // Set the tag you want to assign

    // Start is called before the first frame update
    void Start()
    {
        // Change the tag of this GameObject
        gameObject.tag = newTag;

        // Change the tag of all child GameObjects
        foreach (Transform child in transform)
        {
            child.gameObject.tag = newTag; // Change the tag of each child
        }
    }
}
