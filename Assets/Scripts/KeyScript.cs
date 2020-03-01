using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KeyScript : MonoBehaviour
{

    public float yPositionChange;
    private float yPositionMax;
    private float yPositionMin;

    void Start()
    {
        yPositionChange = 0.05f;
        yPositionMax = transform.position.y + 0.25f;
        yPositionMin = transform.position.y - 0.25f;
    }


    void Update()
    {
        transform.position += new Vector3(0, yPositionChange, 0);
        if (transform.position.y >= yPositionMax || transform.position.y <= yPositionMin)
        {
            yPositionChange *= -1;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
