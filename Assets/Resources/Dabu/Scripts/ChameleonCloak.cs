using System.Collections;
using UnityEditor;
using UnityEngine;

public class ChameleonCloak : Tile
{
    public bool isCloaked = false;
    private Vector3 lastPosition;
    private float stationaryTime = 0f;
    public float cloakActivationDelay = 0.5f;
    public float castTime = 0.5f; // Time it takes to activate the cloak

    void Start()
    {
        lastPosition = transform.position; // Initialize last position
    }

    void Update()
    {
        // Check if the player has moved
        if (transform.position == lastPosition)
        {
            // Accumulate the time the player has been stationary
            stationaryTime += Time.deltaTime;
            if (stationaryTime >= cloakActivationDelay && !isCloaked)
            {
                // Activate cloak if not already cloaked and stationary for more than 3 seconds
                ActivateCloak();
            }
        }
        else
        {
            // If the player moved, reset the stationary time and deactivate the cloak if it was active
            if (isCloaked)
            {
                DeactivateCloak();
            }
            stationaryTime = 0f;
        }
        // Update the last position for the next frame
        lastPosition = transform.position;
    }

    IEnumerator CountDown(float time)
    {
        yield return new WaitForSeconds(time);
        BecomeInvisible();
    }

    public void ActivateCloak()
    {
        BecomeInvisible(); // Wait for castTime before becoming invisible
        isCloaked = true;
    }

    public void DeactivateCloak()
    {
        // Instantly become visible and set isCloaked to false
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        //player becomes visible
        GameObject.FindObjectOfType<Player>().GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        isCloaked = false;
        RecoverEnemyVision();
        
        
    }

    void BecomeInvisible()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);

        if (FindObjectOfType<Player>().tileWereHolding ==this)
        {
            FindObjectOfType<Player>().GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
            ClearEnemyVision();
        }
        

    }
    
    
    //tbh I don't think it's efficient but there is no other manager to manage all enemies
    //I'll figure out a way to do it later
    void ClearEnemyVision()
    {
        // Clear the vision of all enemies
        apt283FollowEnemy[] enemies = GameObject.FindObjectsOfType<apt283FollowEnemy>();
        foreach (apt283FollowEnemy enemy in enemies)
        {
            //enemy.tileWereChasing = null;
            enemy.maxDistanceToContinueChase = 0;
            
            Debug.Log(enemy.name);
            
        }
        
        Debug.Log("Cleared enemy vision");
    }
    
    void RecoverEnemyVision()
    {
        // Rechase the player
        apt283FollowEnemy[] enemies = GameObject.FindObjectsOfType<apt283FollowEnemy>();
        foreach (apt283FollowEnemy enemy in enemies)
        {
            //enemy.tileWereChasing = FindObjectOfType<Player>();
            enemy.maxDistanceToContinueChase = 14;
            
        }
        
        Debug.Log("Recovered enemy vision");
    }
}