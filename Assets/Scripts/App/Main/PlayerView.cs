using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
  [SerializeField] Text playerUsername;
  [SerializeField] Image playerColor;

  public void Initialize(string username, Color color)
  {
    playerUsername.text = username;
    playerColor.color = color;
    gameObject.SetActive(true);
  }
}
