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
            print("°¡°Ýº¯µ¿");
            PigPrice = Random.Range(100, 1000);
            PigPriceText.text = "µÅÁö : " + "" + PigPrice;
            ChickenPrice = Random.Range(100, 1000);
            ChickenPriceText.text = "´ß :" + "" + ChickenPrice;
            CowPrice = Random.Range(100, 1000);
            CowPriceText.text = "¼Ò : " + "" + CowPrice;
            SheepPrice = Random.Range(100, 1000);
            SheepPriceText.text = "¾ç : " + "" + SheepPrice;
        }

        [PunRPC]
        public void UpdatePriceUI()
        {
            PigPriceText.text = "µÅÁö : " + "" + PigPrice;
            ChickenPriceText.text = "´ß :" + "" + ChickenPrice;
            CowPriceText.text = "¼Ò : " + "" + CowPrice;
            SheepPriceText.text = "¾ç : " + "" + SheepPrice;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {

                if (stream.isWriting)
                {
                    stream.SendNext(PigPrice);
                    stream.SendNext(ChickenPrice);
                    stream.SendNext(CowPrice);
                    stream.SendNext(SheepPrice);
                }
                else
                {
                    PigPrice = (int)stream.ReceiveNext();
                    ChickenPrice = (int)stream.ReceiveNext();
                    CowPrice = (int)stream.ReceiveNext();
                    SheepPrice = (int)stream.ReceiveNext();
                }
        }
    }
}
