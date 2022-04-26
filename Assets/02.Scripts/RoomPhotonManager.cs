using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace m211031
{
    public class RoomPhotonManager : Photon.PunBehaviour
    {
        public static RoomPhotonManager Instance;
        //버튼 관련
        public GameObject ExitButton;
        public Text ButtonText;
        public bool isReady = false;
        public GameObject StartButton;
        public GameObject ReadyButton;
        public Text ReadyButtonText;

        //플레이어 번호
        public int myPosIndex;
        public int myPlayerNum;

        //플레이어 UI
        public Text Player1Name;
        public Text Player2Name;
        public Text Player3Name;
        public Text Player4Name;
        public GameObject Player1Image;
        public GameObject Player2Image;
        public GameObject Player3Image;
        public GameObject Player4Image;

        //플레이어 준비 여부
        public bool AllPlayerReady = true; // 모든 플레이어 준비여부
        public bool Player1Ready = true;
        public bool Player2Ready = true;
        public bool Player3Ready = true;
        public bool Player4Ready = true;

        //플레이어 위치 
        public Transform P1Pos;
        public Transform P2Pos;
        public Transform P3Pos;
        public Transform P4Pos;

        private void Awake()
        {
            PhotonNetwork.automaticallySyncScene = true;
        }
        // Start is called before the first frame update
        void Start()
        {
            Instance = this;
            // 해쉬테이블 설정
            if (!PhotonNetwork.connected)
            {
                PhotonNetwork.LoadLevel(0);

            }
            else
            {
                // 3인 까지 접속 가능한 플레이어 넘버 부여
                ExitGames.Client.Photon.Hashtable hash = PhotonNetwork.room.CustomProperties;
                print(hash["PlayerPosition"]);
                string playerPosStr = (string)hash["PlayerPosition"]; // xxx
                myPosIndex = findEmptyIndex(ref playerPosStr); // oxx
                myPlayerNum = myPosIndex+1;
                hash["PlayerPosition"] = playerPosStr;

                PhotonNetwork.room.SetCustomProperties(hash);

                PhotonNetwork.Instantiate("P1", Vector3.up * 3, Quaternion.identity, 0); // 플레이어 프리팹 생성
            }

            // 준비버튼 , 시작버튼 설정
            if (PhotonNetwork.isMasterClient)
            {
                isReady = true;
                ReadyButton.SetActive(false);
            }
            else
            {
                isReady = false;
                StartButton.SetActive(false);
            }

            RoomStart();
            
        }


        private void Update()
        {
            // 모든 플레이어 준비 여부 체크
            if (Player1Ready && Player2Ready && Player3Ready && Player4Ready)
                AllPlayerReady = true;
            else
                AllPlayerReady = false;

        }

        public void MakePlayer()
        {
            if(myPlayerNum == 1)
            PhotonNetwork.Instantiate("P1", Vector3.up * 3, Quaternion.identity, 0);

            else if(myPlayerNum == 2)
                PhotonNetwork.Instantiate("P2", Vector3.up * 3, Quaternion.identity, 0);
        }

        [PunRPC]
        public void RoomStart()
        {
            // 플레이어 이름 UI 설정
            if (myPlayerNum == 1)
            {
                Player1Name.text = PhotonNetwork.playerName;
                Player1Image.SetActive(true);

            }

            else if (myPlayerNum == 2)
            {
                Player2Name.text = PhotonNetwork.playerName;
                Player2Image.SetActive(true);
                Player2Ready = false;
            }
            else if (myPlayerNum == 3)
            {
                Player3Name.text = PhotonNetwork.playerName;
                Player3Image.SetActive(true);
                Player3Ready = false;
            }
            else if (myPlayerNum == 4)
            {
                Player4Name.text = PhotonNetwork.playerName;
                Player4Image.SetActive(true);
                Player4Ready = false;
            }
        }
        public void MyExitRoom()
        {
            PhotonNetwork.Disconnect();
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel(0);
        }

        [PunRPC]
        public void MyStartGame()
        {
            // 내가 마스터 클라이언트고 모든 플레이어가 준비완료 했을 때
            if (PhotonNetwork.isMasterClient && AllPlayerReady)
                PhotonNetwork.LoadLevel(2);
        }

        [PunRPC]
        public void MyReadyGame()
        {

            if (isReady == false)
            {
                isReady = true;
                ReadyButtonText.color = Color.red;
                ReadyButtonText.text = "준 비 완 료";

                if (myPlayerNum == 1)
                {
                    //RoomPlayerManager.Instance
                }

                else if (myPlayerNum == 2)
                {
         
                }
                else if (myPlayerNum == 3)
                {
         
                }
                else if (myPlayerNum == 4)
                {
          
                }
            }

            else if (isReady == true)
            {
                isReady = false;
                ReadyButtonText.color = Color.black;
                ReadyButtonText.text = "준 비 하 기";

                if (myPlayerNum == 1)
                {
         
                }

                else if (myPlayerNum == 2)
                {
           
                }
                else if (myPlayerNum == 3)
                {
         
                }
                else if (myPlayerNum == 4)
                {
          
                }
            }
        }

        public override void OnLeftRoom()
        {
            // 플레이어 접속종료시 플레이어 수를 세기 위해 커스텀프로퍼티 값 수정
            print("방을 나갔습니다.");
            ExitGames.Client.Photon.Hashtable hash = PhotonNetwork.room.CustomProperties;
            print(hash["PlayerPosition"]);

            string playerPos = (string)hash["PlayerPosition"];
            string beforeTxt = playerPos.Substring(0, myPosIndex);
            string afterTxt = playerPos.Substring(myPosIndex + 1, playerPos.Length - myPosIndex - 1);

            playerPos = beforeTxt + "x" + afterTxt;
            hash["PlayerPosition"] = playerPos;
            PhotonNetwork.room.SetCustomProperties(hash);

            PlayerLeftRoom();
            
        }

        [PunRPC]
        public void PlayerLeftRoom()
        {
            // 플레이어 나가면 UI 꺼짐
            if (myPlayerNum == 1)
            {
                Player1Name.text = " ";
                Player1Image.SetActive(false);
                Player1Image.GetComponent<Image>().color = Color.white;

            }

            else if (myPlayerNum == 2)
            {
                Player2Name.text = " ";
                Player2Image.SetActive(false);
                Player2Image.GetComponent<Image>().color = Color.white;
            }
            else if (myPlayerNum == 3)
            {
                Player3Name.text = " ";
                Player3Image.SetActive(false);
                Player3Image.GetComponent<Image>().color = Color.white;
            }
            else if (myPlayerNum == 4)
            {
                Player4Name.text = " ";
                Player4Image.SetActive(false);
                Player4Image.GetComponent<Image>().color = Color.white;
            }
        }
        int findEmptyIndex(ref string str)
        {
            print("---------------------------");
            print($"현재 : {str}"); //수정전 str
            int myIndex = str.IndexOf("x");
            string beforeTxt = str.Substring(0, myIndex);
            string afterTxt = str.Substring(myIndex + 1, str.Length - myIndex - 1);

            str = beforeTxt + "o" + afterTxt;
            print($"수정 : {str}"); //수정후 str
            return myIndex;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                stream.SendNext(Player1Ready);
                stream.SendNext(Player2Ready);
                stream.SendNext(Player3Ready);
                stream.SendNext(Player4Ready);
            }

            else
            {
                Player1Ready = (bool)stream.ReceiveNext();
                Player2Ready = (bool)stream.ReceiveNext();
                Player3Ready = (bool)stream.ReceiveNext();
                Player4Ready = (bool)stream.ReceiveNext();
            }
        }


    }

   


}