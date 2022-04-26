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
            NameInputField.GetComponent<InputField>().text = PlayerPrefs.GetString("이름");
            StatusText.text = "";
            //print(PlayerPrefs.HasKey("이름"));
            //PlayerPrefs.SetString("이름", "abc");
            //print(PlayerPrefs.HasKey("이름"));
            //print(PlayerPrefs.GetString("이름"));
            NameInputField.GetComponent<InputField>().onValueChanged.AddListener((string s) => { });
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void MyUpdateText()
        {
            text = NameInputField.GetComponent<InputField>().text;
            PlayerPrefs.SetString("이름", NameInputField.GetComponent<InputField>().text);
            PhotonNetwork.playerName = text; //포톤 네트워크에 플레이어 이름 설정
            print($"Static call {text}");
        }
        public void MyDinamicUpdateText(string value)
        {
            text = value;
            PlayerPrefs.SetString("이름", value);
            PhotonNetwork.playerName = value;  //포톤 네트워크에 플레이어 이름 설정
            print($"Dinamic call {text}");
        }

        public void MyOnStartGame()
        {
            JoinButton.SetActive(false);
            NameInputField.SetActive(false);
            StatusText.text = "접속중..";

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
            print("마스터 접속");
            PhotonNetwork.JoinRandomRoom();
        }
        public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
        {
            print("방에 조인 실패");
            PhotonNetwork.CreateRoom(null);
        }
        public override void OnJoinedRoom()
        {
            print("방에 조인 성공");
            PhotonNetwork.LoadLevel(1);
        }
        public override void OnCreatedRoom()
        {
            print("방을 만들었음");
            ExitGames.Client.Photon.Hashtable hash =
                PhotonNetwork.room.customProperties; // 방에 커스텀프로퍼티 적용

            hash["PlayerPosition"] = "xxx";
            PhotonNetwork.room.SetCustomProperties(hash);
        }
    }
}