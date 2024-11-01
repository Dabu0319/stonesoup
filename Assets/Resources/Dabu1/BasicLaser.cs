using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BasicLaser : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Vector3 _originalPos;

    public Transform  parent;
    
    public float lineLength = 20;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        _originalPos = lineRenderer.GetPosition(1);
        parent = transform.parent;
        
    }

    // Update is called once per frame
    void Update()
    {
            
        lineRenderer.SetPosition(0, transform.position);
        
        
        //lineRenderer.SetPosition(1, transform.position +parent.right * lineLength);
        
        //if hit something
        RaycastHit2D hit = Physics2D.Raycast(transform.position, parent.right, parent.right.magnitude * lineLength,LayerMask.GetMask("Wall"));
        
        if (hit.collider != null && hit.collider.GetComponent<Tile>().hasTag(TileTags.Wall))
        {
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            lineRenderer.SetPosition(1, transform.position + parent.right * lineLength);
        }
        
        
        //rotate with parent
        //Quaternion rotation = Quaternion.Euler(0, 0, transform.parent.rotation.eulerAngles.z);
        
        
    }
}
