using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerBase.instance.playerMoney.AddCoin();
            StartCoroutine(DestroyCoin());
        }
    }

    public IEnumerator DestroyCoin()
    {
        yield return new WaitForSeconds(.02f);
        Destroy(gameObject);
    }
}
