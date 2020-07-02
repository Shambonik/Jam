using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private GameObject player;
    private Vector3 nullPoint;
    private Vector3 newPosition;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        nullPoint = player.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        newPosition = player.transform.position - nullPoint;
        transform.position = Vector3.Lerp(transform.position, new Vector3(newPosition.x, player.GetComponent<PlayerScript>().getLandingY()-nullPoint.y, newPosition.z), 0.75f*Time.deltaTime);
    }
}
