using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public float attractionRadius = 5f; // 
    public float pullForce = 20f; // 

    
    private List<BoxCollider2D> modifiedColliders = new List<BoxCollider2D>();
    void Update()
    {
        // detect all tiles within the attraction radius and apply a force to them
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attractionRadius);
        
        
        
        
        
        foreach (Collider2D hit in hits)
        {
            Tile tile = hit.GetComponent<Tile>();
            
            Debug.Log("Tile: " + tile);
            if (tile != null)
            {

                if (tile.hasTag(TileTags.Creature))
                {
                    tile.enabled = false;
                }

                if (tile.hasTag(TileTags.Player))
                {
                    tile.enabled = false;
                    //tile.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                }
                
                Rigidbody2D rb = tile.GetComponent<Rigidbody2D>();

                if (rb ==null)
                {
                    rb = tile.gameObject.AddComponent<Rigidbody2D>();
                    rb.gravityScale = 0;
                    rb.isKinematic = false; 
                }
                
                BoxCollider2D boxCollider2D = tile.GetComponent<BoxCollider2D>();
                if (boxCollider2D != null && !boxCollider2D.isTrigger) {
                    boxCollider2D.isTrigger = true;
                    modifiedColliders.Add(boxCollider2D);
                }
                
                    
                
                    
                    // direction and distance
                    Vector2 direction = (transform.position - tile.transform.position).normalized;
                    float distance = Vector2.Distance(transform.position, tile.transform.position);
                    // add force
                    float forceMagnitude = Mathf.Clamp01((attractionRadius - distance) / attractionRadius) * pullForce;
                    rb.AddForce(direction * forceMagnitude, ForceMode2D.Force);
                

                // detect if the tile is close enough to be destroyed
                if (Vector2.Distance(tile.transform.position, transform.position) < 0.5f)
                {
                    tile.takeDamage(Player.instance , 100);
                    Destroy(tile.gameObject);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attractionRadius);
    }
    
    
    void OnDestroy()
    {
        
        foreach (BoxCollider2D collider in modifiedColliders)
        {
            if (collider != null) {
                collider.isTrigger = false;
            }

            if (GetComponent<Rigidbody2D>() != null)
            {
                //set velocity to zero
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            
            //enable the tile
            Tile tile = collider.GetComponent<Tile>();
            if (tile != null)
            {
                tile.enabled = true;
            }

        }
    }
}