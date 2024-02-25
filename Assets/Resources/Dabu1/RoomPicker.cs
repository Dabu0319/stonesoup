using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPicker : Room
{
    public GameObject[] ValidateRooms;

    public override Room createRoom(ExitConstraint requiredExits)
    {
        List<Room> roomsThatMeetConstraints = new List<Room>();
        
        foreach (GameObject room in ValidateRooms)
        {
            
            Room roomScript = room.GetComponent<Room>();
            // if (roomScript.MeetsConstraint(requiredExits))
            // {
            //     roomsThatMeetConstraints.Add(roomScript);
            // }
            
            
        }
        
        if (roomsThatMeetConstraints.Count == 0)
        {
            return null;
        }
        
        return GlobalFuncs.randElem(roomsThatMeetConstraints).createRoom(requiredExits);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
