using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuUIManager : MonoBehaviour
{
  [SerializeField] InputField playerName;
  [SerializeField] Button startBtn;

  void Awake()
  {
    playerName.onValueChanged.AddListener((value) =>
    {
      startBtn.interactable = !string.IsNullOrEmpty(value);
    });
  }

  public void LoadScene(string sceneName)
  {
    LocalPlayerData.Username = playerName.text;
    StartCoroutine(LoadSceneAsync(sceneName));
  }

  private IEnumerator LoadSceneAsync(string sceneName)
  {

    AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

    while (async.progress < 0.9f)
    {
      yield return null;
    }

    async.allowSceneActivation = true;
  }

  public void Quit()
  {
    Application.Quit();
  }


}
