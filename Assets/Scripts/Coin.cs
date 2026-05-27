using UnityEngine;

public class Coin : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerMoney.instance.AddCoin();
            Destroy(gameObject);
        }
    }
}
