using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMove : Tile
{
    
    public enum MoveDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    public MoveDirection moveDirection;
    
    public float speed = 1f;
    
    //public List<bool> moveDirection = new List<bool>();
    
    
    
    void Start()
    {
        
        //random vertical move or horizontal move, four directions, only one direction
        // for (int i = 0; i < 4; i++)
        // {
        //     moveDirection.Add(false);
        // }
        // moveDirection[Random.Range(0, 4)] = true;
        
        
        
        
    }

    // Update is called once per frame
    void Update()
    {


        switch (moveDirection)
        {
            case MoveDirection.Up:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.Translate(Vector3.up * speed * Time.deltaTime);
                break;
            case MoveDirection.Down:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.Translate(Vector3.down * speed * Time.deltaTime);
                break;
            case MoveDirection.Left:
                transform.rotation = Quaternion.Euler(0, 0, -90);
                transform.Translate(Vector3.down * speed * Time.deltaTime);
                break;
            case MoveDirection.Right:
                transform.rotation = Quaternion.Euler(0, 0, -90);
                transform.Translate(Vector3.up * speed * Time.deltaTime);
                break;
        }
        
        
    }
}
