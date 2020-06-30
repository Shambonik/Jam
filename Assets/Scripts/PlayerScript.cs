using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    private Vector2 coordinates = new Vector2(0, 0);
    private Vector3 newPosition;
    private int rot;
    public int maxCoordinate = 14;
    // Start is called before the first frame update
    void Start()
    {
        newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, rot, transform.eulerAngles.z), 30);
        transform.position = Vector3.Lerp(transform.position, newPosition, 0.1f);
        if (Mathf.Sqrt(Mathf.Pow(transform.position.x - newPosition.x, 2) + Mathf.Pow(transform.position.z - newPosition.z, 2)) < 0.1f) {
            if (Input.GetKey("d"))
            {
                coordinates.x = Mathf.Min(coordinates.x + 1, maxCoordinate);
                rot = 0;
               // transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 0, transform.eulerAngles.y);
            }
            else if (Input.GetKey("a"))
            {
                coordinates.x = Mathf.Max(coordinates.x - 1, 0);
                rot = 180;
              //  transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 180, transform.eulerAngles.y);
            }
            else if (Input.GetKey("w"))
            {
                coordinates.y = Mathf.Min(coordinates.y + 1, maxCoordinate);
                rot = -90;
               // transform.rotation = Quaternion.Euler(transform.eulerAngles.x, -90, transform.eulerAngles.y);
            }
            else if (Input.GetKey("s"))
            {
                coordinates.y = Mathf.Max(coordinates.y - 1, 0);
                rot = 90;
                
            }
            //transform.rotation = Quaternion.Euler(transform.eulerAngles.x, rot, transform.eulerAngles.z);
            newPosition = new Vector3(coordinates.x * (-0.4f), transform.position.y, coordinates.y * (-0.4f));
        }
        Debug.Log(coordinates);
    }
}
