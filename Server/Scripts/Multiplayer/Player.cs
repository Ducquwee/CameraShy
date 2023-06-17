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
    public string UserColour { get; private set; }

    private void OnDestroy()
    {
        list.Remove(Id);
    }

    public static void Spawn(ushort id, string username, string userColour)
    {
        Player player;
        if (list.TryGetValue(id, out player))
        {
            // Update the existing player with new username and color
            player.Username = username;
            player.UserColour = string.IsNullOrEmpty(userColour) ? player.UserColour : userColour;
        }
        else
        {
            // Create a new player object
            player = Instantiate(GameLogic.Singleton.PlayerPrefab, new Vector3(0f, 1f, 0f), Quaternion.identity).GetComponent<Player>();
            player.Id = id;
            list.Add(id, player);
        }

        // Set the name of the player object
        string playerColor = string.IsNullOrEmpty(player.UserColour) ? "Default" : player.UserColour;
        string playerName = string.IsNullOrEmpty(player.Username) ? "Guest" : player.Username;
        player.name = $"Player {id} ({playerColor} - {playerName})";

        // Send the updated spawn message to all clients
        player.SendSpawned();
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
        message.AddString(UserColour);

        return message;
    }


    [MessageHandler((ushort)ClientToServerID.name)]
    private static void Name(ushort fromClientId, Message message)
    {
        Spawn(fromClientId, message.GetString(), null);
    }

    [MessageHandler((ushort)ClientToServerID.UserColour)]
    private static void Colour(ushort fromClientId, Message message)
    {
        string colour = message.GetString();
        Player player = GetPlayer(fromClientId);

        Spawn(fromClientId, null, colour);

    }
    private static Player GetPlayer(ushort id)
    {
        if (list.ContainsKey(id))
        {
            return list[id];
        }
        return null;
    }

    #endregion
}