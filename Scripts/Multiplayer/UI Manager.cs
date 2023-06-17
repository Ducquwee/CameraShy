using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Riptide;
using Riptide.Utils;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager _singleton;

    public static UIManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
            {
                _singleton = value;
            }
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(UIManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    [Header("Connect")]
    [SerializeField] private GameObject connectUI;
    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private TMP_Dropdown colourDropdown;


    private void Awake()
    {
        Singleton = this;
    }

    public void ConnectClicked()
    {
        usernameField.interactable = false;
        colourDropdown.interactable = false;
        connectUI.SetActive(false);
        NetworkManager.Singleton.Connect();
    }

    public void BackToMain()
    {
        usernameField.interactable = true;
        colourDropdown.interactable = true;
        connectUI.SetActive(true);
    }

    public void SendName()
    {
        Debug.Log(usernameField.text);
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerID.name);
        message.AddString(usernameField.text);
        NetworkManager.Singleton.Client.Send(message);
    }

    public void SendColour()
    {
        Debug.Log(colourDropdown.options[colourDropdown.value].text);
        Message message = Message.Create(MessageSendMode.Reliable, (ushort)ClientToServerID.colour);
        message.AddString(colourDropdown.options[colourDropdown.value].text);
        NetworkManager.Singleton.Client.Send(message);
    }
}
