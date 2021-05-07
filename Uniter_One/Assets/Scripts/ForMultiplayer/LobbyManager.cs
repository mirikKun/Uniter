using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    [SerializeField] private Text logText;
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject roomCreatorMenu;
    [SerializeField] private Slider size;
    [SerializeField] private Slider enemyCount;
    [SerializeField] private Toggle outerLightOn;
    [SerializeField] private Toggle wallsOn;
    [SerializeField] private Toggle randomRoomSetup;

    private byte _playerCount = 4;

    private void Start()
    {
        PhotonNetwork.NickName = "Player" + Random.Range(1000, 9999);
        Log("Player`s name is set to" + PhotonNetwork.NickName);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Log("ConnectedToMaster");
    }


    public void RoomCreatorMenu()
    {
        startMenu.SetActive(false);
        roomCreatorMenu.SetActive(true);
        size.value = FillOptions.size;
        enemyCount.value = FillOptions.enemyCount;
        outerLightOn.isOn = FillOptions.outerLightEnable;
        wallsOn.isOn = FillOptions.useWalls;
        randomRoomSetup.isOn = FillOptions.randomRoomGeneration;
    }

    public void StartMenu()
    {
        startMenu.SetActive(true);
        roomCreatorMenu.SetActive(false);
    }

    public void CreateRoom(bool defaultRoom)
    {
        Log("Creating");
        FillOptions.defaultRoom = defaultRoom;
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions() {MaxPlayers = _playerCount});
    }

    public void JoinRoom()
    {
        Log("Joining");
        FillOptions.join = true;
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Arena");
    }

    private void Log(string message)
    {
        logText.text += '\n';
        logText.text += message;
    }

    public void SetSize(float roomSize)
    {
        FillOptions.size = (int) roomSize;
    }

    public void SetEnemyCount(float count)
    {
        FillOptions.enemyCount = (int) count;
    }

    public void SetUseWall(bool useWalls)
    {
        FillOptions.useWalls = useWalls;
    }

    public void SetOuterLight(bool outerLight)
    {
        FillOptions.outerLightEnable = outerLight;
    }

    public void SetUseRandom(bool useRandom)
    {
        FillOptions.randomRoomGeneration = useRandom;
    }
}