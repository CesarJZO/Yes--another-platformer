using UnityEngine;

namespace mastermind
{
    public class ModeSelector : MonoBehaviour
    {
        public GameManager manager;
        public GameMode gameMode;
        public CollectTag tagToCollect;
        private GameObject[] _objects;
        public float levelTimer;
        
        
        public enum CollectTag { Cherry, Enemy }

        public enum GameMode { Collectathon, TimeTrial }

        private void Update()
        {
            switch (gameMode)
            {
                case GameMode.Collectathon:
                {
                    _objects = GameObject.FindGameObjectsWithTag(tagToCollect.ToString());
                    if (_objects.Length <= 0) manager.FinishLevel();
                    break;
                }
                case GameMode.TimeTrial when manager.gameOver || manager.levelFinished:
                    return;
                case GameMode.TimeTrial when levelTimer > 0:
                    levelTimer -= Time.deltaTime;
                    break;
                case GameMode.TimeTrial:
                {
                    if (levelTimer < 0)
                        levelTimer = 0;
                    if (!manager.gameOver)
                    {
                        manager.gameOver = true;
                        manager.player.Die();
                    }

                    break;
                }
            }
        }
    }
}
