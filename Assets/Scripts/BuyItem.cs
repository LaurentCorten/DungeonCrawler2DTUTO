using UnityEngine;
using UnityEngine.UI;

public class BuyItem : MonoBehaviour
{
    public int price;
    public float speedMultiplier;
    public int hpModifier;
    public float dmgMultiplier;
    public Text priceText;

    private void Start()
    {
        priceText.text = price.ToString();
        Debug.LogWarning(gameObject.name + " = " +  price + " et playermoney = " + PlayerBase.instance.playerMoney.currentCoins);

        if(price > PlayerBase.instance.playerMoney.currentCoins)
        {
            priceText.color = Color.red;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player") && price <= PlayerBase.instance.playerMoney.currentCoins)
        {
            PlayerBase.instance.playerMoney.AddCoin(-price);

            if (speedMultiplier != 0)
            {
                PlayerBase.instance.playerMouvement.MultiplySpeed(speedMultiplier);
            }
            if (dmgMultiplier != 0)
            {
                PlayerBase.instance.playerMouvement.MultiplyDmg(dmgMultiplier);
            }

            PlayerBase.instance.playerHealth.IncreaseMaxHealth(hpModifier);
        }
    }

}
