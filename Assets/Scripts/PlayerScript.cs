﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    private Vector2 coordinates = new Vector2(0, 0);
    private float jumpSpeed = 0.1f;
    private float g = 0.006f;
    private float ySpeed = 0;
    private float landingY;
    private bool jumping = false;
    private Vector3 newPosition;
    private int rot;
    private int maxCoordinate = 19;


    void Start()
    {
        newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        landingY = transform.position.y;
    } 

    public float getLandingY()
    {
        return landingY;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, rot, transform.eulerAngles.z), 30);
        transform.position = Vector3.Lerp(transform.position, newPosition, 0.1f);
        transform.position = new Vector3(transform.position.x, transform.position.y + ySpeed, transform.position.z);
        if ((ySpeed<0)&&(transform.position.y - landingY < 0.05f))
        {
            transform.position = new Vector3(newPosition.x, landingY, newPosition.z);
            ySpeed = 0;
            jumping = false;
        }
        else if (jumping) ySpeed -= g;
        Debug.Log(ySpeed);
        
        if (ySpeed==0) {
            if (Input.GetKey("w"))
            {
                if (coordinates.x + 1 > maxCoordinate) {
                    coordinates.x = maxCoordinate;
                }
                else {
                    coordinates.x++;
                    ySpeed = jumpSpeed;
                    jumping = true;
                }
                rot = 0;
            }
            else if (Input.GetKey("s"))
            {
                if (coordinates.x - 1 < 0)
                {
                    coordinates.x = 0;
                }
                else
                {
                    coordinates.x--;
                    ySpeed = jumpSpeed;
                    jumping = true;
                    
                }
                rot = 180;
            }
            else if (Input.GetKey("d"))
            {
                if (coordinates.y + 1 > maxCoordinate)
                {
                    coordinates.y = maxCoordinate;
                }
                else
                {
                    coordinates.y++;
                    ySpeed = jumpSpeed;
                    jumping = true;
                }
                rot = 90;
            }
            else if (Input.GetKey("a"))
            {
                if (coordinates.y - 1 < 0)
                {
                    coordinates.y = 0;
                }
                else
                {
                    coordinates.y--;
                    ySpeed = jumpSpeed;
                    jumping = true;
                }
                rot = -90;
            }
            newPosition = new Vector3(coordinates.x * (-0.4f), transform.position.y, coordinates.y * (0.4f));
        }
    }
}
