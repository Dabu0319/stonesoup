using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChameleonCloak : Tile
{
    public bool isInvincible = false;
    public float castTime = 0.5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void CountDownTimer(float time)
    {
        StartCoroutine(CountDown(time));
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    IEnumerator CountDown(float time)
    {
        yield return new WaitForSeconds(time);
        
    }
    
    public void ActivateCloak()
    {
        CountDownTimer(castTime);
        BecomeInvisible();
    }
    
    public void BecomeInvisible()
    {
        gameObject.GetComponentInParent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
        isInvincible = true;
    }
}
