using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace m211031
{

    public class UiManager : Photon.PunBehaviour, IPunObservable
    {
        public static UiManager Instance;

        public float rTime = 15;
        public Text TimeText;
        public int Round = 1;
        public Text RoundText;

        public Text[] PlayersMoneyTexts;

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                stream.SendNext(rTime);
                stream.SendNext(Round);
                //stream.SendNext(TimeText);
                //stream.SendNext(RoundText);
            }
            else
            {
                rTime = (float)stream.ReceiveNext();
                rTime = (int)stream.ReceiveNext();
                //TimeText = (Text)stream.ReceiveNext();
                //RoundText = (Text)stream.ReceiveNext();
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            Instance = this;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}