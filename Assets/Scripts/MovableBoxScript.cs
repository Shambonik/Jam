using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableBoxScript : MonoBehaviour
{

    private float g = 0.0048f;
    private float ySpeed = 0;
    private float landingY = 0.2f;
    private bool landing = false;
    private LayerMask layerIgnore;
    private float blockSide = 0.4f;
    private bool falling = false;
    // Start is called before the first frame update
    void Start()
    {
        layerIgnore = LayerMask.GetMask("Ignore Raycast");
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.parent == null)&&(falling))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + ySpeed, transform.position.z);
            if(transform.position.y - landingY < 0.1f)
            {
                RaycastHit hit;
                if ((!landing) && ((Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 0.4f)) || Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 0.4f, layerIgnore))) landing = true;
                if (transform.position.y - landingY < 0.05f)
                {
                    if (landing)
                    {
                        transform.position = new Vector3(transform.position.x, landingY, transform.position.z);
                        falling = false;
                    }
                    else landingY -= blockSide;
                }
            }
            else
                {
                ySpeed -= g;
                if (transform.position.y < -2f)
                {
                    falling = false;
                }
            }
        }
    }

    public void drop(float landY)
    {
        landingY = landY;
        ySpeed = 0;
        transform.parent = null;
        landing = false;
        falling = true;
    }


}
