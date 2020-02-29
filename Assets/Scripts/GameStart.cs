using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    public UILabel nameInput;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("name"))
        {
            nameInput.text = PlayerPrefs.GetString("name");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonSureClick()
    {
        PlayerPrefs.SetString("name", nameInput.text);
        SceneManager.LoadScene(1);
    }
}
