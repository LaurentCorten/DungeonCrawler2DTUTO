using UnityEngine;
using UnityEngine.UI;

public class PlayerMoney : MonoBehaviour
{
    public Text coinText;
    public int currentCoins;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        coinText = GameObject.FindGameObjectWithTag("CoinsText").GetComponent<Text>();
        UpdateCoinsCount();
    }

    private void UpdateCoinsCount()
    {
        string coins = currentCoins.ToString();
        coinText.text = coins;
    }

    public void AddCoin(int amt = 1)
    {
        currentCoins += amt;
        UpdateCoinsCount();
    }

}
