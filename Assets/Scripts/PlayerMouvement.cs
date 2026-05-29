using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMouvement : MonoBehaviour
{
    public float moveSpeed = 0.1f;
    public float attackRange = 1.5f;
    public int damage = 1;
    public int knockbackForce = 5;

    public Rigidbody2D rb2D;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    private Transform _transform;
    public PlayerHealth playerHealth;

    [SerializeField] InputPlayerMouvement _input;
    [SerializeField] Vector2 _movement;

    private void Awake()
    {
        //Debug.Log("Awake starts!");

        _input = new InputPlayerMouvement();
        //Debug.Log("_input initialized!");

        InputListeningOn();
        //Debug.Log("Listening to Inputs!");

        _input.Player.Enable();
        //Debug.Log("_input Enabled!");

        //Debug.Log("Awake finished!");
    }

    private void Start()
    {
        //Debug.Log("Start starts!");

        _transform = rb2D.GetComponent<Transform>();
        //Debug.Log("_transform got rigidboby<Transform>!");

        //Debug.Log("Start Finished!");
    }

    void Update()
    {
        animator.SetFloat("Speed", _movement.sqrMagnitude);

        if(_movement.x != 0)
        {
            spriteRenderer.flipX = _movement.x < 0;
        }
    }

    private void FixedUpdate()
    {
        ApplyMove();
    }


    private void OnDestroy()
    {
        InputListeningOff();
        //Debug.Log("Listening to Inputs No More!");
        _input.Player.Disable();
        //Debug.Log("Input Player Disabled!");
    }

    public void MultiplySpeed(float speedMultiplier)
    {
        moveSpeed *= speedMultiplier;
    }
    
    public void MultiplyDmg(float dmgMultiplier)
    {
        damage = Mathf.CeilToInt(damage*dmgMultiplier);
    }
    private void ApplyMove()
    {
        _transform.position += new Vector3(_movement.x,_movement.y,0) * moveSpeed;
    }

    void PerformAttack()
    {
        // Active le trigger d'animation
        animator.SetTrigger("Attack");

        // Identifie si on attaque vers la gauche ou vers la droite
        Vector2 attackDirection = spriteRenderer.flipX ? Vector2.left : Vector2.right;

        // Chope tous les elements en contact
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange);

        // Affine selection
        foreach (Collider2D hitCollider in hitColliders)
        {
            // Spécifique au ennemis :
            if (hitCollider.CompareTag("Enemy"))
            {
                // Identifie si l'enemmi est à gauche ou à droite
                Vector2 directionToEnemy = (hitCollider.transform.position - transform.position).normalized;

                // Compare les direction pour vérifier si l'ennemi est du bon côté
                if (Vector2.Dot(attackDirection, directionToEnemy) > 0)
                {
                    // Retire de la vie à l'ennemi le cas échéant !
                    EnemyAI enemyAI = hitCollider.GetComponent<EnemyAI>();
                    enemyAI.TakeDamage(damage);
                    Debug.Log("L'ennemi subit une attaque !");

                    Vector2 knockbackDirection = (hitCollider.transform.position - transform.position).normalized;
                    enemyAI.rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                }
            }
        }
    }

    // Bonus juste pour visualiser la portée d'attaque
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }


    #region Setup input listener
    private void InputListeningOn()
    {
        //Debug.Log("Start InputSuscription");
        _input.Player.Move.performed += onMovePerformed;
        _input.Player.Move.canceled += onMoveCanceled;
        _input.Player.Attack.performed += onAttackPerformed;
        //Debug.Log("InputSuscription Finished");
    }
    private void InputListeningOff()
    {
        //Debug.Log("Start InputUnsuscription");
        _input.Player.Move.performed -= onMovePerformed;
        _input.Player.Move.canceled -= onMoveCanceled;
        _input.Player.Attack.performed -= onAttackPerformed;
        //Debug.Log("InputUnsuscription Finished");
    }

    private void onMovePerformed(InputAction.CallbackContext context)
    {
        //Debug.Log("Start onMovePerformed");
        if (playerHealth.isAlive)
        {
            _movement = context.ReadValue<Vector2>(); 
        }
        //Debug.Log("Stop onMovePerformed");
    }

    private void onMoveCanceled(InputAction.CallbackContext context)
    {
        //Debug.Log("Start onMoveCanceled");
        _movement = Vector2.zero;
        //Debug.Log("Stop onMoveCanceled");
    } 

    private void onAttackPerformed(InputAction.CallbackContext context)
    {
        //Debug.Log("Start onAttackPerformed");
        if (playerHealth.isAlive)
        {
            PerformAttack(); 
        }
        //Debug.Log("Stop onAttackPerformed");
    }

    #endregion

}
