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
    public string UserHexColour { get; private set; }
    public string Team { get; private set; }
    public bool isDead { get; private set; }

    private void OnDestroy()
    {
        list.Remove(Id);
    }

    public static void Spawn(ushort id, string username, string colour)
    {
        Debug.Log("id = " + id);
        Debug.Log("username = " + username);
        Debug.Log("colour = " + colour);
        foreach (Player otherPlayer in list.Values)
            otherPlayer.SendSpawned(id);


        Player player = Instantiate(GameLogic.Singleton.PlayerPrefab, new Vector3(0f, 1f, 0f), Quaternion.identity).GetComponent<Player>();
        player.name = $"Player {id} ({(string.IsNullOrEmpty(username) ? $"Guest {id}" : $"{username} - {colour}")})";
        player.Id = id;
        player.Username = string.IsNullOrEmpty(username) ? $"Guest {id}" : username;
        player.UserColour = colour;

        player.SendSpawned();
        list.Add(id, player);
    }

    public static void assignColour(ushort id, string colour)
    {
        string hexValue = string.Empty;

        if (DefineColours.ColourHexValues.ContainsKey(colour.ToLower()))
        {
            hexValue = DefineColours.ColourHexValues[colour.ToLower()];
            if (list.ContainsKey(id))
            {
                Player player = list[id];
                player.SendHexColour();
                player.UserHexColour = hexValue;
            }
            else
            {
                Debug.LogError("Player with ID " + id + " does not exist.");
            }
        }
        else
        {
            Debug.LogError("Unknown color: " + colour);
        }


    }

    #region Messages
    private void SendSpawned()
    {
        NetworkManager.Singleton.Server.SendToAll(AddSpawnData(Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientID.playerSpawned)));
    }

    private void SendSpawned(ushort toClientId)
    {
        Debug.Log("SendSpawned");
        NetworkManager.Singleton.Server.Send(AddSpawnData(Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientID.playerSpawned)), toClientId);
    }

    private void SendHexColour()
    {
        Debug.Log("SendHexColour");
        NetworkManager.Singleton.Server.SendToAll(AssignColourData(Message.Create(MessageSendMode.Reliable, (ushort)ServerToClientID.assignedColours)));
    }
    private Message AddSpawnData(Message message)
    {
        message.AddUShort(Id);
        message.AddString(Username);
        message.AddString(UserColour);
        message.AddString(Team);
        Debug.Log(Id + "sent spawn data message");
        //Debug.Log(""Username);
        //Debug.Log(UserColour);
        return message;
    }

    private Message AssignColourData(Message message)
    {
        message.AddUShort(Id);
        message.AddString(UserHexColour);
        return message;
    }


    [MessageHandler((ushort)ClientToServerID.name)]
    private static void Name(ushort fromClientId, Message message)
    {
        Spawn(fromClientId, message.GetString(), message.GetString());
    }

    #endregion
}