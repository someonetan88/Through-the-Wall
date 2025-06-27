using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetHighScore : MonoBehaviour
{

 public void OnResetHighScoreButtonPressed()
    {
        PlayerPrefs.SetInt("HighScore", 0); 
        PlayerPrefs.Save(); 
        Debug.Log("High score has been reset from the main menu.");
    }
}
