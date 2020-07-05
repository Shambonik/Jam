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
    public RectTransform restartButton;
    public GameObject menu;
    private PlayerScript player;
    private bool death = false;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        startButton.sizeDelta = new Vector2(Screen.width / 4, Screen.height / 5);
        startButton.position = new Vector3(Screen.width / 2, Screen.height * 5 / 7);
        startButton.gameObject.GetComponentInChildren<Text>().fontSize = Screen.height/15;

        restartButton.sizeDelta = new Vector2(Screen.width / 4, Screen.height / 5);
        restartButton.position = new Vector3(Screen.width / 2, Screen.height *3/ 7);
        restartButton.gameObject.GetComponentInChildren<Text>().fontSize = Screen.height / 15;

        exitButton.sizeDelta = new Vector2(Screen.width / 4, Screen.height / 5);
        exitButton.position = new Vector3(Screen.width / 2, Screen.height /7);
        exitButton.gameObject.GetComponentInChildren<Text>().fontSize = Screen.height / 15;

        soundButton.localScale = new Vector3(Screen.width / 320, Screen.width / 320);
        soundButton.position = new Vector3( Screen.width/20*2, Screen.height - soundButton.rect.height*3/4, 0);
        //Debug.Log(Screen.width + " " + Screen.height);
        bgImage.localScale = new Vector2(Screen.width, Screen.width);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && tag == "Pause Menu" && !death)
        {
            //this.GetComponent<Canvas>().enabled = !this.GetComponent<Canvas>().enabled;
            menu.SetActive(!menu.activeSelf);
            player.enabled = !player.enabled;
        }

       /* if (this.GetComponent<Canvas>().enabled && player.enabled)
        {
            death = true;
            StartCoroutine("playerDeath");
        }*/

        if (player.getDead())
        {
            death = true;
            StartCoroutine("playerDeath");
        }

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

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //FindObjectOfType<PlayerScript>().stopTime();
    }

    public void Continue()
    {
        //this.GetComponent<Canvas>().enabled = !this.GetComponent<Canvas>().enabled;
        menu.SetActive(!menu.activeSelf);
        player.enabled = !player.enabled;
    }

    IEnumerator playerDeath()
    {
        yield return new WaitForSeconds(6.5f);
        RestartLevel();
        //yield return new WaitForSeconds(8f);
    }
}
