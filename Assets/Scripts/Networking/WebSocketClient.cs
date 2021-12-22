using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class WebSocketClient : MonoBehaviour
{
  public const string WSURL_KEY = "wsurl";
  private static WebSocketClient instance;
  [SerializeField] PlayerManager playerManager;

  WebSocket ws;

  #region MonoBehaviour
  void Awake()
  {
    instance = this;
  }

  private void Start()
  {
    ws = new WebSocket(PlayerPrefs.GetString(WSURL_KEY));
    ws.OnOpen += OnWSOpen;
    ws.OnMessage += OnWSMessageReceived;
    ws.Connect();
  }

  void OnApplicationQuit()
  {
    ws.Close();
  }

  void OnDestroy()
  {
    instance = null;
  }
  #endregion

  #region Static Functions
  public static void SendMsg(string message)
  {
    instance.ws.Send(message);
  }

  public static void CloseConnection()
  {
    instance.ws.Close();
  }
  #endregion

  #region WebSocket Callbacks
  void OnWSOpen(object sender, System.EventArgs e)
  {
    if (string.IsNullOrEmpty(LocalPlayerData.Username)) LocalPlayerData.Username = "unityClient";
    ws.Send("{ \"action\" : \"connected\", \"data\" : { \"username\" : \"" + LocalPlayerData.Username + "\" } }");
  }

  void OnWSMessageReceived(object sender, MessageEventArgs e)
  {
    var received = JObject.Parse(e.Data);
    switch ((string)received[WSJsonKeys.ACTION])
    {
      case WSJsonActions.LOCAL_CONNECTION:
        ConnectionMsg connectionMsg = JsonUtility.FromJson<ConnectionMsg>(e.Data);
        foreach (PlayerData pd in connectionMsg.data)
        {
          playerManager.SpawnRemotePlayer(pd.clientId, pd.username, pd.spawnIndex, pd.color, pd.transform.position, pd.transform.rotation);
        }
        break;
      case WSJsonActions.CONNECTED:
        if (((bool)received[WSJsonKeys.LOCALCLIENT]))
        {
          //Setting Local Player username and color
          LocalPlayerData.ClientId = (string)received[WSJsonKeys.CLIENTID];
          LocalPlayerData.ColorCode = (string)received[WSJsonKeys.DATA][WSJsonKeys.COLOR];
          playerManager.SpawnLocalPlayer((int)received[WSJsonKeys.DATA][WSJsonKeys.SPAWN_INDEX]);
        }
        else
        {
          playerManager.SpawnRemotePlayer(
            (string)received[WSJsonKeys.CLIENTID],
            (string)received[WSJsonKeys.DATA][WSJsonKeys.USERNAME],
            (int)received[WSJsonKeys.DATA][WSJsonKeys.SPAWN_INDEX],
            (string)received[WSJsonKeys.DATA][WSJsonKeys.COLOR]);
        }
        break;
      case WSJsonActions.MOVEMENT:
        if (((bool)received[WSJsonKeys.LOCALCLIENT])) return;

        TransformData msgReceived = JsonUtility.FromJson<TransformData>(JsonConvert.SerializeObject(received[WSJsonKeys.DATA]));
        playerManager.SendPlayerUpdate((string)received[WSJsonKeys.CLIENTID], msgReceived.position, msgReceived.rotation);
        break;
      case WSJsonActions.USER_LEAVE:
        playerManager.RemovePlayer((string)received[WSJsonKeys.CLIENTID]);
        break;
    }
  }
  #endregion
}