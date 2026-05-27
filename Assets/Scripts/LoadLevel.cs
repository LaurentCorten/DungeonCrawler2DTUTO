using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{

    public string levelToLoad;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            StartCoroutine(LoadSceneWithFade(levelToLoad));
            PlayerPrefs.SetInt("Money", PlayerMoney.instance.currentCoins);
            PlayerPrefs.Save();
        }
    }

    IEnumerator LoadSceneWithFade(string sceneName)
    {
        yield return FadeManager.instance.FadeOut();
        yield return SceneManager.LoadSceneAsync(sceneName);
    }
}
