using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace m211031
{
    public class PhotonManager : Photon.PunBehaviour
    {
        public GameObject JoinButton;
        public GameObject NameInputField;
        public Text StatusText;
        public string text;
        //222
        // Start is called before the first frame update
        void Start()
        {
            NameInputField.GetComponent<InputField>().text = PlayerPrefs.GetString("�̸�");
            StatusText.text = "";
            //print(PlayerPrefs.HasKey("�̸�"));
            //PlayerPrefs.SetString("�̸�", "abc");
            //print(PlayerPrefs.HasKey("�̸�"));
            //print(PlayerPrefs.GetString("�̸�"));
            NameInputField.GetComponent<InputField>().onValueChanged.AddListener((string s) => { });
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void MyUpdateText()
        {
            text = NameInputField.GetComponent<InputField>().text;
            PlayerPrefs.SetString("�̸�", NameInputField.GetComponent<InputField>().text);
            PhotonNetwork.playerName = text; //���� ��Ʈ��ũ�� �÷��̾� �̸� ����
            print($"Static call {text}");
        }
        public void MyDinamicUpdateText(string value)
        {
            text = value;
            PlayerPrefs.SetString("�̸�", value);
            PhotonNetwork.playerName = value;  //���� ��Ʈ��ũ�� �÷��̾� �̸� ����
            print($"Dinamic call {text}");
        }

        public void MyOnStartGame()
        {
            JoinButton.SetActive(false);
            NameInputField.SetActive(false);
            StatusText.text = "������..";

            //print(JoinButton.GetType());
            //print(NameInputField.GetType());
            //print(StatusText.GetType());

            if (PhotonNetwork.connected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.ConnectUsingSettings("1");
            }
        }
        public override void OnConnectedToMaster()
        {
            print("������ ����");
            PhotonNetwork.JoinRandomRoom();
        }
        public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
        {
            print("�濡 ���� ����");
            PhotonNetwork.CreateRoom(null);
        }
        public override void OnJoinedRoom()
        {
            print("�濡 ���� ����");
            PhotonNetwork.LoadLevel(1);
        }
        public override void OnCreatedRoom()
        {
            print("���� �������");
            ExitGames.Client.Photon.Hashtable hash =
                PhotonNetwork.room.customProperties; // �濡 Ŀ����������Ƽ ����

            hash["PlayerPosition"] = "xxx";
            PhotonNetwork.room.SetCustomProperties(hash);
        }
    }
}