using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace m211031
{


    public class RoomPlayerManager : Photon.PunBehaviour
    {
        public static RoomPlayerManager Instance;

        //플레이어 번호
        public int myPosIndex;
        public int myPlayerNum;

        //플레이어 UI
        public Text PlayerName;
        public GameObject PlayerImage;

        //플레이어 UI버튼
        public GameObject ReadyButton;
        public Text ReadyButtonText;

        //플레이어 준비 여부
        public bool isReady = false;
        public bool PlayerReady = true;

        //UI 캔버스
        public GameObject Canvas;
        public GameObject StartButton;


        // Start is called before the first frame update
        void Start()
        {
            Instance = this;
            myPlayerNum = RoomPhotonManager.Instance.myPosIndex + 1; // 플레이어 넘버 부여
            PlayerName.text = this.gameObject.GetComponent<PhotonView>().owner.NickName; // 플레이어 닉네임 설정
            Canvas = GameObject.Find("Players"); // UI 캔버스 자식으로 프리팹 넣기
            this.gameObject.transform.SetParent(Canvas.transform, false); // 플레이어 위치 설정

            // 버튼 위치 설정
            StartButton = GameObject.Find("StartButtonPos");


            if (!PhotonNetwork.isMasterClient && this.gameObject.GetComponent<PhotonView>().isMine)
            {
                ReadyButton.SetActive(true);
                //ReadyButton = GameObject.Find("ReadyButton"); // 버튼 설정
                ReadyButtonText = ReadyButton.GetComponentInChildren<Text>();
                ReadyButton.transform.position = StartButton.transform.position;
            }


            if (myPlayerNum == 1)
            {
                this.gameObject.transform.position = RoomPhotonManager.Instance.P1Pos.transform.position;
                
            }

            else if (myPlayerNum == 2)
            {
                this.gameObject.transform.position = RoomPhotonManager.Instance.P2Pos.transform.position;
                RoomPhotonManager.Instance.Player2Ready = false;
            }
            else if (myPlayerNum == 3)
            {
                this.gameObject.transform.position = RoomPhotonManager.Instance.P3Pos.transform.position;
                RoomPhotonManager.Instance.Player3Ready = false;
            }
            else if (myPlayerNum == 4)
            {
                this.gameObject.transform.position = RoomPhotonManager.Instance.P4Pos.transform.position;
                RoomPhotonManager.Instance.Player4Ready = false;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        [PunRPC]
        public void MyReadyGame()
        {

            if (isReady == false)
            {
                isReady = true; // 이 플레이어 준비상태 변경
                ReadyButtonText.color = Color.red; // 버튼텍스트 색 변경
                ReadyButtonText.text = "준 비 완 료"; // 버튼텍스트 내용 변경
                PlayerImage.GetComponent<Image>().color = Color.red; // 이 플레이어 UI이미지 색 변경
                PlayerReady = true; // 준비완료 상태 변경
                
                if(myPlayerNum == 2)
                {
                    RoomPhotonManager.Instance.Player2Ready = true;
                }
            }

            else if (isReady == true)
            {
                isReady = false;
                ReadyButtonText.color = Color.black;
                ReadyButtonText.text = "준 비 하 기";
                PlayerImage.GetComponent<Image>().color = Color.white;
                PlayerReady = false;

                if (myPlayerNum == 2)
                {
                    RoomPhotonManager.Instance.Player2Ready = false;
                }
            }
        }


        //public void onphotonserializeview(photonstream stream, photonmessageinfo info)
        //{
        //    if (stream.iswriting)
        //    {
        //        stream.sendnext(playerimage.getcomponent<image>().color);

        //        stream.sendnext(pigpricetext);
        //        stream.sendnext(chickenpricetext);
        //        stream.sendnext(cowpricetext);
        //        stream.sendnext(sheeppricetext);
        //    }
        //    else
        //    {
        //        playerimage.getcomponent<image>().color = (color)stream.receivenext();

        //    }
        //}
    }
}
