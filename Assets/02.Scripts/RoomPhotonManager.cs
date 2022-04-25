using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace m211031
{
    public class RoomPhotonManager : Photon.PunBehaviour
    {
        //��ư ����
        public GameObject ExitButton;
        public Text ButtonText;
        public bool isReady = false;
        public GameObject StartButton;
        public GameObject ReadyButton;
        public Text ReadyButtonText;

        //�÷��̾� ��ȣ
        public int myPosIndex;
        public int myPlayerNum;

        //�÷��̾� UI
        public Text Player1Name;
        public Text Player2Name;
        public Text Player3Name;
        public Text Player4Name;
        public GameObject Player1Image;
        public GameObject Player2Image;
        public GameObject Player3Image;
        public GameObject Player4Image;

        //�÷��̾� �غ� ����
        public bool AllPlayerReady = true; // ��� �÷��̾� �غ񿩺�
        public bool Player1Ready = true;
        public bool Player2Ready = true;
        public bool Player3Ready = true;
        public bool Player4Ready = true;

        private void Awake()
        {
            PhotonNetwork.automaticallySyncScene = true;
        }
        // Start is called before the first frame update
        void Start()
        {

            // �ؽ����̺� ����
            if (!PhotonNetwork.connected)
            {
                PhotonNetwork.LoadLevel(0);

            }
            else
            {
                ExitGames.Client.Photon.Hashtable hash = PhotonNetwork.room.CustomProperties;
                print(hash["PlayerPosition"]);
                string playerPosStr = (string)hash["PlayerPosition"]; // xxx
                myPosIndex = findEmptyIndex(ref playerPosStr); // oxx
                myPlayerNum = myPosIndex+1;
                hash["PlayerPosition"] = playerPosStr;
                print(myPosIndex);
                print($"���� �ڸ��� {myPosIndex + 1} �� �Դϴ�.");
                print(PhotonPlayer.Find(2));
                //transform.position = positions[myPosIndex].position;

                PhotonNetwork.room.SetCustomProperties(hash);
                print(PhotonNetwork.playerName);
          

            }

            // �غ��ư , ���۹�ư ����
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

            // �÷��̾� �̸� UI ����
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

        private void Update()
        {
            // ��� �÷��̾� �غ� ���� üũ
            if (Player1Ready && Player2Ready && Player3Ready && Player4Ready)
                AllPlayerReady = true;
            else
                AllPlayerReady = false;

        }
        public void MyExitRoom()
        {
            PhotonNetwork.Disconnect();
            PhotonNetwork.LoadLevel(0);
        }

        public void MyStartGame()
        {
            // ���� ������ Ŭ���̾�Ʈ�� ��� �÷��̾ �غ�Ϸ� ���� ��
            if (PhotonNetwork.isMasterClient && AllPlayerReady)
                PhotonNetwork.LoadLevel(2);
        }

        public void MyReadyGame()
        {
            if (isReady == false)
            {
                isReady = true;
                ReadyButtonText.color = Color.red;
                ReadyButtonText.text = "�� �� �� ��";

                if (myPlayerNum == 1)
                {
                    Player1Image.GetComponent<Image>().color = Color.red; // �غ�Ϸ� �� ����
                    Player1Ready = true; // �غ�Ϸ� ���� ����
                }

                else if (myPlayerNum == 2)
                {
                    Player2Image.GetComponent<Image>().color = Color.red;
                    Player2Ready = true;
                }
                else if (myPlayerNum == 3)
                {
                    Player3Image.GetComponent<Image>().color = Color.red;
                    Player3Ready = true;
                }
                else if (myPlayerNum == 4)
                {
                    Player4Image.GetComponent<Image>().color = Color.red;
                    Player4Ready = true;
                }
            }

            else if (isReady == true)
            {
                isReady = false;
                ReadyButtonText.color = Color.black;
                ReadyButtonText.text = "�� �� �� ��";

                if (myPlayerNum == 1)
                {
                    Player1Image.GetComponent<Image>().color = Color.white;
                    Player1Ready = false;
                }

                else if (myPlayerNum == 2)
                {
                    Player2Image.GetComponent<Image>().color = Color.white;
                    Player2Ready = false;
                }
                else if (myPlayerNum == 3)
                {
                    Player3Image.GetComponent<Image>().color = Color.white;
                    Player3Ready = false;
                }
                else if (myPlayerNum == 4)
                {
                    Player4Image.GetComponent<Image>().color = Color.white;
                    Player4Ready = false;
                }
            }
        }

        public override void OnLeftRoom()
        {

            print("���� �������ϴ�.");
            ExitGames.Client.Photon.Hashtable hash = PhotonNetwork.room.CustomProperties;
            print(hash["PlayerPosition"]);

            string playerPos = (string)hash["PlayerPosition"];
            string beforeTxt = playerPos.Substring(0, myPosIndex);
            string afterTxt = playerPos.Substring(myPosIndex + 1, playerPos.Length - myPosIndex - 1);

            playerPos = beforeTxt + "x" + afterTxt;
            hash["PlayerPosition"] = playerPos;
            PhotonNetwork.room.SetCustomProperties(hash);

            // �÷��̾� ������ UI ����
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
            print($"���� : {str}"); //������ str
            int myIndex = str.IndexOf("x");
            string beforeTxt = str.Substring(0, myIndex);
            string afterTxt = str.Substring(myIndex + 1, str.Length - myIndex - 1);

            str = beforeTxt + "o" + afterTxt;
            print($"���� : {str}"); //������ str
            return myIndex;
        }
    }


}