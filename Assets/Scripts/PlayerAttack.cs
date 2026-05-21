using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public float attackRange = 1.5f;

    public int damage = 10;

    public SpriteRenderer spriteRenderer;

    // Update is called once per frame
    void Update()
    {
        
    }

    void PerformAttack() 
    {
        Vector2 attackDirection = spriteRenderer.flipX ? Vector2.left : Vector2.right;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (Collider2D hitCollider in hitColliders) 
        {
            if(hitCollider.CompareTag("Enemy"))
            {
                Vector2 directionToEnemy = (hitCollider.transform.position - transform.position).normalized;

                if(Vector2.Dot(attackDirection, directionToEnemy) > 0 )
                {
                    // Retirer vie à l'ennemi !
                    Debug.Log("L'ennemi subit une attaque !");
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
