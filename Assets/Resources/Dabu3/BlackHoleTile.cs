using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleTile : Tile
{
    public float attractionRadius = 1f;
    public float pullForce = 50f;
    public float radiusGrowthRate = 0.5f;
    public float scaleGrowthRate = 0.5f;

    public float destroyDis = 1f;
    
    
    private List<BoxCollider2D> modifiedColliders = new List<BoxCollider2D>();

    void Update()
    {
        attractionRadius += radiusGrowthRate * Time.deltaTime;
        transform.localScale += new Vector3(scaleGrowthRate, scaleGrowthRate, 0) * Time.deltaTime;
        destroyDis += scaleGrowthRate/4 * Time.deltaTime;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attractionRadius);
        foreach (Collider2D hit in hits)
        {
            Tile tile = hit.GetComponent<Tile>();
            
            //Debug.Log("Tile: " + tile);
            if (tile != null)
            {

                if (tile.hasTag(TileTags.Creature) && !tile.hasTag(TileTags.Player))
                {
                    tile.enabled = false;
                }

                if (tile.hasTag(TileTags.Player))
                {
                    //tile.enabled = false;
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
                    //boxCollider2D.isTrigger = true;
                    modifiedColliders.Add(boxCollider2D);
                }
                
                    
                
                    
                    // direction and distance
                    Vector2 direction = (transform.position - tile.transform.position).normalized;
                    float distance = Vector2.Distance(transform.position, tile.transform.position);
                    // add force
                    float forceMagnitude = Mathf.Clamp01((attractionRadius - distance) / attractionRadius) * pullForce;
                    rb.AddForce(direction * forceMagnitude, ForceMode2D.Force);
                

                    if (tile != this && !tile.GetComponent<BlackHoleTile>() && Vector2.Distance(tile.transform.position, transform.position) < destroyDis)
                    {
                        tile.takeDamage(Player.instance, 100); 
                        Debug.Log("Destroying Tile: " + tile);
                        Destroy(tile.gameObject);
                    }
            }
        }

    }

    void OnDrawGizmos()
    {
        // 使用Gizmos在Unity编辑器中绘制表示吸引半径的圆圈
        Gizmos.color = Color.red; // 设置Gizmos颜色为红色
        Gizmos.DrawWireSphere(transform.position, attractionRadius); // 绘制圆圈
    }
} 