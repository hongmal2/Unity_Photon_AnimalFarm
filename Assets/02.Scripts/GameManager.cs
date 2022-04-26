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
                print($"나의 자리는 {myPosIndex + 1} 번 입니다.");
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

                //statusTxt.text = "기다리는중..";
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
                TimeText.text = "장 마감 시간 : " + Mathf.Round(rTime);
                RoundText.text = Round + "라운드";
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
        public void UpdateMyMoney() // 플레이어 개인 UI 변경 ( 게임머니 보유 양 )
        {
            MoneyText.text = "현금 :" + " " + PlayerManager.Instance.Money;

            
        }

        [PunRPC]
        public void UpdateCount() // 플레이어 개인 UI 변경 ( 동물 보유 수 )
        {
            PigCountText.gameObject.GetComponent<Text>().text = "돼지" + " " + PlayerManager.Instance.PigCount;
            ChickenCountText.gameObject.GetComponent<Text>().text = "닭" + " " + PlayerManager.Instance.ChickenCount;
            CowCountText.gameObject.GetComponent<Text>().text = "소" + " " + PlayerManager.Instance.CowCount;
            SheepCountText.gameObject.GetComponent<Text>().text = "양" + " " + PlayerManager.Instance.SheepCount;
        }

        [PunRPC]
        public void UpdateTime() // 플레이어 전체 UI 변경 ( 시간 , 라운드 )
        {
            TimeText.text = "장 마감 시간 : " + Mathf.Round(rTime);
            RoundText.text = Round + "라운드";
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
                        PlayersMoneyTexts[i].text = "플레이어" + i + " : " + 0 + "원";
                        return;
                    }

                    else
                    {
                        PlayersMoneyTexts[i].text = Players[i].GetComponent<PhotonView>().owner.NickName
                            + " : " + Players[i].GetComponent<PlayerManager>().Money + " 원 ";
                    }
                }
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
            print("방을 나갔습니다.");
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
            print("gameScene : 마스터에 접속했어요.");
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
