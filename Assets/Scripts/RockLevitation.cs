using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockLevitation : MonoBehaviour
{

        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private EdgeCollider2D rockCollider;
        [SerializeField] protected float colliderDistance;
        [SerializeField] protected float range;
        [SerializeField] private GameObject cursor;

        private SpriteRenderer spriteRenderer;
        private GameObject playerObject = null;

        private void Start() {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update() {
            if(PlayerInsight()){
                MoveRock();
            }
        }


    public bool PlayerInsight()
    {
        // Check if player is insight
        RaycastHit2D raycastHit = Physics2D.BoxCast(rockCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, new Vector2(rockCollider.bounds.size.x * range, rockCollider.bounds.size.y), 0, Vector2.left, 0, playerLayer);
        if (raycastHit.collider != null)
        {
            // Object detected, prepare to drag
            playerObject = raycastHit.collider.gameObject;
        }
        return raycastHit.collider != null;
    }
     private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(rockCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, new Vector2(rockCollider.bounds.size.x * range, rockCollider.bounds.size.y));

    }

     private void MoveRock(){
        if (playerObject != null)
        {
                if (spriteRenderer != null) {
        spriteRenderer.color = Color.green;
    }
    Animator playerAnimator = playerObject.GetComponent<Animator>();
    

            if (Input.GetMouseButton(1))
            {
                if (playerAnimator != null) {
                            playerAnimator.SetBool("levitation", true);
                        }
                // animator.SetBool("levitation", true);
                Vector2 cursorPosition = cursor.transform.position;
                transform.position = cursorPosition;
            } else if (Input.GetMouseButtonUp(1))
            {
                if (playerAnimator != null) {
                            playerAnimator.SetBool("levitation", false);
                        }
                // animator.SetBool("levitation", false);
                playerObject = null;
            }
        }

    }
}
