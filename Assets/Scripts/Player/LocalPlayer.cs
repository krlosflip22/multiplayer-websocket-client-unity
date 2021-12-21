using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class LocalPlayer : MonoBehaviour
{
  TransformMsg msgToSend;

  [SerializeField] Transform cameraT;
  [SerializeField] float speed = 0.5f;

  public float xMoveThreshold = 1000.0f;
  public float yMoveThreshold = 1000.0f;

  public float yMaxLimit = 45.0f;
  public float yMinLimit = -45.0f;


  float yRotCounter = 0.0f;
  float xRotCounter = 0.0f;

  PlayerView pv;

  private void Awake()
  {
    msgToSend = new TransformMsg();
    msgToSend.Initialize();
  }

  private void Start()
  {
    Camera.main.transform.parent = cameraT;
    Camera.main.transform.localPosition = Vector3.zero;
    Camera.main.transform.localRotation = Quaternion.Euler(Vector3.zero);

    Color color;
    ColorUtility.TryParseHtmlString(LocalPlayerData.ColorCode, out color);
    pv = UIManager.AddPlayerView(LocalPlayerData.Username, color);
  }

  void Update()
  {
    if (InputManager.Horizontal != 0 || InputManager.Vertical != 0 || InputManager.IsDragging)
    {
      transform.position += speed * (InputManager.Horizontal * transform.right + InputManager.Vertical * transform.forward);
      if (InputManager.IsDragging)
      {
        xRotCounter += Input.GetAxis("Mouse X") * xMoveThreshold * Time.deltaTime;
        yRotCounter += Input.GetAxis("Mouse Y") * yMoveThreshold * Time.deltaTime;
        yRotCounter = Mathf.Clamp(yRotCounter, yMinLimit, yMaxLimit);
        cameraT.localEulerAngles = new Vector3(-yRotCounter, 0, 0);
        transform.localEulerAngles = new Vector3(0, xRotCounter, 0);
      }

      msgToSend.data.position.SetValue(transform.position);
      msgToSend.data.rotation.SetValue(new Vector3(cameraT.localEulerAngles.x, transform.localEulerAngles.y, 0));
      WebSocketHandler.SendMsg(JsonConvert.SerializeObject(msgToSend));
    }
  }

  void OnDestroy()
  {
    if (pv) Destroy(pv.gameObject);
  }
}