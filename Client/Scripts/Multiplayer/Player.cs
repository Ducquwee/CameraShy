using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;

public class Player : MonoBehaviour
{
    // Dictionary to store the list of players using their IDs as keys
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

    // Properties
    public ushort Id { get; private set; } // ID of the player
    public bool IsLocal { get; private set; } // Flag indicating if the player is the local player
    private string Username; // Username of the player
    private string UserColour;


    // Called when the player object is destroyed

    private void OnDestroy()
    {
        list.Remove(Id); // Remove the player from the list when destroyed
    }

    // Spawns a player object with the given ID, username, and position
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

    }





    // Message handler for spawning a player received from the server
    [MessageHandler((ushort)ServerToClientID.playerSpawned)]
    private static void SpawnPlayer(Message message)
    {
        // Extract the ID, username, and position from the message and call the Spawn method
        Spawn(message.GetUShort(), message.GetString(), null);
    }

    [MessageHandler((ushort)ClientToServerID.UserColour)]
    private static void Colour(ushort fromClientId, Message message)
    {
        string colour = message.GetString();
        Player player = GetPlayer(fromClientId);
        if (player != null)
        {
            player.UserColour = colour;
        }
        else
        {
            Spawn(fromClientId, null, colour);
        }
    }

    private static Player GetPlayer(ushort id)
    {
        if (list.ContainsKey(id))
        {
            return list[id];
        }
        return null;
    }
}
