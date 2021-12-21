using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RemotePlayer : MonoBehaviour
{
  [SerializeField] TextMeshProUGUI playerNameText;
  [SerializeField] Image bgColor;
  [SerializeField] MeshRenderer bodyM;
  [SerializeField] MeshRenderer headM;
  [SerializeField] Transform headT;
  [SerializeField] Transform billboard;

  float lerpRate = 5;

  public float speed = 1.0f;

  Vector3 syncTransformPosition;
  Quaternion syncBodyRotation;
  Quaternion syncHeadRotation;

  PlayerView pv;

  public void Initialize(string clientId, string username, string colorStr, Vector3 position, Vector3 rotation)
  {
    name = clientId;
    playerNameText.text = username;
    Color newColor;
    ColorUtility.TryParseHtmlString(colorStr, out newColor);
    bgColor.color = newColor;
    bodyM.material.color = newColor;
    headM.material.color = newColor;
    SetTransform(position, rotation);

    pv = UIManager.AddPlayerView(username, newColor);
  }

  public void SetTransform(Vector3 position, Vector3 rotation)
  {
    syncTransformPosition = position;
    syncBodyRotation = Quaternion.Euler(0, rotation.y, 0);
    syncHeadRotation = Quaternion.Euler(rotation.x, 0, 0);
  }

  private void FixedUpdate()
  {
    transform.position = Vector3.Lerp(transform.position, syncTransformPosition, Time.deltaTime * lerpRate);
    transform.rotation = Quaternion.Lerp(transform.rotation, syncBodyRotation, Time.deltaTime * lerpRate);
    headT.localRotation = Quaternion.Lerp(headT.localRotation, syncHeadRotation, Time.deltaTime * lerpRate);
  }
  private void LateUpdate()
  {
    billboard.rotation = Quaternion.Euler(0f, Camera.main.transform.eulerAngles.y, 0f); //canvas.worldCamera.transform.rotation;
  }

  private void OnDestroy()
  {
    if (pv) Destroy(pv.gameObject);

    System.GC.Collect();
  }
}