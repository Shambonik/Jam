using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level3Manager : MonoBehaviour
{
    public LeverScript[] levers;
    private bool mistake = false;
    private float newRotation;
    public GameObject bridge;
    private bool solved = false;
    private GameObject levelChangePanel;
    private AudioSource bridgeSound;
    public GameObject endPanel;
    private GameObject player;
    private int phase = 0;
    private bool sound = true;

    void Start()
    {
        //newRotation = bridge.transform.localRotation.z;

        newRotation = -90;
        levelChangePanel = GameObject.FindGameObjectWithTag("LevelChangePanel");
        levelChangePanel.GetComponent<Animator>().Play("LevelChangeEnd");
        bridgeSound = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerScript>().setCoordinates(new Vector2(0, 11));
    }

    // Update is called once per frame
    void Update()
    {
        if (solved)
        {
            bridge.transform.localRotation = Quaternion.Lerp(bridge.transform.localRotation, Quaternion.AngleAxis(newRotation, Vector3.right), 1*Time.deltaTime);
            if (sound)
            {
                bridgeSound.volume = FindObjectOfType<VolumeScript>().GetVolume();
                bridgeSound.Play();
                sound = false;
            }
        }

        if (phase == 0)
        {
            if (levers[1].getActivated() || levers[2].getActivated() || levers[3].getActivated()) LeversDeactivation();
            else if (levers[0].getActivated()) phase++;
        }
        else if(phase == 1)
        {
            if (levers[1].getActivated() || !levers[0].getActivated() || levers[3].getActivated()) LeversDeactivation();
            else if (levers[2].getActivated()) phase++;
        }
        else if (phase == 2)
        {
            if (!levers[2].getActivated() || !levers[0].getActivated() || levers[3].getActivated()) LeversDeactivation();
            else if (levers[1].getActivated()) phase++;
        }
        else if (phase == 3)
        {
            if (!levers[2].getActivated() || !levers[0].getActivated() || !levers[1].getActivated()) LeversDeactivation();
            else if (levers[3].getActivated())
            {
                solved = true;
            }
        }


        if (player.transform.position.x<=-11.1)
        {
            endPanel.SetActive(true);
            StartCoroutine("EndGame");
        }
        mistake = false;
    }

    void LeversDeactivation()
    {
        phase = 0;
        Debug.Log("MICTAKE");
        mistake = true;
        foreach (LeverScript lvr in levers)
        {
            lvr.setActivated(false);
        }
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(5f);
        Application.Quit();
    }
}

