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
    private string username; // Username of the player
    private string colour;

    // Called when the player object is destroyed
    private void OnDestroy()
    {
        list.Remove(Id); // Remove the player from the list when destroyed
    }

    // Spawns a player object with the given ID, username, and position
    public static void Spawn(ushort id, string username, Vector3 position, string colour)
    {
        Player player;
        if (id == NetworkManager.Singleton.Client.Id)
        {
            // If the ID matches the local client's ID, instantiate the local player prefab
            player = Instantiate(GameLogic.Singleton.LocalPlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = true; // Set IsLocal flag to true for the local player
        }
        else
        {
            // If the ID doesn't match the local client's ID, instantiate the regular player prefab
            player = Instantiate(GameLogic.Singleton.PlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = false; // Set IsLocal flag to false for other players
        }

        string playerColour = string.IsNullOrEmpty(colour) ? "Default" : colour; // Set the default color if none is provided

        player.name = $"Player {id} ({playerColour} - {(string.IsNullOrEmpty(username) ? "Guest" : username)})"; // Set the name of the player object

        player.Id = id; // Set the ID of the player

        player.username = string.IsNullOrEmpty(username) ? $"Guest {id}" : username; // Set the username of the player
    }


    // Message handler for spawning a player received from the server
    [MessageHandler((ushort)ServerToClientID.playerSpawned)]
    private static void SpawnPlayer(Message message)
    {
        // Extract the ID, username, and position from the message and call the Spawn method
        Spawn(message.GetUShort(), message.GetString(), message.GetVector3(), message.GetString());
    }
}
