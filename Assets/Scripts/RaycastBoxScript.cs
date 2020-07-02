using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastBoxScript : MonoBehaviour
{

    private float deltaY;
    private GameObject player;
    //public bool[] obstacle = { false, false, false, false };
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        deltaY = transform.position.y - player.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + deltaY, player.transform.position.z);
    }

    public bool obstacle(Vector3 direction)
    {
        RaycastHit hit;
        if ((Physics.Raycast(transform.position, transform.TransformDirection(direction), out hit, 0.4f))) return true;
        return false;
    }
}