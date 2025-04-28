using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonTravel : MonoBehaviour
{
    public AudioSource aud;
    public string sceneName;
    public Animation animation_;
    public void GoToScene()
    {
        SceneManager.LoadScene(sceneName);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void ActivateGameObject(GameObject objectToActivate)
    {
        objectToActivate.SetActive(true);
    }
    public void DeactivateGameObject(GameObject objectToActivate)
    {
        objectToActivate.SetActive(false);
    }
    public void GoToSceneAfterAnim()
    {
        StartCoroutine(AnimObjectEnabled());
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public IEnumerator AnimObjectEnabled()
    {
        animation_.gameObject.SetActive(true);
        animation_.Play();
        yield return new WaitUntil(() => !animation_.isPlaying);
        SceneManager.LoadScene(sceneName);
    }
}
