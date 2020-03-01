using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerScript : MonoBehaviour
{
    public float moveSpeed;
    private Vector3 startPos;
    private Vector3 endPos;

    void Start()
    {
        moveSpeed = 5.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("GO");
            startPos = transform.position;
            endPos = transform.position + new Vector3(-0.5f, 1.0f, 2.0f);
            Debug.Log("startPos = " + startPos);
            Debug.Log("endPos = " + endPos);
            //transform.position = Vector3.MoveTowards(startPos, endPos, Time.deltaTime * moveSpeed);
            //transform.position = endPos;
            //transform.position = Vector3.MoveTowards(transform.position, startPos, Time.deltaTime * moveSpeed);
            //transform.position = startPos;
            //Debug.Log("moved back = " + transform.position);
            StartCoroutine(Strike(0.5f));
            //Debug.Log("DONE");
        }
    }

    private IEnumerator Strike(float waitTime)
    {
        Debug.Log("STRIKE");
        //transform.position = Vector3.MoveTowards(startPos, endPos, Time.deltaTime * moveSpeed);
        transform.position = endPos;
        Debug.Log("position after move up = " + transform.position);
        yield return new WaitForSeconds(waitTime);
        //transform.position = Vector3.MoveTowards(transform.position, startPos, Time.deltaTime * moveSpeed);
        transform.position = startPos;
        Debug.Log("position after move back = " + transform.position);
    }
}
