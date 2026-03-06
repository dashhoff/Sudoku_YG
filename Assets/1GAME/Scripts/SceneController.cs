///////////////////////////////////////////////
//    Author: https://github.com/dashhoff
////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void LoadLevel(int id)
    {
        //ScreenFader.Instance.FadeIn(() => { SceneManager.LoadScene(id); });
        
        YG2.InterstitialAdvShow();
        
        SceneManager.LoadScene(id);
    }

    public void LoadLevel(string name)
    {
        //ScreenFader.Instance.FadeIn(() => { SceneManager.LoadScene(name); });
        
        YG2.InterstitialAdvShow();
        
        SceneManager.LoadScene(name);
    }

    public void RestartLevel()
    {
        //ScreenFader.Instance.FadeIn(() => { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); });
        
        YG2.InterstitialAdvShow();
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
