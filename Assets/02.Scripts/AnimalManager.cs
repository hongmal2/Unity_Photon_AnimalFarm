using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace m211031
{
    public class AnimalManager : MonoBehaviour , IPunObservable
    {
        public static AnimalManager Instance;


        public GameObject Pig, Chicken, Cow, Sheep;
        public int PigPrice, ChickenPrice, CowPrice, SheepPrice;
        public Text PigPriceText, ChickenPriceText, CowPriceText, SheepPriceText;
        // Start is called before the first frame update
        void Start()
        {
            Instance = this;

            PigPrice = 100;
            ChickenPrice = 100;
            CowPrice = 100;
            SheepPrice = 100;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdatePrice()
        {
            print("���ݺ���");
            PigPrice = Random.Range(100, 1000);
            PigPriceText.text = "���� : " + "" + PigPrice;
            ChickenPrice = Random.Range(100, 1000);
            ChickenPriceText.text = "�� :" + "" + ChickenPrice;
            CowPrice = Random.Range(100, 1000);
            CowPriceText.text = "�� : " + "" + CowPrice;
            SheepPrice = Random.Range(100, 1000);
            SheepPriceText.text = "�� : " + "" + SheepPrice;
        }

        [PunRPC]
        public void UpdatePriceUI()
        {
            PigPriceText.text = "���� : " + "" + PigPrice;
            ChickenPriceText.text = "�� :" + "" + ChickenPrice;
            CowPriceText.text = "�� : " + "" + CowPrice;
            SheepPriceText.text = "�� : " + "" + SheepPrice;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {

                if (stream.isWriting)
                {
                    stream.SendNext(PigPrice);
                    stream.SendNext(ChickenPrice);
                    stream.SendNext(CowPrice);
                    stream.SendNext(SheepPrice);
                    //stream.SendNext(PigPriceText);
                    //stream.SendNext(ChickenPriceText);
                    //stream.SendNext(CowPriceText);
                    //stream.SendNext(SheepPriceText);
                }
                else
                {
                    PigPrice = (int)stream.ReceiveNext();
                    ChickenPrice = (int)stream.ReceiveNext();
                    CowPrice = (int)stream.ReceiveNext();
                    SheepPrice = (int)stream.ReceiveNext();
                    //PigPriceText = (Text)stream.ReceiveNext();
                    //ChickenPriceText = (Text)stream.ReceiveNext();
                    //CowPriceText = (Text)stream.ReceiveNext();
                    //SheepPriceText = (Text)stream.ReceiveNext();
                }
        }
    }
}
