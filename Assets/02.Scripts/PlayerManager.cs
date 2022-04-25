using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace m211031
{
    public class PlayerManager : MonoBehaviour, IPunObservable
    {
        public static PlayerManager Instance;

        public int Money;
        public int PigCount, ChickenCount, CowCount, SheepCount;
        public Text MoneyText;
        //public Text TotalPrice;
        public GameObject PigCountText, ChickenCountText, CowCountText, SheepCountText;
        public GameObject ResetTransform;
        public TextMesh PlayerName;
       
        public int MyNumIndex;

        //private GameObject playerUiPrefab;

        Rigidbody rig;
        float h => Input.GetAxisRaw("Horizontal");
        float v
        {
            get
            {
                return Input.GetAxisRaw("Vertical");
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            PlayerName.text = gameObject.GetComponent<PhotonView>().owner.NickName;
            MyNumIndex = GameManager.Instance.myPosIndex;
            

            Money = 1000;
            rig = GetComponent<Rigidbody>();

            if(GetComponent<PhotonView>().isMine)
            {
            TakeCamera();
            Instance = this;

            }
        
            //CameraManager.Instance.targets.Add(transform);
            GameManager.Instance.UpdatePlayerList();
            //UpdateTotal();
            //TotalMoneyUpdate();


        }
        //void TotalMoneyUpdate()
        //{
        //    int myIndexNum = GameManager.Instance.myPosIndex;
        //    // PlayersMoneyTexts[myPosIndex].text =
        //    //PlayersMoneyTexts[myPosIndex].gameObject.GetComponent<PhotonView>().owner.NickName +
        //    //PlayersMoneyTexts[myPosIndex].gameObject.GetComponent<PlayerManager>().Money;
        //    GameManager.Instance.PlayersMoneyTexts[myIndexNum].text =
        //               $"{GetComponent<PhotonView>().owner.NickName} :" +
        //               $" {GetComponent<PlayerManager>().Money}";
        //}
        // Update is called once per frame

        void Update()
        {
            if (!GetComponent<PhotonView>().isMine) return;

            Vector3 movement = new Vector3(h, 0, v);
            rig.MovePosition(rig.position + movement * 18 * Time.deltaTime);



        }

        //[PunRPC]
        //public void UpdateMyMoney()
        //{
        //    MoneyText.text = "현금 :" + " " + Money;
        //}
        public void TakeCamera()
        {
            //CameraManager.Instance.target = transform;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("BuyPig"))
                BuyPig();
            if (collision.gameObject.CompareTag("BuyChicken"))
                BuyChicken();
            if (collision.gameObject.CompareTag("BuyCow"))
                BuyCow();
            if (collision.gameObject.CompareTag("BuySheep"))
                BuySheep();
            if (collision.gameObject.CompareTag("SellPig"))
                SellPig();
            if (collision.gameObject.CompareTag("SellChicken"))
                SellChicken();
            if (collision.gameObject.CompareTag("SellCow"))
                SellCow();
            if (collision.gameObject.CompareTag("SellSheep"))
                SellSheep();
        }

        #region TradeAnimal
        [PunRPC]
        public void BuyPig()
        {
            if (Money > AnimalManager.Instance.PigPrice)
            {
                PigCount++;
                PigCountText.gameObject.GetComponent<Text>().text = "돼지" + " " + PigCount;
                Money = Money - AnimalManager.Instance.PigPrice;

                //TotalMoneyUpdate();
                GameManager.Instance.UpdateMyMoney();
                GameManager.Instance.UpdateCount();
            }
            else
            {
                print("현금 부족");
            }
            transform.position = ResetTransform.transform.position;
            

        }
        [PunRPC]
        public void BuyChicken()
        {
            if (Money > AnimalManager.Instance.ChickenPrice)
            {
                ChickenCount++;
                ChickenCountText.gameObject.GetComponent<Text>().text = "치킨" + " " + ChickenCount;
                Money = Money - AnimalManager.Instance.ChickenPrice;

                //TotalMoneyUpdate();
                GameManager.Instance.UpdateMyMoney();
                GameManager.Instance.UpdateCount();
            }
            else
            {
                print("현금 부족");
            }
            transform.position = ResetTransform.transform.position;
            
        }
        [PunRPC]
        public void BuyCow()
        {
            if (Money > AnimalManager.Instance.CowPrice)
            {
                CowCount++;
                CowCountText.gameObject.GetComponent<Text>().text = "소" + " " + CowCount;
                Money = Money - AnimalManager.Instance.CowPrice;

                GameManager.Instance.UpdateMyMoney();
                GameManager.Instance.UpdateCount();
            }
            else
            {
                print("현금 부족");
            }
            transform.position = ResetTransform.transform.position;
        }
        [PunRPC]
        public void BuySheep()
        {
            if (Money > AnimalManager.Instance.SheepPrice)
            {
                SheepCount++;
                SheepCountText.gameObject.GetComponent<Text>().text = "양" + " " + SheepCount;
                Money = Money - AnimalManager.Instance.SheepPrice;

                GameManager.Instance.UpdateMyMoney();
                GameManager.Instance.UpdateCount();
            }
            else
            {
                print("현금 부족");
            }
            transform.position = ResetTransform.transform.position;
        }
        [PunRPC]
        public void SellPig()
        {
            if (PigCount >= 1)
            {
                PigCount--;
                PigCountText.gameObject.GetComponent<Text>().text = "돼지" + " " + PigCount;
                Money = Money + AnimalManager.Instance.PigPrice;

                //TotalMoneyUpdate();
                GameManager.Instance.UpdateMyMoney();
                GameManager.Instance.UpdateCount();
            }
            else
            {
                print("보유 한 돼지가 없습니다");
            }
            transform.position = ResetTransform.transform.position;
            
        }
        [PunRPC]
        public void SellChicken()
        {
            if (ChickenCount >= 1)
            {
                ChickenCount--;
                ChickenCountText.gameObject.GetComponent<Text>().text = "치킨" + " " + ChickenCount;
                Money = Money + AnimalManager.Instance.ChickenPrice;
                GameManager.Instance.UpdateMyMoney();
                GameManager.Instance.UpdateCount();
                //TotalMoneyUpdate();
            }
            else
                print("보유한 닭이 없습니다");

            transform.position = ResetTransform.transform.position;
        }
        [PunRPC]
        public void SellCow()
        {
            if (CowCount >= 1)
            {
                CowCount--;
                CowCountText.gameObject.GetComponent<Text>().text = "소" + " " + CowCount;
                Money = Money + AnimalManager.Instance.CowPrice;

                GameManager.Instance.UpdateMyMoney();
                GameManager.Instance.UpdateCount();
            }
            else
                print("보유한 소가 없습니다");

            transform.position = ResetTransform.transform.position;
        }
        [PunRPC]
        public void SellSheep()
        {
            if (SheepCount >= 1)
            {
                SheepCount--;
                SheepCountText.gameObject.GetComponent<Text>().text = "양" + " " + SheepCount;
                Money = Money + AnimalManager.Instance.SheepPrice;

                GameManager.Instance.UpdateMyMoney();
                GameManager.Instance.UpdateCount();
            }
            else
                print("보유한 양이 없습니다");

            transform.position = ResetTransform.transform.position;
        }

        #endregion
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                stream.SendNext(Money);
            }
            else
            {
                 Money = (int)stream.ReceiveNext();
            }
        }
        
       [PunRPC]
        public void TotalMoneyUpdate()
        {

            int myIndexNum = GameManager.Instance.myPosIndex;

            PlayerName.text = GameManager.Instance.Players[myIndexNum].GetComponent<PhotonView>().owner.NickName;

            GameManager.Instance.PlayersMoneyTexts[myIndexNum].text =
              GameManager.Instance.Players[myIndexNum].GetComponent<PhotonView>().owner.name + " " 
               + GameManager.Instance.Players[myIndexNum].gameObject.GetComponent<PlayerManager>().Money;
        }

        [PunRPC]
        public void UpdateTotal()
        {
            GameManager.Instance.PlayersMoneyTexts[MyNumIndex].text = GameManager.Instance.Players[MyNumIndex].GetComponent<PhotonView>().owner.name + " " +
               GameManager.Instance.Players[MyNumIndex].GetComponent<PlayerManager>().Money;
        }
    }
}