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
    private float boxDelayDuration = 12;
    private float startTime;
    private RaycastBoxScript raycastBoxTopScript;
    private RaycastBoxScript raycastBoxBottomScript;
    private bool landing = false;
    private float blockSide = 0.4f;
    private CameraScript cameraScript;
    private LayerMask layerIgnore;
    private GameObject panelDeath;
    private GameObject textDeath;
    private GameObject box = null;
    private bool boxInHands = false;
    private bool canRotate = false;
    private string ePressed = "";
    private bool ladder = false;
    private float ladderUp = 0;
    private bool onLadder = false;

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
        raycastBoxTopScript = GameObject.FindGameObjectWithTag("RaycastBoxTop").GetComponent<RaycastBoxScript>();
        raycastBoxBottomScript = GameObject.FindGameObjectWithTag("RaycastBoxBottom").GetComponent<RaycastBoxScript>();
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
        if (ladder)
        {
            transform.position = Vector3.Lerp(transform.position, newPosition, 4f * Time.deltaTime);
            if (transform.position.y - newPosition.y > -0.01f)
            {
                ladder = false;
                ladderUp = 0;
                //anim.Play("IdleLadder");
            }
        }
        if (((Mathf.Abs(transform.rotation.eulerAngles.y - rot) < 0.02f) || (Mathf.Abs(transform.rotation.eulerAngles.y - rot - 360) < 0.02f)) && (box != null) && (box.tag == "Box"))
        {
            if (!boxInHands)
            {
                anim.Play("CratePickUpAnimation");
                box.transform.parent = transform;
                box.GetComponent<BoxCollider>().enabled = false;
                boxInHands = true;
                startTime = Time.time;
            }
            if ((Time.time - startTime) > boxDelayDuration * Time.deltaTime) box.transform.localPosition = Vector3.Lerp(box.transform.localPosition, new Vector3(-0.3f, 0.4f, 0), 0.1f);
        }
        if (jumping)
        {
            if ((Time.time - startTime) > shortDelayDuration * Time.deltaTime)
            {
                anim.SetBool("isJumping", false);
                transform.position = Vector3.Lerp(transform.position, newPosition, 3.2f * Time.deltaTime);
                transform.position = new Vector3(transform.position.x, transform.position.y + ySpeed, transform.position.z);

                if ((ySpeed < 0) && (transform.position.y - landingY < 0.1f))
                {
                    RaycastHit hit;
                    if ((!landing) && ((Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 0.2f)) || Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 0.2f, layerIgnore))) landing = true;
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
                    if (transform.position.y < -1f)
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
        if ((ySpeed == 0) && (Time.time - startTime > delayDuration * Time.deltaTime))
        {
            cameraScript.setMaterial();
            if (!boxInHands)
            {
                if (Input.GetKey("w")) rot = 0;
                else if (Input.GetKey("s")) rot = 180;
                else if (Input.GetKey("d")) rot = 90;
                else if (Input.GetKey("a")) rot = -90;
                canRotate = true;
            }
            else
            {
                if (Input.GetKey("w") && !raycastBoxBottomScript.obstacle(Vector3.left))
                {
                    rot = 0;
                    canRotate = true;
                }
                else if (Input.GetKey("s") && !raycastBoxBottomScript.obstacle(Vector3.right))
                {
                    rot = 180;
                    canRotate = true;
                }
                else if (Input.GetKey("d") && !raycastBoxBottomScript.obstacle(Vector3.forward))
                {
                    rot = 90;
                    canRotate = true;
                }
                else if (Input.GetKey("a") && !raycastBoxBottomScript.obstacle(Vector3.back))
                {
                    rot = -90;
                    canRotate = true;
                }
                else canRotate = false;
            }

            if (!Input.GetKey("e"))
            {
                ePressed = "";
                if (Input.GetKey("w"))
                {
                    if (canRotate)
                    {
                        if (boxInHands) raycastBoxTopScript.changeDelta(0.4f, 0.2f, 0);
                        else raycastBoxTopScript.setOriginalDelta();
                        if (coordinates.x + 1 > maxCoordinate)
                        {
                            coordinates.x = maxCoordinate;
                        }
                        else if (!raycastBoxTopScript.obstacle(Vector3.left))
                        {
                            GameObject bottomObject = raycastBoxBottomScript.getObject(Vector3.left);
                            if ((bottomObject == null) || ((bottomObject.tag != "Lever") && (bottomObject.tag != "Destructible")))
                            {
                                setJump(Vector3.left);
                                coordinates.x++;
                            }
                            else
                            {
                                if ((bottomObject.tag == "Lever"))
                                {
                                    bottomObject.GetComponent<LeverScript>().changePosition();
                                    delayDuration = 32;
                                }
                                else if (bottomObject.tag == "Destructible")
                                {
                                    bottomObject.GetComponent<DestructibleScript>().destroy();
                                    delayDuration = 32;
                                }
                            }
                            ladder = false;
                        }
                        else if ((raycastBoxTopScript.getObject(Vector3.left).tag == "Ladder") && (!boxInHands)&& (raycastBoxTopScript.getObject(Vector3.left).GetComponent<LadderScript>().direction == 'w'))
                        {
                            ladderUp = transform.position.y + 0.4f;
                            anim.Play("ClimbingTheLadder");
                            delayDuration = 32;
                            landingY += blockSide;
                            ladder = true;
                        }
                        startTime = Time.time;
                    }
                }
                else if (Input.GetKey("s"))
                {
                    if (canRotate)
                    {
                        if (boxInHands) raycastBoxTopScript.changeDelta(-0.4f, 0.2f, 0);
                        else raycastBoxTopScript.setOriginalDelta();
                        if (coordinates.x - 1 < 0)
                        {
                            coordinates.x = 0;
                        }
                        else if (!raycastBoxTopScript.obstacle(Vector3.right))
                        {
                            GameObject bottomObject = raycastBoxBottomScript.getObject(Vector3.right);
                            if ((bottomObject == null) || ((bottomObject.tag != "Lever") && (bottomObject.tag != "Destructible")))
                            {
                                setJump(Vector3.right);
                                coordinates.x--;
                            }
                            else
                            {
                                if ((bottomObject.tag == "Lever"))
                                {
                                    bottomObject.GetComponent<LeverScript>().changePosition();
                                    delayDuration = 32;
                                }
                                else if (bottomObject.tag == "Destructible")
                                {
                                    bottomObject.GetComponent<DestructibleScript>().destroy();
                                    delayDuration = 32;
                                }
                            }
                            ladder = false;
                        }
                        else if ((raycastBoxTopScript.getObject(Vector3.right).tag == "Ladder") && (!boxInHands) && (raycastBoxTopScript.getObject(Vector3.right).GetComponent<LadderScript>().direction == 's'))
                        {
                            ladderUp = transform.position.y + 0.4f;
                            anim.Play("ClimbingTheLadder");
                            delayDuration = 32;
                            landingY += blockSide;
                            ladder = true;
                        }
                        startTime = Time.time;
                    }
                }
                else if (Input.GetKey("d"))
                {
                    if (canRotate)
                    {
                        if (boxInHands) raycastBoxTopScript.changeDelta(0, 0.2f, 0.4f);
                        else raycastBoxTopScript.setOriginalDelta();
                        if (coordinates.y + 1 > maxCoordinate)
                        {
                            coordinates.y = maxCoordinate;
                        }
                        else if (!raycastBoxTopScript.obstacle(Vector3.forward))
                        {
                            GameObject bottomObject = raycastBoxBottomScript.getObject(Vector3.forward);
                            if ((bottomObject == null) || ((bottomObject.tag != "Lever") && (bottomObject.tag != "Destructible")))
                            {
                                setJump(Vector3.forward);
                                coordinates.y++;
                            }
                            else
                            {
                                if ((bottomObject.tag == "Lever"))
                                {
                                    bottomObject.GetComponent<LeverScript>().changePosition();
                                    delayDuration = 32;
                                }
                                else if (bottomObject.tag == "Destructible")
                                {
                                    bottomObject.GetComponent<DestructibleScript>().destroy();
                                    delayDuration = 32;
                                }
                            }
                            ladder = false;
                        }
                        else if ((raycastBoxTopScript.getObject(Vector3.forward).tag == "Ladder") && (!boxInHands) && (raycastBoxTopScript.getObject(Vector3.forward).GetComponent<LadderScript>().direction == 'd'))
                        {
                            ladderUp = transform.position.y + 0.4f;
                            anim.Play("ClimbingTheLadder");
                            delayDuration = 32;
                            landingY += blockSide;
                            ladder = true;
                        }
                        startTime = Time.time;
                    }
                }
                else if (Input.GetKey("a"))
                {
                    if (canRotate)
                    {
                        if (boxInHands) raycastBoxTopScript.changeDelta(0, 0.2f, -0.4f);
                        else raycastBoxTopScript.setOriginalDelta();
                        if (coordinates.y - 1 < 0)
                        {
                            coordinates.y = 0;
                        }
                        else if (!raycastBoxTopScript.obstacle(Vector3.back))
                        {
                            GameObject bottomObject = raycastBoxBottomScript.getObject(Vector3.back);
                            if ((bottomObject == null) || ((bottomObject.tag != "Lever") && (bottomObject.tag != "Destructible")))
                            {
                                setJump(Vector3.back);
                                coordinates.y--;
                            }
                            else
                            {
                                if ((bottomObject.tag == "Lever"))
                                {
                                    bottomObject.GetComponent<LeverScript>().changePosition();
                                    delayDuration = 32;
                                }
                                else if (bottomObject.tag == "Destructible")
                                {
                                    bottomObject.GetComponent<DestructibleScript>().destroy();
                                    delayDuration = 32;
                                }
                            }
                            ladder = false;
                        }
                        else if ((raycastBoxTopScript.getObject(Vector3.back).tag == "Ladder") && (!boxInHands) && (raycastBoxTopScript.getObject(Vector3.back).GetComponent<LadderScript>().direction == 'a'))
                        {
                            ladderUp = transform.position.y + 0.4f;
                            anim.Play("ClimbingTheLadder");
                            delayDuration = 32;
                            landingY += blockSide;
                            ladder = true;
                        }
                        else
                        {
                            Debug.Log(raycastBoxTopScript.getObject(Vector3.back).name);
                        }
                        startTime = Time.time;
                    }
                }
                else
                {
                    anim.SetBool("isJumping", false);
                }
                if (!ladder)
                {
                    newPosition = new Vector3(coordinates.x * (-blockSide), transform.position.y, coordinates.y * (blockSide));
                    Debug.Log(coordinates);
                }
                else newPosition = new Vector3(transform.position.x, ladderUp, transform.position.z);
            }
            else
            {
                if (!boxInHands)
                {
                    if (Input.GetKey("w") && ePressed != "w")
                    {
                        box = raycastBoxBottomScript.getObject(Vector3.left);
                        ePressed = "w";
                    }
                    else if (Input.GetKey("s") && ePressed != "s")
                    {
                        box = raycastBoxBottomScript.getObject(Vector3.right);
                        ePressed = "s";
                    }
                    else if (Input.GetKey("d") && ePressed != "d")
                    {
                        box = raycastBoxBottomScript.getObject(Vector3.forward);
                        ePressed = "d";
                    }
                    else if (Input.GetKey("a") && ePressed != "a")
                    {
                        box = raycastBoxBottomScript.getObject(Vector3.back);
                        ePressed = "a";
                    }
                }
                else
                {
                    if (canRotate)
                    {
                        if (Input.GetKey("w") && ePressed != "w")
                        {
                            box.transform.position = new Vector3(transform.position.x - 0.4f, transform.position.y + 0.25f, transform.position.z);
                            ePressed = "w";
                            setBoxFree();
                        }
                        else if (Input.GetKey("s") && ePressed != "s")
                        {
                            box.transform.position = new Vector3(transform.position.x + 0.4f, transform.position.y + 0.25f, transform.position.z);
                            ePressed = "s";
                            setBoxFree();
                        }
                        else if (Input.GetKey("d") && ePressed != "d")
                        {
                            box.transform.position = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z + 0.4f);
                            ePressed = "d";
                            setBoxFree();
                        }
                        else if (Input.GetKey("a") && ePressed != "a")
                        {
                            box.transform.position = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z - 0.4f);
                            ePressed = "a";
                            setBoxFree();
                        }
                    }
                }
            }
        }
            //else
            //{
            //    anim.Play("ClimbingTheLadder");
            //    newPosition = new Vector3(ladderScript.playerX * (-blockSide), transform.position.y + blockSide, ladderScript.playerZ * (blockSide));
            //    delayDuration = 60;
            //    if (ladderDirection == 'w')
            //    {
            //        rot = 0;
            //        raycastBoxTopScript.changeDelta(-0.4f, raycastBoxTopScript.getOriginalDeltaY(), 0);
            //        raycastBoxBottomScript.changeDelta(-0.4f, raycastBoxTopScript.getOriginalDeltaY(), 0);
            //        if (!raycastBoxTopScript.obstacle(Vector3.left))
            //        {
            //            setJump(Vector3.left);
            //            coordinates.x++;
            //            landingY += 0.4f;
            //            ladder = false;
            //            newPosition = new Vector3(coordinates.x * (-blockSide), transform.position.y, coordinates.y * (blockSide));
            //        }
            //    }
            //    else if(ladderDirection == 's')
            //    {
            //        rot = 180;
            //        raycastBoxTopScript.changeDelta(0.4f, raycastBoxTopScript.getOriginalDeltaY(), 0);
            //        raycastBoxBottomScript.changeDelta(0.4f, raycastBoxTopScript.getOriginalDeltaY(), 0);
            //        if (!raycastBoxTopScript.obstacle(Vector3.right))
            //        {
            //            setJump(Vector3.right);
            //            coordinates.x--;
            //            landingY += 0.4f;
            //            ladder = false;
            //            newPosition = new Vector3(coordinates.x * (-blockSide), transform.position.y, coordinates.y * (blockSide));
            //        }
            //    }
            //    else if(ladderDirection == 'd')
            //    {
            //        rot = 90;
            //        raycastBoxTopScript.changeDelta(0, raycastBoxTopScript.getOriginalDeltaY(), -0.4f);
            //        raycastBoxBottomScript.changeDelta(0, raycastBoxTopScript.getOriginalDeltaY(), -0.4f);
            //        if (!raycastBoxTopScript.obstacle(Vector3.forward))
            //        {
            //            Debug.Log("LADDER");
            //            setJump(Vector3.forward);
            //            coordinates.y++;
            //            landingY += 0.4f;
            //            ladder = false;
            //            newPosition = new Vector3(coordinates.x * (-blockSide), transform.position.y, coordinates.y * (blockSide));
            //        }
            //    }
            //    else if (ladderDirection == 'a')
            //    {
            //        rot = -90;
            //        raycastBoxTopScript.changeDelta(0, raycastBoxTopScript.getOriginalDeltaY(), 0.4f);
            //        raycastBoxBottomScript.changeDelta(0, raycastBoxTopScript.getOriginalDeltaY(), 0.4f);
            //        if (!raycastBoxTopScript.obstacle(Vector3.back))
            //        {
            //            setJump(Vector3.back);
            //            coordinates.y--;
            //            landingY += 0.4f;
            //            ladder = false;
            //            newPosition = new Vector3(coordinates.x * (-blockSide), transform.position.y, coordinates.y * (blockSide));
            //        }
            //    }
            //    startTime = Time.time;
            //}
    }

    private void setBoxFree()
    {
        anim.Play("Idle");
        box.GetComponent<BoxCollider>().enabled = true;
        boxInHands = false;
        box.GetComponent<MovableBoxScript>().drop(transform.position.y + 0.2f);
        box = null;
        startTime = Time.time;
    }

    private void setJump(Vector3 direction)
    {
        anim.SetBool("isJumping", true);
        if (boxInHands) anim.Play("CrateJumpAnimation");
        else anim.Play("Jump");
        ySpeed = jumpSpeed;
        jumping = true;
        if (raycastBoxBottomScript.obstacle(direction)) landingY+=blockSide;
        delayDuration = 8;
        //startTime = Time.time;
    }

    public float getRot()
    {
        return rot;
    }
}
