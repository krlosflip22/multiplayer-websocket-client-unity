using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
  private readonly ConcurrentQueue<Action> _actions = new ConcurrentQueue<Action>();

  [Header("Prefabs")]
  [SerializeField] Transform localPlayerPrefab;
  [SerializeField] Transform remotePlayerPrefab;
  [Header("Spawn")]
  [SerializeField] Transform[] spawnPositions;


  LocalPlayer localPlayer;
  Dictionary<string, RemotePlayer> players;


  void Awake()
  {
    players = new Dictionary<string, RemotePlayer>();
  }

  private void Update()
  {
    // Work the dispatched actions on the Unity main thread because the web sockets callbacks are not running in main thread.
    while (_actions.Count > 0)
    {
      if (_actions.TryDequeue(out var action))
      {
        action?.Invoke();
      }
    }
  }


  public void SpawnLocalPlayer(int spawnIndex)
  {
    _actions.Enqueue(() =>
    {
      localPlayer = Instantiate(localPlayerPrefab, spawnPositions[spawnIndex % 4].position, spawnPositions[spawnIndex % 4].rotation).GetComponent<LocalPlayer>();
      Debug.Log("Local player connected");
    });
  }

  public void SpawnRemotePlayer(string clientId, string username, int spawnIndex, string color, Vector position = null, Vector rotation = null)
  {
    _actions.Enqueue(() =>
    {
      if (players.ContainsKey(clientId)) return;

      Vector3 spawnPos = position == null ? spawnPositions[spawnIndex % 4].position : position.GetVector3Value();
      Vector3 spawnRot = rotation == null ? spawnPositions[spawnIndex % 4].rotation.eulerAngles : rotation.GetVector3Value();

      RemotePlayer rp = Instantiate(remotePlayerPrefab, spawnPos, Quaternion.Euler(spawnRot)).GetComponent<RemotePlayer>();
      rp.Initialize(clientId, username, color, spawnPos, spawnRot);

      players.Add(clientId, rp);
      Debug.Log($"Remote player {username}|{clientId} connected");
    });
  }


  public void SendPlayerUpdate(string clientId, Vector position, Vector rotation)
  {
    players[clientId]?.SetTransform(position.GetVector3Value(), rotation.GetVector3Value());
  }


  public void RemovePlayer(string clientId)
  {
    _actions.Enqueue(() =>
    {
      RemotePlayer rp = players[clientId];
      players.Remove(clientId);
      Destroy(rp.gameObject);
    });
  }
}
