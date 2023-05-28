using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.ProBuilder.Shapes;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private static LobbyManager instance = null;
    public static LobbyManager Instance
    {
        get
        {
            if (instance == null) return null;

            return instance;
        }
    }

    private readonly string gameVersion = "1.0";
    private string userId = "None";

    [Header("For Join Server")]
    public TMP_InputField inputUserId; // 유저 이름 입력 후 엔터키 or joinButton 클릭으로 room에 접속 가능하도록 구현할 것
    public TMP_Text connectionInfoText;
    public Button joinButton;

    [Header("Player & Camera & StartCanvas")]
    public GameObject spawnPos;
    public GameObject startCanvas;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        PhotonNetwork.AutomaticallySyncScene = false; // 각 클라이언트가 개별적으로 씬을 이동해야 하기 때문에 씬 동기화 false
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.NickName = userId;

        // 캔버스 초기
        startCanvas.SetActive(true);

        PhotonNetwork.ConnectUsingSettings(); // 마스터 서버 접속

        joinButton.interactable = false; // 접속 버튼 비활성화 (마스터 서버 접속에 성공하면 다시 활성화)
        connectionInfoText.text = "Connecting To Master Server...";
        
        Debug.Log("Connecting To Master Server...");
    }

    public override void OnConnectedToMaster() // 마스터 서버에 접속 성공 했을 시 실행되는 함수
    {
        joinButton.interactable = true;
        connectionInfoText.text = "Online : Connected to Master Server";
        
        Debug.Log("Online : Connected to Master Server");
    }

    public override void OnDisconnected(DisconnectCause cause) // 마스터 서버 접속에 시도 했으나 실패한 경우 or 이미 접속된 상태에서 연결이 끊어진 경우에 실행 됨
    {
        joinButton.interactable = false;
        connectionInfoText.text = $"Offline : Connection Disabled {cause.ToString()} - Try reconnecting...";

        Debug.Log($"Offline : Connection Disabled {cause.ToString()} - Try reconnecting...");

        PhotonNetwork.ConnectUsingSettings(); // 재접속 시도
    }

    public void Connect() // joinButton or 엔터키를 입력했을 때 실행되는 함수
    {
        if (string.IsNullOrEmpty(inputUserId.text)) return; // 이름을 입력하지 않았을 경우 함수 종료
        if (PhotonNetwork.InRoom) return; // 플레이어가 이미 Room에 입장한 경우 종료


        joinButton.interactable = false; // 연속 실행을 방지하기 위해 버튼 비활성화

        if (PhotonNetwork.IsConnected) // 마스터 서버가 연결 되어 있다면
        {
            connectionInfoText.text = "Connecting to Random Room...";

            Debug.Log("Connecting to Random Room...");

            PhotonNetwork.NickName = inputUserId.text;
            Debug.Log(inputUserId.text);
            PhotonNetwork.JoinRandomRoom(); // 랜덤한 방으로 접속을 시도
        }
        else
        {
            connectionInfoText.text = "Offline : Connection Disabled - Try reconnecting...";

            Debug.Log("Offline : Connection Disabled - Try reconnecting...");
            PhotonNetwork.ConnectUsingSettings(); // 재접속 시도
        }
    }

    public void OnInputEndEdit(string value)
    {
        if (string.IsNullOrEmpty(value)) return; // 이름이 입력되지 않았을 경우 함수 종료

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Connect();
        }
    }

    public override void OnJoinRandomFailed(short returCode, string message)
    {
        connectionInfoText.text = "There is no empty room, Creating new Room.";

        Debug.Log("There is no empty room, Creating new Room.");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 20 }); // 방을 생성하는 함수, 매개변수: (방 이름, 방 옵션)
    }

    public override void OnJoinedRoom() // 방을 생성해서 들어왔거나 기존 방에 들어오면 실행되는 함수
    {
        connectionInfoText.text = "Connected with Room.";
        Debug.Log("Connected with Room.");
        // SceneManager.LoadScene(); // LoadScene()으로 씬 이동 시 자신만 넘어가고 나머지 사람들은 넘어가지 않으므로 씬 변경 시 해당 함수를 사용해선 안됌
        // PhotonNetwork.LoadLevel("Kumoh_Main"); // Photon에선 씬 이동 시 PhotonNetwork.LoadLevel()를 사용해서 이동해야 함
        // 기본적으로 Photon PUN은 방에 입장한 클라이언트들이 동일한 씬을 공유하도록 설계 되어있어 OnJoinedRoom 함수에서 씬을 불러오지 않아도 기본적으로 현재 씬이 유지됩니다.

        Camera.main.enabled = false;
        GameObject player = PhotonNetwork.Instantiate("Player", spawnPos.transform.position, spawnPos.transform.rotation);


        // 플레이어 기능 On
        startCanvas.SetActive(false);

        // 마우스 커서 비활성화
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("New player entered the room: " + newPlayer.NickName);
    }

}
