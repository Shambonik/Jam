using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private GameObject player;
    private Vector3 nullPoint;
    private Vector3 newPosition;
    public Material transparentMaterial;
    private List<Material> originalMaterial;
    private List<GameObject> objectMaterial;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        nullPoint = player.transform.position - transform.position;
        objectMaterial = new List<GameObject>();
        originalMaterial = new List<Material>();
    }

    // Update is called once per frame
    void Update()
    {
        newPosition = player.transform.position - nullPoint;
        transform.position = Vector3.Lerp(transform.position, new Vector3(newPosition.x, player.GetComponent<PlayerScript>().getLandingY()-nullPoint.y, newPosition.z), 0.75f*Time.deltaTime);
    }

    public void setMaterial()
    {
        for (int i = 0; i < objectMaterial.Count; i++)
        {
            if (objectMaterial[i] != null && originalMaterial[i] != null)
            {
                objectMaterial[i].GetComponent<Renderer>().material = originalMaterial[i];
                objectMaterial[i].layer = 0;
            }
        }
        objectMaterial = new List<GameObject>();
        originalMaterial = new List<Material>();
        raycastMaterial();
    }

    public void raycastMaterial()
    {
        Ray ray = new Ray(newPosition, new Vector3(player.transform.position.x, player.transform.position.y+0.3f, player.transform.position.z) - newPosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject != player)
            {
                objectMaterial.Add(hit.collider.gameObject);
                originalMaterial.Add(hit.collider.gameObject.GetComponent<Renderer>().material);
                hit.collider.gameObject.GetComponent<Renderer>().material = transparentMaterial;
                hit.collider.gameObject.layer = 2;
                raycastMaterial();
            }
        }
    }
}
