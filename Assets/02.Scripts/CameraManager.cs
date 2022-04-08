using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace m211031
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance;
        public Transform target;
        public List<Transform> targets;

        int currentIndex = 0;
        //public void MyNextPlayer()
        //{
        //    currentIndex++;
        //    currentIndex = targets.Count <= currentIndex ? 0 : currentIndex;
        //    target = targets[currentIndex];
        //}

        //public void MyPrevPlayer()
        //{
        //    currentIndex--;
        //    currentIndex = currentIndex < 0 ? targets.Count - 1 : currentIndex;
        //    target = targets[currentIndex];
        //}

        void Start()
        {
            Instance = this;
        }
        Vector3 offset = new Vector3(0, 95.6f, -35.3f);

        void Update()
        {
            if (target != null)
            {
                transform.position = target.position + offset;
            }
        }
    }
}
