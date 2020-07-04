using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public RectTransform startButton;
    public RectTransform exitButton;
    public RectTransform soundButton;
    public RectTransform bgImage;
    
    void Start()
    {
        startButton.sizeDelta = new Vector2(Screen.width / 4, Screen.height / 5);
        startButton.position = new Vector3(Screen.width / 2, Screen.height * 3 / 5);
        startButton.gameObject.GetComponentInChildren<Text>().fontSize = Screen.height/15;
        exitButton.sizeDelta = new Vector2(Screen.width / 4, Screen.height / 5);
        exitButton.position = new Vector3(Screen.width / 2, Screen.height / 4);
        exitButton.gameObject.GetComponentInChildren<Text>().fontSize = Screen.height / 15;
        //soundButton.localScale = new Vector3(Screen.width / 320, Screen.width / 320);
        soundButton.position = new Vector3( Screen.width/20*2, Screen.height - soundButton.rect.height*3/4, 0);
        //Debug.Log(Screen.width + " " + Screen.height);
        bgImage.sizeDelta = new Vector3(Screen.width, Screen.width);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetColor()
    {
        Image img = soundButton.gameObject.GetComponent<Image>();
        if (img.color == Color.white) img.color = Color.HSVToRGB(0, 0, 0);
        else img.color = Color.white;
    }
}
