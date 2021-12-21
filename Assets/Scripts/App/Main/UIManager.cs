using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
  private static UIManager instance;
  [SerializeField] Transform playerViewContainer;
  [SerializeField] PlayerView playerViewPrefab;

  void Awake()
  {
    instance = this;
  }

  public static PlayerView AddPlayerView(string username, Color color)
  {
    PlayerView pv = Instantiate(instance.playerViewPrefab, instance.playerViewContainer);
    pv.Initialize(username, color);
    return pv;
  }
  public static Transform PlayerViewContainer => instance.playerViewContainer;

  public void ReturnToMenu(string menuSceneName)
  {
    WebSocketHandler.CloseConnection();
    SceneManager.LoadScene(menuSceneName);
  }

  public void Quit()
  {
    Application.Quit();
  }

  void OnDestroy()
  {
    instance = null;
  }
}
