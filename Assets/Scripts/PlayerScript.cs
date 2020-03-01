using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public static int health;
    public static int keysCollected;
    public Text playerText;
    void Start()
    {
        health = 200;
        keysCollected = 0;
        playerText.text = "Health = " + health + "\nKeys = " + keysCollected;
    }

    void Update()
    {
        UpdateText(health, keysCollected);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Key"))
        {
            keysCollected += 1;
        }
    }

    private void UpdateText(int h, int kC)
    {
        playerText.text = "Health = " + h + "\nKeys = " + kC;
    }
}
