using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    public GameObject AfterRegister;
    public GameObject OnRegister;
    public GameObject HomeMenu;
    public GameObject ModeMenu;
    public Button EndlessButton;
    public Button MultiButton;

    [Space]

    public GameObject OnLevel;

    public Button[] Levels;

    void Start()
    {
        Time.timeScale = 1.0f;
        AudioManager.Instance.PlayMusic("One");

        if (OnLevel == null) {
            if (Register.Instance.playerName != "")
            {
                AfterRegister.SetActive(true);
                OnRegister.SetActive(false);
            } 

            if (Register.Instance.Level < 10)
            {
                EndlessButton.interactable = false;
                MultiButton.interactable = false;
            }
            else
            {
                EndlessButton.interactable = true;
                MultiButton.interactable = true;
            }
        }
        else
        {
            for (int i = 0; i < Levels.Length; i++)
            {
                if(i <= Register.Instance.Level)
                {
                    Levels[i].interactable = true;
                }
                else
                {
                    Levels[i].interactable = false;
                }
            }
        }
    }

    public void BackToMode(bool isMode)
    {
        ModeMenu.SetActive(isMode);
    }
}
