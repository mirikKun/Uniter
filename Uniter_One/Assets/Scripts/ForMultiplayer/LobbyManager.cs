
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public Text logText;
    public GameObject startMenu;
    public GameObject roomCreatorMenu;
    public Slider size;
    public Slider enemyCount;
    public Toggle outerLightOn;
    public Toggle wallsOn;
    public Toggle randomRoomSetup;
    void Start()
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
        
      size.value=FillOptions.size;
      enemyCount.value=FillOptions.enemyCount;
      outerLightOn.isOn=FillOptions.outerLightEnable;
      wallsOn.isOn=FillOptions.useWalls;
      randomRoomSetup.isOn=FillOptions.randomRoomGeneration;
    }

    public void StartMenu()
    {
        startMenu.SetActive(true);
        roomCreatorMenu.SetActive(false);
    }

    public void CreateRoom()
    {
        FillOptions.defaultRoom = false;
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions() {MaxPlayers = 2});
    }
    public void CreateDefaultRoom()
    {
        FillOptions.defaultRoom = true;
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions() {MaxPlayers = 2});
    }

    public void JionRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Arena");
    }

    private void Log(string message)
    {
        Debug.Log(message);
        logText.text += '\n';
        logText.text += message;
    }

    public void SetSize(float roomSize)
    {
        FillOptions.size = (int)roomSize;
    }
    public void SetEnemyCount(float count)
    {
        FillOptions.enemyCount = (int)count;
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
