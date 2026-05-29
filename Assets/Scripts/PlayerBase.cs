using System;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public PlayerMouvement playerMouvement;
    public PlayerMoney playerMoney;

    public static PlayerBase instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        LoadData();
    }

    private void OnApplicationQuit()
    {
        ClearData();
    }

    public void ClearData()
    {
        PlayerPrefs.DeleteAll();
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("Money", PlayerBase.instance.playerMoney.currentCoins);
        PlayerPrefs.SetInt("Attack", PlayerBase.instance.playerMouvement.damage);
        PlayerPrefs.SetFloat("Speed", PlayerBase.instance.playerMouvement.moveSpeed);
        PlayerPrefs.SetInt("MaxHp", PlayerBase.instance.playerHealth.maxHealth);
        PlayerPrefs.SetInt("CurrentHp", PlayerBase.instance.playerHealth.currentHealth);
        PlayerPrefs.Save();
    }

    private void LoadData()
    {
        if(PlayerPrefs.HasKey("Money"))
        {
            playerMoney.currentCoins = PlayerPrefs.GetInt("Money");
        }
        if(PlayerPrefs.HasKey("Attack"))
        {
            playerMouvement.damage = PlayerPrefs.GetInt("Attack");
        }
        if(PlayerPrefs.HasKey("Speed"))
        {
            playerMouvement.moveSpeed = PlayerPrefs.GetFloat("Speed");
        }
        if(PlayerPrefs.HasKey("MaxHp"))
        {
            playerHealth.maxHealth = PlayerPrefs.GetInt("MaxHp");
        }
        if(PlayerPrefs.HasKey("CurrentHp"))
        {
            playerHealth.currentHealth = PlayerPrefs.GetInt("CurrentHp");
        }

        playerHealth.UpdateHealthBarUI();
    }
}
