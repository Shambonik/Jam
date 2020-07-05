using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : MonoBehaviour
{

    private GameObject player;
    private bool activated = false;
    private Quaternion newRotation;
    private float startTime;
    private float delayDuration = 24;
    private GameObject lever; 
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        startTime = Time.time;
        int i = 0;
        for (lever = transform.GetChild(i).gameObject; lever.name != "Lever"; i++) ;
        newRotation = lever.transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime > delayDuration * Time.deltaTime)
        {
            lever.transform.localRotation = Quaternion.Lerp(lever.transform.localRotation, newRotation, 0.2f);
        }
    }

    public void changePosition()
    {
        Debug.Log(Mathf.Abs(transform.rotation.eulerAngles.y - player.GetComponent<PlayerScript>().getRot()));
        if (Mathf.Abs(Mathf.Abs(transform.rotation.eulerAngles.y - player.GetComponent<PlayerScript>().getRot()) - 180) < 0.01)
        {
            if (!activated) player.GetComponent<Animator>().Play("PushLever");
            else player.GetComponent<Animator>().Play("PullLever");
            startChanging();
        }
        else if ((Mathf.Abs(transform.rotation.eulerAngles.y - player.GetComponent<PlayerScript>().getRot()) < 0.01)||(Mathf.Abs(Mathf.Abs(transform.rotation.eulerAngles.y - player.GetComponent<PlayerScript>().getRot()) - 360)  < 0.01))
        {
            if (!activated) player.GetComponent<Animator>().Play("PullLever");
            else player.GetComponent<Animator>().Play("PushLever");
            startChanging();
        }
    }

    private void startChanging()
    {
        activated = !activated;
        startTime = Time.time;
        newRotation = Quaternion.Euler(-120, lever.transform.localRotation.eulerAngles.y, lever.transform.localRotation.eulerAngles.z);
    }

    public bool getActivated()
    {
        return activated;
    }

    public void setActivated(bool act)
    {
        if(act!=activated) newRotation = Quaternion.Euler(-120, lever.transform.localRotation.eulerAngles.y, lever.transform.localRotation.eulerAngles.z);
        activated = act;
    }
}
