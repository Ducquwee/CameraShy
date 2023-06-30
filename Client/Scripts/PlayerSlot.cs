using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSlot : MonoBehaviour
{
    //When the player clicks on the slot, it gives the slot a unique Player ID and will replace the placeholder colour with the players colour
    public Image playerSlot;
    public Player player;
    public string playerHexColour;
    public Color color;
    public void ReplacePlayerSlotColour()
    {
        if (playerSlot.GetComponent<Image>().color != null)
        {

            ColorUtility.TryParseHtmlString(player.UserHexColour, out color);
            playerSlot.GetComponent<Image>().color = color;
        }
    }
}
