
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public Text LogText;
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

    public void CreateRoom()
    {
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
        LogText.text += '\n';
        LogText.text += message;
    }
}
