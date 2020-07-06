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
    public LeverScript lever1;
    public LeverScript lever2;
    public GameObject gate1;
    public GameObject gate2;
    public GameObject ladder;
    private float newRotation1;
    private float newRotation2;
    private float newLadderPosition;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        levelChangePanel = GameObject.FindGameObjectWithTag("LevelChangePanel");
        levelChangePanel.GetComponent<Animator>().Play("LevelChangeEnd");
        newRotation1 = 90;
        newRotation2 = 90;
        newLadderPosition = 4;
    }

    // Update is called once per frame
    void Update()
    {
        gate1.transform.rotation = Quaternion.Lerp(gate1.transform.rotation, Quaternion.Euler(gate1.transform.rotation.eulerAngles.x, newRotation1, gate1.transform.rotation.eulerAngles.z), 1 * Time.deltaTime);
        gate2.transform.rotation = Quaternion.Lerp(gate2.transform.rotation, Quaternion.Euler(gate2.transform.rotation.eulerAngles.x, newRotation2, gate2.transform.rotation.eulerAngles.z), 1 * Time.deltaTime);
        ladder.transform.localPosition = Vector3.Lerp(ladder.transform.localPosition, new Vector3(ladder.transform.localPosition.x, newLadderPosition, ladder.transform.localPosition.z), 1 * Time.deltaTime);
        if (lever1.getActivated())
        {
            newRotation1 = 0;
            newRotation2 = 180;
        }
        else
        {
            newRotation1 = 90;
            newRotation2 = 90;
        }
        if (lever2.getActivated())
        {
            newLadderPosition = 0.6f;
            Debug.Log("Lever2 works");
        }
        else newLadderPosition = 4; ;
        if ((player.transform.position.y > 5.2f) || (changeStarted))
        {
            levelChangePanel.GetComponent<Animator>().Play("LevelChange");
            if (!changeStarted) startTime = Time.time;
            changeStarted = true;
            if (Time.time - startTime > 60 * Time.deltaTime) SceneManager.LoadScene(2);
        }
    }
}
