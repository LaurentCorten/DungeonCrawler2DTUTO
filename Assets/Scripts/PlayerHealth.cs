using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth = 3;
    public bool isAlive = true;

    public Transform healthBarUI;
    public GameObject hpPrefab;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        currentHealth = maxHealth;
        UpdateHealthBarUI();
    }
    public void TakeDamage(int dmg)
    {
        if(isAlive)
        {
            currentHealth -= dmg;
            UpdateHealthBarUI();

            if (currentHealth <= 0)
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

        for (int i = 0; i < currentHealth; i++)
        {
            Instantiate(hpPrefab, healthBarUI);
        }
    }

    public void DisablePlayerVisual()
    {
        spriteRenderer.enabled = false;
    }

    public void IncreaseMaxHealth(int amt)
    {
        maxHealth += amt;
        currentHealth += amt;
        UpdateHealthBarUI();
    }
     
}
