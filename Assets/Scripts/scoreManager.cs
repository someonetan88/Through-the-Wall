using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class scoreManager : MonoBehaviour
{
    public static scoreManager Instance;

    public int score = 0; 
    private int highScore; 
    public TextMeshProUGUI scoreText; 
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI playerName;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        score = PlayerPrefs.GetInt("Score", 0);
        UpdateScoreText(); 
        UpdateHighScoreText();
        UpdateName();
    }


    public void IncrementScore(int i)
    {
        score+=i;
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.Save();

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore); 
            PlayerPrefs.Save();
        }

        UpdateScoreText();
        UpdateHighScoreText();
    }


    public void ResetHighScore()
    {
        highScore = 0; 
        score = 0;
        PlayerPrefs.SetString("Player", "");
        Register.Instance.playerName = PlayerPrefs.GetString("Player");
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.Save();
        UpdateScoreText();
        UpdateHighScoreText();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool TrySpendCoins(int amount)
    {
        if (score >= amount)
        {
            IncrementScore(-amount);
            return true;
        }
        return false;
    }

    public void UpdateName()
    {
        playerName.text = Register.Instance.playerName;
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }


    private void UpdateHighScoreText()
    {
        highScoreText.text = "High Score: " + highScore; 
    }
}
