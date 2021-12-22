using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuUIManager : MonoBehaviour
{
  [SerializeField] InputField playerName;
  [SerializeField] InputField serverUrlSettings;
  [SerializeField] Button startBtn;
  [SerializeField] Button setServerURLBtn;

  void Awake()
  {
    string wsUrl = PlayerPrefs.GetString(WebSocketClient.WSURL_KEY);
    if (!string.IsNullOrEmpty(wsUrl))
    {
      serverUrlSettings.text = wsUrl;
    }
    else
    {
      PlayerPrefs.SetString(WebSocketClient.WSURL_KEY, serverUrlSettings.text);
    }

    playerName.onValueChanged.AddListener((value) =>
      {
        startBtn.interactable = !string.IsNullOrEmpty(value);
      });
    serverUrlSettings.onValueChanged.AddListener((value) =>
    {
      setServerURLBtn.interactable = !string.IsNullOrEmpty(value);
    });
  }

  public void SetWSUrl()
  {
    PlayerPrefs.SetString(WebSocketClient.WSURL_KEY, serverUrlSettings.text);
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
