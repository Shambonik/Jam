using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2Script : MonoBehaviour
{
    private GameObject player;
    private GameObject levelChangePanel;
    private bool changeStarted = false;
    private float startTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        levelChangePanel = GameObject.FindGameObjectWithTag("LevelChangePanel");
        levelChangePanel.GetComponent<Animator>().Play("LevelChangeEnd");
    }

    // Update is called once per frame
    void Update()
    {
        if ((player.transform.position.y > 5.2f) || (changeStarted))
        {
            levelChangePanel.GetComponent<Animator>().Play("LevelChange");
            if (!changeStarted) startTime = Time.time;
            changeStarted = true;
            if (Time.time - startTime > 60 * Time.deltaTime) SceneManager.LoadScene(3);
        }
    }
}
