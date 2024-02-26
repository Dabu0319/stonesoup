using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiVisibleLight : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<ChameleonCloak>())
        {
            other.GetComponent<ChameleonCloak>().cloakActive = false;
        }
        
        Debug.Log("Entered");
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<ChameleonCloak>())
        {
            other.GetComponent<ChameleonCloak>().cloakActive = true;
        }
        
        Debug.Log("Exited");
    }
}
