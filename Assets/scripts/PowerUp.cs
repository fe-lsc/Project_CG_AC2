using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    private Quaternion startPoint;
    public bool powerUpActive = false;

    // Update is called once per frame
    void Update()
    {
    
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            SendMessageUpwards("PowerUpPickup", SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }
    }
}
