using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastBoxScript : MonoBehaviour
{

    public float deltaY;
    private GameObject player;
    private LayerMask layerIgnore;
    private float originalDeltaY;
    private float deltaX = 0;
    private float deltaZ = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //deltaY = transform.position.y - player.transform.position.y;
        layerIgnore = LayerMask.GetMask("Ignore Raycast");
        originalDeltaY = deltaY;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool obstacle(Vector3 direction)
    {
        transform.position = new Vector3(player.transform.position.x - deltaX, player.transform.position.y + deltaY, player.transform.position.z + deltaZ);
        RaycastHit hit;
        if ((Physics.Raycast(transform.position, transform.TransformDirection(direction), out hit, 0.4f))) return true;
        if ((Physics.Raycast(transform.position, transform.TransformDirection(direction), out hit, 0.4f, layerIgnore))) return true;
        return false;
    }

    public GameObject getObject(Vector3 direction)
    {
        transform.position = new Vector3(player.transform.position.x - deltaX, player.transform.position.y + deltaY, player.transform.position.z + deltaZ);
        RaycastHit hit;
        if ((Physics.Raycast(transform.position, transform.TransformDirection(direction), out hit, 0.4f))) return hit.transform.gameObject;
        if ((Physics.Raycast(transform.position, transform.TransformDirection(direction), out hit, 0.4f, layerIgnore))) return hit.transform.gameObject;
        return null;
    }

    public void changeDelta(float x, float y, float z)
    {
        deltaX = x;
        deltaY = y;
        deltaZ = z;
        transform.position = new Vector3(player.transform.position.x - deltaX, player.transform.position.y + deltaY, player.transform.position.z + deltaZ);
    }
    
    public float getOriginalDeltaY()
    {
        return originalDeltaY;
    }

    public void setOriginalDelta()
    {
        deltaX = 0;
        deltaZ = 0;
        deltaY = originalDeltaY;
        transform.position = new Vector3(player.transform.position.x - deltaX, player.transform.position.y + deltaY, player.transform.position.z + deltaZ);
    }

}