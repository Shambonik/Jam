using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleScript : MonoBehaviour
{

    private GameObject player;
    private float startTime;
    private float delayDuration = 30;
    public GameObject prefab;
    private bool isAttacked = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        startTime = Time.time;
        transform.position -= new Vector3(0.05f, 0, 0.05f);
    }

    // Update is called once per frame
    void Update()
    {
        if(isAttacked&&(Time.time - startTime > delayDuration * Time.deltaTime))
        {
            GameObject destroyed = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
            Destroy(this.gameObject);
        }
    }

    public void destroy()
    {
        isAttacked = true;
        player.GetComponent<Animator>().Play("Attack");
        startTime = Time.time;
    }
}
