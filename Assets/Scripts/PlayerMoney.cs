using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoney : MonoBehaviour
{
    public Text coinText;
    public int currentCoins;

    public static PlayerMoney instance;

    private void Awake()
    {
        if (instance is null)
        {
            instance = this; 
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(PlayerPrefs.HasKey("Money"))
        {
            currentCoins = PlayerPrefs.GetInt("Money");
        }

        coinText = GameObject.FindGameObjectWithTag("CoinsText").GetComponent<Text>();
        UpdateCoinsCount();
    }

    private void UpdateCoinsCount()
    {
        coinText.text = currentCoins.ToString();
    }

    public void AddCoin()
    {
        currentCoins++;
        UpdateCoinsCount();
    }

}
