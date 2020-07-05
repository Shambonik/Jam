using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Level1Manager : MonoBehaviour
{
    public RectTransform tutorial1;
    private GameObject player;
    private GameObject levelChangePanel;
    private bool changeStarted = false;
    private float startTime = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        levelChangePanel = GameObject.FindGameObjectWithTag("LevelChangePanel");
        //tutorial1.localScale = new Vector2(Screen.width / 4, Screen.height / 5);
        tutorial1.position = new Vector3(Screen.width/5, Screen.height * 4 / 7);
        tutorial1.gameObject.GetComponentInChildren<Text>().fontSize = Screen.height / 20;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            tutorial1.gameObject.GetComponentInChildren<Text>().rectTransform.sizeDelta = new Vector2(Screen.width / 3, Screen.height / 2);
            tutorial1.sizeDelta = new Vector2(Screen.width / 3, Screen.height *2 / 3);
            tutorial1.gameObject.GetComponentInChildren<Text>().text = "To pick up a crate hold \"E\" and press \"W\", \"A\", \"S\" or \"D\" according to " +
                "where the crate is placed. Try putting that crate in the water to form a bridge and proceed to next level!";
        }
        if((player.transform.position.y > 3)||(changeStarted))
        {
            levelChangePanel.GetComponent<Animator>().Play("LevelChange");
            if(!changeStarted) startTime = Time.time;
            changeStarted = true;
            if (Time.time - startTime > 60*Time.deltaTime) SceneManager.LoadScene(2);
        }
    }
}
