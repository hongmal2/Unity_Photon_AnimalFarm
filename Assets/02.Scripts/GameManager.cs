using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace m211031
{

    public class GameManager : Photon.PunBehaviour, IPunObservable
    {
        public static GameManager Instance;

        public float rTime = 15;

        public Text TimeText;
        public int Round = 1;
        public Text RoundText;
        public Text PlayerMoneyText;
        
        public Text MoneyText;
        public Text PigCountText, ChickenCountText, CowCountText, SheepCountText;

        public Text[] PlayersMoneyTexts;
        public GameObject[] Players;
        public string[] PlayerName;
        public int[] PlayerMoney;
        //public List<GameObject> PlayerList = new List<GameObject>();

        public int myPosIndex;
        // Start is called before the first frame update
        void Start()
        {
            UpdatePlayerList();
            Instance = this;
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
                hash["PlayerPosition"] = playerPosStr;
                print(myPosIndex);
                print($"���� �ڸ��� {myPosIndex + 1} �� �Դϴ�.");
                print(PhotonPlayer.Find(2));
                //transform.position = positions[myPosIndex].position;
                PhotonNetwork.room.SetCustomProperties(hash);

                PhotonNetwork.Instantiate("MyPlayer", Vector3.up * 3, Quaternion.identity, 0);
                //UpdatePlayer();

                //UpdateAllPlayer();

                print("number of player : " + Players.Length);
                //PlayerName[myPosIndex] = gameObject.GetComponent<PhotonView>().owner.NickName;
                //PlayerMoney[myPosIndex] = gameObject.GetComponent<PlayerManager>().Money;
                //PlayerManager.Instance.MyNumIndex = myPosIndex;

                //statusTxt.text = "��ٸ�����..";
                //playBtn.interactable = false;

            }

            //GameObject _obj = Instantiate(PlayerList) as GameObject;
        }

        // Update is called once per frame
        void Update()
        {
            //PlayerManager.Instance.UpdateTotal();
            if (PhotonNetwork.isMasterClient)
            {
                
                
                rTime -= Time.deltaTime;

                if (rTime < 0)
                {
                    rTime = 15f;
                    //UpdateAllPlayer();
                    Round++;
                    int myIndexNum = GameManager.Instance.myPosIndex;


                    //
                    AnimalManager.Instance.UpdatePrice();
                    
                    
                    //UpdatePlayer();
                    //PlayerMoneyText.text = PlayerManager.Instance.name + " " + PlayerManager.Instance.Money;
                }
                TimeText.text = "�� ���� �ð� : " + Mathf.Round(rTime);
                RoundText.text = Round + "����";
                UpdateAllPlayer();
            }

            else
            {
                //UpdateAllPlayer();
                AnimalManager.Instance.UpdatePriceUI();
                UpdateTime();
                UpdateAllPlayer();
                //TotalMoneyUpdate();
                //PlayerManager.Instance.TotalMoneyUpdate();
            }
            //PlayerManager.Instance.TotalMoneyUpdate();
        }
        [PunRPC]
        public void UpdateMyMoney() // �÷��̾� ���� UI ���� ( ���ӸӴ� ���� �� )
        {
            MoneyText.text = "���� :" + " " + PlayerManager.Instance.Money;

            
        }

        [PunRPC]
        public void UpdateCount() // �÷��̾� ���� UI ���� ( ���� ���� �� )
        {
            PigCountText.gameObject.GetComponent<Text>().text = "����" + " " + PlayerManager.Instance.PigCount;
            ChickenCountText.gameObject.GetComponent<Text>().text = "��" + " " + PlayerManager.Instance.ChickenCount;
            CowCountText.gameObject.GetComponent<Text>().text = "��" + " " + PlayerManager.Instance.CowCount;
            SheepCountText.gameObject.GetComponent<Text>().text = "��" + " " + PlayerManager.Instance.SheepCount;
        }

        [PunRPC]
        public void UpdateTime() // �÷��̾� ��ü UI ���� ( �ð� , ���� )
        {
            TimeText.text = "�� ���� �ð� : " + Mathf.Round(rTime);
            RoundText.text = Round + "����";
        }
   
        [PunRPC]
        public void UpdatePlayer()
        {
            print(myPosIndex);
            PlayersMoneyTexts[myPosIndex].text =
            PlayersMoneyTexts[myPosIndex].gameObject.GetComponent<PhotonView>().owner.NickName +
            PlayersMoneyTexts[myPosIndex].gameObject.GetComponent<PlayerManager>().Money;
        }

        public void UpdateAllPlayer()
        {
            for (int i = 0; i < Players.Length; i++)
            {
                {
                    if (Players[i] == null)
                    {
                        PlayersMoneyTexts[i].text = "�÷��̾�" + i + " : " + 0 + "��";
                        return;
                    }

                    else
                    {
                        PlayersMoneyTexts[i].text = Players[i].GetComponent<PhotonView>().owner.NickName
                            + " : " + Players[i].GetComponent<PlayerManager>().Money + " �� ";
                    }
                }
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

        [PunRPC]
        public void UpdatePlayerList()
        {
            Players = GameObject.FindGameObjectsWithTag("Player");
        }
        public void MyLeftRoom()
        {
            //PlayerManager.Instance.removeTrnFromCamera();

            PhotonNetwork.LeaveRoom();
        }

        public override void OnLeftRoom()
        {
            UpdatePlayerList();
            print("���� �������ϴ�.");
            ExitGames.Client.Photon.Hashtable hash = PhotonNetwork.room.CustomProperties;
            print(hash["PlayerPosition"]);

            string playerPos = (string)hash["PlayerPosition"];
            string beforeTxt = playerPos.Substring(0, myPosIndex);
            string afterTxt = playerPos.Substring(myPosIndex + 1, playerPos.Length - myPosIndex - 1);

            playerPos = beforeTxt + "x" + afterTxt;
            hash["PlayerPosition"] = playerPos;
            PhotonNetwork.room.SetCustomProperties(hash);
        }

        public override void OnConnectedToMaster()
        {
            print("gameScene : �����Ϳ� �����߾��.");
            PhotonNetwork.LoadLevel(0);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                stream.SendNext(rTime);
                stream.SendNext(Round);
                stream.SendNext(myPosIndex);
                //stream.SendNext(PlayersMoneyTexts);
                //stream.SendNext(TimeText);
                //stream.SendNext(RoundText);
            }
            else
            {
                rTime = (float)stream.ReceiveNext();
                Round = (int)stream.ReceiveNext();
                myPosIndex = (int)stream.ReceiveNext();
                //PlayersMoneyTexts = (Text[])stream.ReceiveNext();
                //TimeText = (Text)stream.ReceiveNext();
                //RoundText = (Text)stream.ReceiveNext();
            }

        }
    }
}
