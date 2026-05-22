using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]private int _currentHealth;
    public int maxHealth = 3;
    public bool isAlive = true;

    public Transform healthBarUI;
    public GameObject hpPrefab;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        _currentHealth = maxHealth;
        UpdateHealthBarUI();
    }
    public void TakeDamage(int dmg)
    {
        if(isAlive)
        {
            _currentHealth -= dmg;
            UpdateHealthBarUI();

            if (_currentHealth <= 0)
            {
                animator.SetTrigger("Die");
                isAlive = false;
            } 
        }
    }

    public void UpdateHealthBarUI()
    {
        foreach (Transform t in healthBarUI)
        {
            Destroy(t.gameObject);
        }

        for (int i = 0; i < _currentHealth; i++)
        {
            Instantiate(hpPrefab, healthBarUI);
        }
    }

    public void DisablePlayerVisual()
    {
        spriteRenderer.enabled = false;
    }
}
