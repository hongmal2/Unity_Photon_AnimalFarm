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

        //�÷��̾� ��ȣ
        public int myPosIndex;
        public int myPlayerNum;

        //�÷��̾� UI
        public Text PlayerName;
        public GameObject PlayerImage;

        //�÷��̾� UI��ư
        public GameObject ReadyButton;
        public Text ReadyButtonText;

        //�÷��̾� �غ� ����
        public bool isReady = false;
        public bool PlayerReady = true;

        //UI ĵ����
        public GameObject Canvas;
        public GameObject StartButton;


        // Start is called before the first frame update
        void Start()
        {
            Instance = this;
            myPlayerNum = RoomPhotonManager.Instance.myPosIndex + 1; // �÷��̾� �ѹ� �ο�
            PlayerName.text = this.gameObject.GetComponent<PhotonView>().owner.NickName; // �÷��̾� �г��� ����
            Canvas = GameObject.Find("Players"); // UI ĵ���� �ڽ����� ������ �ֱ�
            this.gameObject.transform.SetParent(Canvas.transform, false); // �÷��̾� ��ġ ����

            // ��ư ��ġ ����
            StartButton = GameObject.Find("StartButtonPos");


            if (!PhotonNetwork.isMasterClient && this.gameObject.GetComponent<PhotonView>().isMine)
            {
                ReadyButton.SetActive(true);
                //ReadyButton = GameObject.Find("ReadyButton"); // ��ư ����
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
                isReady = true; // �� �÷��̾� �غ���� ����
                ReadyButtonText.color = Color.red; // ��ư�ؽ�Ʈ �� ����
                ReadyButtonText.text = "�� �� �� ��"; // ��ư�ؽ�Ʈ ���� ����
                PlayerImage.GetComponent<Image>().color = Color.red; // �� �÷��̾� UI�̹��� �� ����
                PlayerReady = true; // �غ�Ϸ� ���� ����
                
                if(myPlayerNum == 2)
                {
                    RoomPhotonManager.Instance.Player2Ready = true;
                }
            }

            else if (isReady == true)
            {
                isReady = false;
                ReadyButtonText.color = Color.black;
                ReadyButtonText.text = "�� �� �� ��";
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
