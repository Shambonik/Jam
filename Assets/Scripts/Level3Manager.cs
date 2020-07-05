using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Manager : MonoBehaviour
{
    public LeverScript[] levers;
    private bool mistake = false;
    private float newRotation;
    public GameObject bridge;
    private bool solved = false;
    private GameObject levelChangePanel;

    void Start()
    {
        //newRotation = bridge.transform.localRotation.z;
        newRotation = 0;
        levelChangePanel = GameObject.FindGameObjectWithTag("LevelChangePanel");
        levelChangePanel.GetComponent<Animator>().Play("LevelChangeEnd");
    }

    // Update is called once per frame
    void Update()
    {
        if (solved)
        {
            bridge.transform.localRotation = Quaternion.Lerp(bridge.transform.localRotation, Quaternion.AngleAxis(newRotation, Vector3.right), 0.05f);
        }
        if (!mistake && levers[0].getActivated())
        {
            if (!mistake && levers[2].getActivated())
            {
                if (!mistake && levers[1].getActivated())
                {
                    if (!mistake && levers[3].getActivated())
                    {
                        newRotation = -90f;
                        solved = true;
                    }
                    else LeversDeactivation();
                }
                else LeversDeactivation();
            }
            else LeversDeactivation();
        }
        else LeversDeactivation();

        mistake = false;
    }

    void LeversDeactivation()
    {
        mistake = true;
        foreach (LeverScript lvr in levers)
        {
            lvr.setActivated(false);
        }
    }
}
