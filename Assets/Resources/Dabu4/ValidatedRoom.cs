using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidatedRoom : Room
{
    public bool hasUpExit;
    public bool hasDownExit;
    public bool hasLeftExit;
    public bool hasRightExit;

    public bool hasLeftToUpPath;
    public bool hasLeftToDownPath;
    public bool hasLeftToRightPath;
    public bool hasRightToUpPath;
    public bool hasRightToDownPath;
    public bool hasUpToDownPath;

    public bool MeetsConstraint(ExitConstraint requiredExits)
    {
        if (requiredExits.upExitRequired && !hasUpExit)
            return false;

        if (requiredExits.downExitRequired && !hasDownExit)
            return false;

        if (requiredExits.leftExitRequired && !hasLeftExit)
            return false;

        if (requiredExits.rightExitRequired && !hasRightExit)
            return false;

        if (requiredExits.leftExitRequired && requiredExits.upExitRequired && !hasLeftToUpPath)
            return false;

        if (requiredExits.leftExitRequired && requiredExits.downExitRequired && !hasLeftToDownPath)
            return false;

        if (requiredExits.leftExitRequired && requiredExits.rightExitRequired && !hasLeftToRightPath)
            return false;

        if (requiredExits.rightExitRequired && requiredExits.upExitRequired && !hasRightToUpPath)
            return false;

        if (requiredExits.rightExitRequired && requiredExits.downExitRequired && !hasRightToDownPath)
            return false;

        if (requiredExits.upExitRequired && requiredExits.downExitRequired && !hasUpToDownPath)
            return false;


        return true;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        FindPaths();
    }

    private void FindPaths()
    {
        
    }

    bool FindPath(Vector2Int startPoint, Vector2 endPoint)
    {
        List<Vector2Int> openSet = new List<Vector2Int>();
        List<Vector2Int> closedSet = new List<Vector2Int>();
        
        openSet.Add(startPoint);

        while (openSet.Count > 0)
        {
            Vector2Int currentPoint = openSet[0];
            if (currentPoint == endPoint)
            {
                return true;
            }

            for (int x = -1; x <=1 ; x++)
            {
                for (int y = -1; y < 1; y++)
                {
                    if ( x!= 0 && y != 0)
                    {
                        Vector2Int newPoint = new Vector2Int(x, y);
                        if (openSet.Contains(newPoint) == false && closedSet.Contains(newPoint) == false)
                        {
                            openSet.Add(newPoint);
                        }
                        
                    }
                
                }
            }
            
            closedSet.Add(currentPoint);
        }
        
        return false;
    }
}
