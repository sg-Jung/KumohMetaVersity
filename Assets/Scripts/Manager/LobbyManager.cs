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
    public TMP_InputField inputUserId; // ���� �̸� �Է� �� ����Ű or joinButton Ŭ������ room�� ���� �����ϵ��� ������ ��
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

        PhotonNetwork.AutomaticallySyncScene = false; // �� Ŭ���̾�Ʈ�� ���������� ���� �̵��ؾ� �ϱ� ������ �� ����ȭ false
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.NickName = userId;

        // ĵ���� �ʱ�
        startCanvas.SetActive(true);

        PhotonNetwork.ConnectUsingSettings(); // ������ ���� ����

        joinButton.interactable = false; // ���� ��ư ��Ȱ��ȭ (������ ���� ���ӿ� �����ϸ� �ٽ� Ȱ��ȭ)
        connectionInfoText.text = "Connecting To Master Server...";
        
        Debug.Log("Connecting To Master Server...");
    }

    public override void OnConnectedToMaster() // ������ ������ ���� ���� ���� �� ����Ǵ� �Լ�
    {
        joinButton.interactable = true;
        connectionInfoText.text = "Online : Connected to Master Server";
        
        Debug.Log("Online : Connected to Master Server");
    }

    public override void OnDisconnected(DisconnectCause cause) // ������ ���� ���ӿ� �õ� ������ ������ ��� or �̹� ���ӵ� ���¿��� ������ ������ ��쿡 ���� ��
    {
        joinButton.interactable = false;
        connectionInfoText.text = $"Offline : Connection Disabled {cause.ToString()} - Try reconnecting...";

        Debug.Log($"Offline : Connection Disabled {cause.ToString()} - Try reconnecting...");

        PhotonNetwork.ConnectUsingSettings(); // ������ �õ�
    }

    public void Connect() // joinButton or ����Ű�� �Է����� �� ����Ǵ� �Լ�
    {
        if (string.IsNullOrEmpty(inputUserId.text)) return; // �̸��� �Է����� �ʾ��� ��� �Լ� ����
        if (PhotonNetwork.InRoom) return; // �÷��̾ �̹� Room�� ������ ��� ����


        joinButton.interactable = false; // ���� ������ �����ϱ� ���� ��ư ��Ȱ��ȭ

        if (PhotonNetwork.IsConnected) // ������ ������ ���� �Ǿ� �ִٸ�
        {
            connectionInfoText.text = "Connecting to Random Room...";

            Debug.Log("Connecting to Random Room...");

            PhotonNetwork.NickName = inputUserId.text;
            Debug.Log(inputUserId.text);
            PhotonNetwork.JoinRandomRoom(); // ������ ������ ������ �õ�
        }
        else
        {
            connectionInfoText.text = "Offline : Connection Disabled - Try reconnecting...";

            Debug.Log("Offline : Connection Disabled - Try reconnecting...");
            PhotonNetwork.ConnectUsingSettings(); // ������ �õ�
        }
    }

    public void OnInputEndEdit(string value)
    {
        if (string.IsNullOrEmpty(value)) return; // �̸��� �Էµ��� �ʾ��� ��� �Լ� ����

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Connect();
        }
    }

    public override void OnJoinRandomFailed(short returCode, string message)
    {
        connectionInfoText.text = "There is no empty room, Creating new Room.";

        Debug.Log("There is no empty room, Creating new Room.");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 20 }); // ���� �����ϴ� �Լ�, �Ű�����: (�� �̸�, �� �ɼ�)
    }

    public override void OnJoinedRoom() // ���� �����ؼ� ���԰ų� ���� �濡 ������ ����Ǵ� �Լ�
    {
        connectionInfoText.text = "Connected with Room.";
        Debug.Log("Connected with Room.");
        // SceneManager.LoadScene(); // LoadScene()���� �� �̵� �� �ڽŸ� �Ѿ�� ������ ������� �Ѿ�� �����Ƿ� �� ���� �� �ش� �Լ��� ����ؼ� �ȉ�
        // PhotonNetwork.LoadLevel("Kumoh_Main"); // Photon���� �� �̵� �� PhotonNetwork.LoadLevel()�� ����ؼ� �̵��ؾ� ��
        // �⺻������ Photon PUN�� �濡 ������ Ŭ���̾�Ʈ���� ������ ���� �����ϵ��� ���� �Ǿ��־� OnJoinedRoom �Լ����� ���� �ҷ����� �ʾƵ� �⺻������ ���� ���� �����˴ϴ�.

        Camera.main.enabled = false;
        GameObject player = PhotonNetwork.Instantiate("Player", spawnPos.transform.position, spawnPos.transform.rotation);


        // �÷��̾� ��� On
        startCanvas.SetActive(false);

        // ���콺 Ŀ�� ��Ȱ��ȭ
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("New player entered the room: " + newPlayer.NickName);
    }

}
