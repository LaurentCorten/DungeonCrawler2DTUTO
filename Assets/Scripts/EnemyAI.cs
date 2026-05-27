using Pathfinding;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // Cible que l'ennemi doit suivre (par exemple, le joueur)
    public Transform target;

    // Vitesse de déplacement de l'ennemi
    public float speed = 120f;

    // Distance à laquelle l'ennemi considère qu'il a atteint un waypoint
    public float nextWpDistance = 1f;

    // Gestion des attaques
    public float attackCD = 2f;
    float _currentCD;
    public int enemyDmg = 1;
    public int maxHealth = 2;
    [SerializeField] private int _health;
    private bool isAlive = true; 

    // Distance minimale pour déclencher une attaque (l'ennemi s'arrête en dehors de cette portée)
    public float attackRange = 1.5f;
    public float detectionRange = 7f;

    // Chemin calculé par le Seeker
    public Path path;

    // Indice du waypoint actuel que l'ennemi essaie d'atteindre
    int currWp = 0;

    // Composant Seeker utilisé pour calculer le chemin
    public Seeker seeker;

    // Composant Rigidbody2D utilisé pour le mouvement physique de l'ennemi
    public Rigidbody2D rb;

    // Composant d'animation
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    public GameObject coinPrefab;

    private void Awake()
    {
        _health = maxHealth;
    }

    // Méthode appelée au début de l'exécution
    void Start()
    {
        // Met à jour le chemin toutes les 0,5 secondes pour suivre la position du joueur
        InvokeRepeating("UpdatePath", 0, 0.5f);
    }

    // Méthode pour mettre à jour le chemin vers la cible
    void UpdatePath()
    {
        // Vérifie si le Seeker est prêt à calculer un nouveau chemin
        if (isAlive && seeker.IsDone() && Vector2.Distance(transform.position, target.position) <= detectionRange)
            // Demande un nouveau chemin du Seeker entre la position actuelle et la cible
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    // Méthode appelée lorsque le Seeker a terminé de calculer un chemin
    void OnPathComplete(Path p)
    {
        // Si le calcul a réussi (pas d'erreur), met à jour le chemin et réinitialise l'indice du waypoint
        if (!p.error)
        {
            path = p;
            currWp = 0;
        }
    }

    // Setup des animations
    private void Update()
    {
        if (!isAlive) return;


        animator.SetFloat("Speed", rb.linearVelocity.sqrMagnitude);

        if (rb.linearVelocity.x != 0f)
        {
            spriteRenderer.flipX= rb.linearVelocity.x < 0;
        }

        _currentCD -= Time.deltaTime;

        if(_currentCD < 0) _currentCD = 0;
    }

    // Méthode appelée à chaque frame fixe pour gérer les mouvements physiques
    void FixedUpdate()
    {
        // Si aucun chemin n'a été calculé ou si tous les waypoints ont été atteints, ne fait rien
        if (path == null || currWp >= path.vectorPath.Count || !isAlive)
        {
            return;
        }

        // Calcule la distance entre l'ennemi et le joueur
        float playerDistance = Vector2.Distance(target.transform.position, transform.position);

        // Si le joueur est en dehors de la portée d'attaque = on le poursuit
        if (playerDistance > attackRange)
        {
            // Calcule la direction vers le prochain waypoint
            Vector2 direction = ((Vector2)path.vectorPath[currWp] - rb.position).normalized;

            // Lisser la direction pour éviter des changements brusques (interpolation)
            Vector2 smoothDirection = Vector2.Lerp(rb.linearVelocity.normalized, direction, 0.1f);

            // Calcule la vitesse en fonction de la direction et de la vitesse spécifiée
            Vector2 velocity = smoothDirection * speed * Time.fixedDeltaTime;

            // Applique la vitesse calculée au Rigidbody2D
            rb.linearVelocity = velocity;

            // Calcule la distance entre l'ennemi et le waypoint actuel
            float distance = Vector2.Distance(rb.position, path.vectorPath[currWp]);

            // Si l'ennemi est suffisamment proche du waypoint, passe au suivant
            if (distance < nextWpDistance)
            {
                currWp++;
            }            
        }
        else // => si le joueur est à portée
        {
            // et si le CD est down => Attaque
            if (_currentCD <= 0) Attack();
        }
    }

    private void Attack()
    {
        // Active le trigger d'animation
        animator.SetTrigger("Attack");

        // Reset le CD
        _currentCD = attackCD;        
    }

    // TODO : Problème 1 - il n'y a pas de detection de direction valide comme sur le hero => tape à 360°
    // TODO : Problème 2 - reçevoir un coup reset bien le cd et applique le knockback mais, si l'enemy avait démarrer son coup, bien que l'animation soit intérrompue, les dégâts seront appliqués qd même si on est tjs en range après le timing de l'animation fantôme
    void EndOfAttack()
    {
        if(Vector2.Distance(transform.position, target.position) <= attackRange)
        {
            // Retire des PV au player
            target.GetComponent<PlayerHealth>().TakeDamage(enemyDmg);
            Debug.Log("Le Hero subit une attaque !");
            
        }
    }

    public void TakeDamage(int dmg)
    {
        if (!isAlive) return;

        _health -= dmg;

        if (_health <= 0)
        {
            animator.SetTrigger("Die");
            isAlive = false;
            Instantiate(coinPrefab, transform.position, transform.rotation);
            Destroy(gameObject, 3f);
        } else
        {
            animator.SetTrigger("Hit");
            _currentCD = attackCD;
        }
    }

    // Bonus juste pour visualiser la portée d'attaque
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

}