using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class db10Minotaur : BasicAICreature {
    public float chargeSpeed = 20f;
    public float maxChargeDistance = 10f;
    public float cooldownAfterCharge = 2f;
    public float chargeUpTime = 1f; // 蓄力时间

    private Vector2 chargeDirection;
    private float chargeDistanceCovered = 0f;
    
    [SerializeField]
    private bool isCharging = false;
    
    [SerializeField]
    private bool isChargingUp = false; // 是否正在蓄力
    
    
    [SerializeField]
    private float cooldownTimer = 0f;
    private float chargeUpTimer = 0f; // 蓄力计时器
    
    [SerializeField]
    private Tile _tileWereChasing;
    
    public int damageAmount = 1;

    public Tile tileWereChasing {
        get { return _tileWereChasing; }
        set { _tileWereChasing = value; }
    }
    
    public void tileDetected() {
        if (_tileWereChasing == null) {
            _tileWereChasing = FindObjectOfType<Player>();
            PrepareCharge(); 
        }
        else
        {
            PrepareCharge();
        }
    }

    public override void Start() {
        tileDetected();
    }

    public override void FixedUpdate() {
        if (isChargingUp) {
            chargeUpTimer += Time.deltaTime;
            if (chargeUpTimer >= chargeUpTime) {
                
                StartCharge();
            }
        } else if (isCharging) {
            float step = chargeSpeed * Time.deltaTime;
            Vector2 newPosition = GetComponent<Rigidbody2D>().position + chargeDirection * step;
            GetComponent<Rigidbody2D>().MovePosition(newPosition);
            chargeDistanceCovered += step;

            if (chargeDistanceCovered >= maxChargeDistance) {
                EndCharge();
            }
        } else if (cooldownTimer > 0) {
            cooldownTimer -= Time.deltaTime;
        }

        
        if (cooldownTimer <= 0 && !isCharging && !isChargingUp) {
            tileDetected(); 
        }

        if (isChargingUp)
        {
            Debug.Log("Charging up");
            //the g and b values are set to 0 to make the color red linearly using lerping
            float g = Mathf.Lerp(1, 0, chargeUpTimer / chargeUpTime);
            float b =  Mathf.Lerp(1, 0, chargeUpTimer / chargeUpTime);
            
            //Debug.Log(GetComponent<SpriteRenderer>().color);
            GetComponent<SpriteRenderer>().color = new Color(1, g, b, 1);
            
            //GetComponent<SpriteRenderer>().color = Color.red;
            //Debug.Log(GetComponent<SpriteRenderer>().color);
        }
        
        // if (isCharging)
        // {
        //     
        //     GetComponent<SpriteRenderer>().color = Color.white;
        // }
    }



    void PrepareCharge() {
        if (_tileWereChasing != null && !isCharging && cooldownTimer <= 0) {
            isChargingUp = true;
            

            
            
            
            chargeUpTimer = 0f; // 
        }
    }

    void StartCharge() {
        chargeDirection = (_tileWereChasing.transform.position - transform.position).normalized;
        isCharging = true;
        isChargingUp = false; // 
        chargeDistanceCovered = 0f;
    }

    void EndCharge() {
        isCharging = false;
        cooldownTimer = cooldownAfterCharge;
        
        GetComponent<SpriteRenderer>().color = Color.white;
        if (GetComponent<Rigidbody2D>() != null) {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero; 
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Tile otherTile = collision.gameObject.GetComponent<Tile>();
        
        if (otherTile.hasTag(TileTags.Player)) {
            otherTile.takeDamage(this, damageAmount);
        }
        
        if (isCharging && otherTile.hasTag(TileTags.Wall)) {
            Debug.Log("Hit wall");
            EndCharge();
        }
    }
}