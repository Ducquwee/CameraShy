using Riptide;
using Riptide.Utils;
using UnityEngine;


public enum ServerToClientID : ushort
{
    playerSpawned = 1,
    playerEliminated,
    assignedColours,
}
public enum ClientToServerID : ushort
{
    name = 1,
    colourHex,
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
        

        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
        StartServer();

    }

    public void StartServer()
    {
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
        if (Server != null)
        {
            Server.Update();
        }
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

