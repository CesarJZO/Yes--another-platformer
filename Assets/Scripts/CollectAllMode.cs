using System;
using UnityEngine;

namespace mastermind
{
    public class CollectAllMode : MonoBehaviour
    {
        public GameManager manager;
        public CollectTag tagToCollect;
        public GameObject[] objects;

        public enum CollectTag { Cherry, Enemy }
        
        private void Update()
        {
            objects = GameObject.FindGameObjectsWithTag(tagToCollect.ToString());
            if (objects.Length <= 0) manager.FinishLevel();
        }
    }
}