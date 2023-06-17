using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;
using Riptide.Utils;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

    public ushort Id { get; private set; }
    public string Username { get; private set; }
    public string Colour { get; private set; }

    private void OnDestroy()
    {
        list.Remove(Id);
    }

    public static void Spawn(ushort id, string username, string colour)
    {
        foreach (Player otherPlayer in list.Values)
            otherPlayer.SendSpawned(id);

        Player player = Instantiate(GameLogic.Singleton.PlayerPrefab, new Vector3(0f, 1f, 0f), Quaternion.identity).GetComponent<Player>();

        player.Id = id;
        string playerColor = string.IsNullOrEmpty(colour) ? "Default" : colour;
        player.name = $"Player {id} ({playerColor} - {(string.IsNullOrEmpty(username) ? "Guest" : username)})";

        player.SendSpawned();
        list.Add(id, player);
    }

    #region Messages
    private void SendSpawned()
    {
        NetworkManager.Singleton.Server.SendToAll(AddSpawnData(Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientID.playerSpawned)));
    }

    private void SendSpawned(ushort toClientId)
    {
        NetworkManager.Singleton.Server.Send(AddSpawnData(Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientID.playerSpawned)), toClientId);
    }
    private Message AddSpawnData(Message message)
    {
        message.AddUShort(Id);
        message.AddString(Username);
        message.AddVector3(transform.position);
        message.AddString(Colour);
        return message;
    }

    [MessageHandler((ushort)ClientToServerID.name)]
    private static void Name(ushort fromClientId, Message message)
    {
        Spawn(fromClientId, message.GetString(), message.GetString());
    }

    #endregion
}
