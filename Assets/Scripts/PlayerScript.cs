using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    private Vector2 coordinates = new Vector2(0, 0);
    private float jumpSpeed = 0.1f;
    private float g = 0.0048f;
    private float ySpeed = 0;
    private float landingY;
    private bool jumping = false;
    private Vector3 newPosition;
    private int rot;
    private int maxCoordinate = 19;
    private Animator anim;
    private float delayDuration = 8;
    private float startTime;
    private RaycastBoxScript raycastBoxScript;
    private bool landing = false;

    void Start()
    {
        newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        landingY = transform.position.y;
        anim = transform.GetComponent<Animator>();
        startTime = Time.time;
        raycastBoxScript = GameObject.FindGameObjectWithTag("RaycastBox").GetComponent<RaycastBoxScript>();
    }

    public float getLandingY()
    {
        return landingY;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, rot, transform.eulerAngles.z), 800 * Time.deltaTime);
        if ((Time.time - startTime) > delayDuration * Time.deltaTime)
        {
            anim.SetBool("isJumping", false);
            transform.position = Vector3.Lerp(transform.position, newPosition, 3.2f * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, transform.position.y + ySpeed, transform.position.z);
            RaycastHit hit;
            if ((ySpeed < 0) && (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 0.2f)))
            {
                if (transform.position.y - landingY > 0.18f) landingY += 0.4f;
                landing = true;
            }
            else if (jumping)
            {
                ySpeed -= g;
                if (transform.position.y - landingY < -0.15f) landingY -= 0.4f;
                if (transform.position.y < -0.4f)
                {
                    ySpeed = 0;
                    jumping = false;
                }
            }
            if (landing && (transform.position.y - landingY < 0.05f))
            {
                transform.position = new Vector3(newPosition.x, landingY, newPosition.z);
                ySpeed = 0;
                jumping = false;
                landing = false;
            }
        }
        if (ySpeed == 0)
        {
            if (Input.GetKey("w"))
            {
                if (coordinates.x + 1 > maxCoordinate)
                {
                    coordinates.x = maxCoordinate;
                }
                else if (!raycastBoxScript.obstacle(Vector3.left))
                {
                    anim.SetBool("isJumping", true);
                    anim.Play("Jump");
                    coordinates.x++;
                    ySpeed = jumpSpeed;
                    jumping = true;
                    startTime = Time.time;
                }
                rot = 0;
            }
            else if (Input.GetKey("s"))
            {
                if (coordinates.x - 1 < 0)
                {
                    coordinates.x = 0;
                }
                else if (!raycastBoxScript.obstacle(Vector3.right))
                {
                    anim.SetBool("isJumping", true);
                    anim.Play("Jump");
                    coordinates.x--;
                    ySpeed = jumpSpeed;
                    jumping = true;
                    startTime = Time.time;
                }
                rot = 180;
            }
            else if (Input.GetKey("d"))
            {
                if (coordinates.y + 1 > maxCoordinate)
                {
                    coordinates.y = maxCoordinate;
                }
                else if (!raycastBoxScript.obstacle(Vector3.forward))
                {
                    anim.SetBool("isJumping", true);
                    anim.Play("Jump");
                    coordinates.y++;
                    ySpeed = jumpSpeed;
                    jumping = true;
                    startTime = Time.time;
                }
                rot = 90;
            }
            else if (Input.GetKey("a"))
            {
                if (coordinates.y - 1 < 0)
                {
                    coordinates.y = 0;
                }
                else if (!raycastBoxScript.obstacle(Vector3.back))
                {
                    anim.SetBool("isJumping", true);
                    anim.Play("Jump");
                    coordinates.y--;
                    ySpeed = jumpSpeed;
                    jumping = true;
                    startTime = Time.time;
                }
                rot = -90;
            }
            else
            {
                anim.SetBool("isJumping", false);
            }

            newPosition = new Vector3(coordinates.x * (-0.4f), transform.position.y, coordinates.y * (0.4f));
        }
    }


}
