using System.Collections;
using UnityEngine;



    public class BlackHoleBullet : Tile {
        public float onGroundThreshold = 1f;
        public float explosionRadius = 5f; // 爆炸半径
        public float attractionDuration = 0.5f; // 吸引持续时间

        protected float _destroyTimer = 0.2f;

        void Start() {
            if (GetComponent<TrailRenderer>() != null) {
                GetComponent<TrailRenderer>().Clear();
            }
        }

        void Update() {
            if (_body.velocity.magnitude <= onGroundThreshold) {
                _destroyTimer -= Time.deltaTime;
                if (_destroyTimer <= 0) {
                    die();
                }
            }
        }

        public virtual void OnCollisionEnter2D(Collision2D collision) {
            //AttractAndDestroyTiles();
            die();
        }

        // private void AttractAndDestroyTiles() {
        //     // 
        //     Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        //     foreach (Collider2D hitCollider in hitColliders) {
        //         Tile tile = hitCollider.GetComponent<Tile>();
        //         if (tile != null) {
        //             // 
        //             // 
        //
        //             // 
        //             StartCoroutine(MoveTileToPosition(tile));
        //         }
        //     }
        // }
        //
        // IEnumerator MoveTileToPosition(Tile tile) {
        //     float timer = 0;
        //     while (timer < attractionDuration) {
        //         if (tile != null && tile.gameObject != null) {
        //             tile.transform.position = Vector2.MoveTowards(tile.transform.position, transform.position, Time.deltaTime * (explosionRadius / attractionDuration));
        //         }
        //         timer += Time.deltaTime;
        //         yield return null;
        //     }
        //     
        //     if (tile != null && tile.gameObject != null) {
        //         
        //         tile.takeDamage(this, 1);
        //     }
        // }
    }
