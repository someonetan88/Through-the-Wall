using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    public static Register Instance;

    public Button m, f;
    public InputField nameInput;
    public GameObject[] genderPlayer;
    public int Level;
    public ClothingType type;
    public int InputGame;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    void Start()
    {
        playerName = PlayerPrefs.GetString("Player");
        int i = PlayerPrefs.GetInt("Gender");
        type = (ClothingType)(i);
        playerGender = genderPlayer[i];
        var input = nameInput;
        input.onEndEdit.AddListener(SubmitName);
    }

    private void Update()
    {
        if (nameInput == null)
        {
            InputField inputField = FindAnyObjectByType<InputField>();
            if(inputField == null) return;
            nameInput = inputField;
            var input = nameInput;
            input.onEndEdit.AddListener(SubmitName);
        }
        if (f == null && m == null) 
        {
            Button ma = GameObject.Find("Male").GetComponent<Button>();
            Button fa = GameObject.Find("Female").GetComponent<Button>();
            if(ma == null && fa == null) return;
            m = ma;
            f = fa;
            m.onClick.AddListener(() => chooseGender(0));
            f.onClick.AddListener(() => chooseGender(1));
        }
    }

    private void SubmitName(string arg0)
    {
        playerName = arg0;
        PlayerPrefs.SetString("Player", playerName);
        scoreManager.Instance.UpdateName();
    }
    public string playerName;

    public void chooseGender(int i)
    {
        Debug.Log(i);
        type = (ClothingType)i;
        playerGender = genderPlayer[i];
        PlayerPrefs.SetInt("Gender", i);
    }

    public GameObject playerGender;
}
