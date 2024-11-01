using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRotate : Tile
{
    public float speed = 1f;
    
    public bool isRotate = true;
    
    void Start()
    {
        //change the random rotation first
        //transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        

    }


    // Update is called once per frame
    void Update()
    {
        

        
         //rotate the laser
         if (isRotate)
         {
             transform.Rotate(Vector3.forward * speed * Time.deltaTime);
         }
        
    }
}
