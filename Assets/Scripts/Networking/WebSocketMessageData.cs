#region Messages
[System.Serializable]
public class ConnectionMsg
{
  public string action;
  public PlayerData[] data;
}

[System.Serializable]
public class TransformMsg
{
  public string action = "movement";
  public TransformData data;

  public void Initialize()
  {
    data = new TransformData();
    data.position = new Vector();
    data.rotation = new Vector();
  }
}
#endregion

#region Message Data
[System.Serializable]
public class PlayerData
{
  public string clientId;
  public string username;
  public int spawnIndex;
  public string color;
  public TransformData transform;
}

[System.Serializable]
public class TransformData
{
  public Vector position;
  public Vector rotation;
}
#endregion

#region Custom classes
[System.Serializable]
public class Vector
{
  public float x;
  public float y;
  public float z;

  public void SetValue(UnityEngine.Vector3 _vector)
  {
    x = _vector.x; y = _vector.y; z = _vector.z;
  }

  public UnityEngine.Vector3 GetVector3Value()
  {
    return new UnityEngine.Vector3(x, y, z);
  }
}
#endregion