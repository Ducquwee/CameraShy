using Riptide;
using Riptide.Utils;
using UnityEngine;


public enum ServerToClientID : ushort
{
    playerSpawned = 1,
}
public enum ClientToServerID : ushort
{
    name = 1,
    UserColour,
}
public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _singleton;

    public static NetworkManager Singleton
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
                Debug.Log($"{nameof(NetworkManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    public Server Server { get; private set; }
    [SerializeField] private ushort port;
    [SerializeField] private ushort maxClientCount;

    private void Start()
    {
        Application.targetFrameRate = 60;

        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        Server = new Server();
        Server.Start(port, maxClientCount);
        Server.ClientDisconnected += PlayerLeft;
    }

    private void Awake()
    {
        Singleton = this;
    }

    private void FixedUpdate()
    {
        Server.Update();
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    private void PlayerLeft(object sender, ServerDisconnectedEventArgs e)
    {
        Destroy(Player.list[e.Client.Id].gameObject);
    }
}

