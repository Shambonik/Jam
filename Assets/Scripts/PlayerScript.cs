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
    private float shortDelayDuration = 4;
    private float startTime;
    private RaycastBoxScript raycastBoxTop;
    private RaycastBoxScript raycastBoxBottom;
    private bool landing = false;
    private float blockSide = 0.4f;
    private CameraScript cameraScript;
    private LayerMask layerIgnore;
    private GameObject panelDeath;
    private GameObject textDeath;

    void Start()
    {
        newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        landingY = transform.position.y;
        anim = transform.GetComponent<Animator>();
        textDeath = GameObject.FindGameObjectWithTag("TextDeath");
        panelDeath = GameObject.FindGameObjectWithTag("PanelDeath");
        panelDeath.SetActive(false);
        textDeath.SetActive(false);
        startTime = Time.time;
        raycastBoxTop = GameObject.FindGameObjectWithTag("RaycastBoxTop").GetComponent<RaycastBoxScript>();
        raycastBoxBottom = GameObject.FindGameObjectWithTag("RaycastBoxBottom").GetComponent<RaycastBoxScript>();
        cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>();
        layerIgnore = LayerMask.GetMask("Ignore Raycast");
    }

    public float getLandingY()
    {
        return landingY;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, rot, transform.eulerAngles.z), 800 * Time.deltaTime);
        if (jumping) { 
            if ((Time.time - startTime) > shortDelayDuration * Time.deltaTime)
            {
                anim.SetBool("isJumping", false);
                transform.position = Vector3.Lerp(transform.position, newPosition, 3.2f * Time.deltaTime);
                transform.position = new Vector3(transform.position.x, transform.position.y + ySpeed, transform.position.z);

                if ((ySpeed < 0) && (transform.position.y - landingY < 0.1f))
                {
                    RaycastHit hit;
                    if ((!landing) && (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 0.2f)) || Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 0.2f, layerIgnore)) landing = true;
                    if (transform.position.y - landingY < 0.05f)
                    {
                        if (landing)
                        {
                            transform.position = new Vector3(newPosition.x, landingY, newPosition.z);
                            ySpeed = 0;
                            jumping = false;
                            landing = false;
                            startTime = Time.time;
                        }
                        else landingY -= blockSide;
                    }
                }
                else
                {
                    ySpeed -= g;
                    if(transform.position.y < -1f)
                    {
                        panelDeath.SetActive(true);
                        textDeath.SetActive(true);
                        ySpeed = 0;
                        jumping = false;
                        landing = false;
                    }
                }
            }
        }
        if ((ySpeed == 0)&&(Time.time-startTime > delayDuration * Time.deltaTime))
        {
            cameraScript.setMaterial();
            if (Input.GetKey("w"))
            {
                if (coordinates.x + 1 > maxCoordinate)
                {
                    coordinates.x = maxCoordinate;
                }
                else if (!raycastBoxTop.obstacle(Vector3.left))
                {
                    setJump(Vector3.left);
                    coordinates.x++;
                }
                rot = 0;
            }
            else if (Input.GetKey("s"))
            {
                if (coordinates.x - 1 < 0)
                {
                    coordinates.x = 0;
                }
                else if (!raycastBoxTop.obstacle(Vector3.right))
                {
                    setJump(Vector3.right);
                    coordinates.x--;
                }
                rot = 180;
            }
            else if (Input.GetKey("d"))
            {
                if (coordinates.y + 1 > maxCoordinate)
                {
                    coordinates.y = maxCoordinate;
                }
                else if (!raycastBoxTop.obstacle(Vector3.forward))
                {
                    setJump(Vector3.forward);
                    coordinates.y++;
                }
                rot = 90;
            }
            else if (Input.GetKey("a"))
            {
                if (coordinates.y - 1 < 0)
                {
                    coordinates.y = 0;
                }
                else if (!raycastBoxTop.obstacle(Vector3.back))
                {
                    setJump(Vector3.back);
                    coordinates.y--;
                }
                rot = -90;
            }
            else
            {
                anim.SetBool("isJumping", false);
            }

            newPosition = new Vector3(coordinates.x * (-blockSide), transform.position.y, coordinates.y * (blockSide));
        }

    }

    private void setJump(Vector3 direction)
    {
        anim.SetBool("isJumping", true);
        anim.Play("Jump");
        ySpeed = jumpSpeed;
        jumping = true;
        if (raycastBoxBottom.obstacle(direction)) landingY+=blockSide;
        startTime = Time.time;
    }

}
