using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private GameObject player;
    private Vector3 nullPoint;
    private Vector3 newPosition;
    public Material transparentMaterial;
    private Material originalMaterial;
    private GameObject objectMaterial;
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

    public void setMaterial()
    {
        RaycastHit hit;
        Ray ray = new Ray(newPosition, new Vector3(player.transform.position.x, player.transform.position.y - 0.05f, player.transform.position.z) - newPosition);
        Physics.Raycast(ray, out hit);
        if (objectMaterial != null)
        {
            objectMaterial.GetComponent<Renderer>().material = originalMaterial;
        }
        if (hit.collider != null)
        {
            if (hit.collider.gameObject != player)
            {
                objectMaterial = hit.collider.gameObject;
                originalMaterial = objectMaterial.GetComponent<Renderer>().material;
                objectMaterial.GetComponent<Renderer>().material = transparentMaterial;
            }
        }
    }
}
