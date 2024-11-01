using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBoss : Tile
{
    public GameObject laserMovePrefab;
    public GameObject laserRotatePrefab;
    
    public Vector2 roomSize;
    
    public Vector2 changeOffset;
    private Vector2 _startOffset = new Vector2(0, 0);
    void Start()
    {
        _startOffset = transform.parent.position + new Vector3(changeOffset.x,changeOffset.y);
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     
        //     GameObject newLaser = Instantiate(laserMovePrefab, transform.position, Quaternion.identity,transform);
        // }
        //
        if (Input.GetMouseButtonDown(1))
        {
            GameObject newLaser0 = Instantiate(laserRotatePrefab, transform.position, Quaternion.identity  ,transform);
            //rotate 120 degree
            
            GameObject newLaser1 = Instantiate(laserRotatePrefab, transform.position, Quaternion.identity  ,transform);
            newLaser1.transform.rotation = Quaternion.Euler(0, 0, 120);
            
            GameObject newLaser2 = Instantiate(laserRotatePrefab, transform.position, Quaternion.identity  ,transform);
            newLaser2.transform.rotation = Quaternion.Euler(0, 0, 240);
            
            
        }


        if (Input.GetKeyDown(KeyCode.J))
        {
            //instantiate the laser at the left of the room
            GameObject newLaser = Instantiate(laserMovePrefab,  new Vector3(0 + _startOffset.x, roomSize.y+_startOffset.y, 0), Quaternion.identity, transform);
            newLaser.GetComponent<LaserMove>().moveDirection = LaserMove.MoveDirection.Right;
        }
        
        if (Input.GetKeyDown(KeyCode.L))
        {
            //instantiate the laser at the right of the room
            GameObject newLaser = Instantiate(laserMovePrefab,  new Vector3(roomSize.x +_startOffset.x, roomSize.y+_startOffset.y, 0), Quaternion.identity, transform);
            newLaser.GetComponent<LaserMove>().moveDirection = LaserMove.MoveDirection.Left;
        }
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            //instantiate the laser at the top of the room
            GameObject newLaser = Instantiate(laserMovePrefab,  new Vector3(  _startOffset.x, roomSize.y+_startOffset.y, 0), Quaternion.identity, transform);
            newLaser.GetComponent<LaserMove>().moveDirection = LaserMove.MoveDirection.Down;
        }
        
        if (Input.GetKeyDown(KeyCode.K))
        {
            //instantiate the laser at the bottom of the room
            GameObject newLaser = Instantiate(laserMovePrefab,  new Vector3(_startOffset.x,_startOffset.y, 0), Quaternion.identity, transform);
            newLaser.GetComponent<LaserMove>().moveDirection = LaserMove.MoveDirection.Up;
        }

    }
}
