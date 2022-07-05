using UnityEngine;
using UnityEngine.UI;

public class ModeSelector : MonoBehaviour
{
    public enum CollectTag
    {
        Cherry,
        Enemy
    }

    public enum GameMode
    {
        Collectathon,
        TimeTrial
    }

    public GameManager manager;
    public GameMode gameMode;
    public CollectTag tagToCollect;
    public float levelTimer;
    private GameObject[] _objects;

    [Header("UI")]
    public Text collectathonText;
    public Image image;
    public Sprite cherrySprite;
    public Sprite enemySprite;

    private void Update()
    {
        switch (gameMode)
        {
            case GameMode.Collectathon:
            {
                image.sprite = tagToCollect switch
                {
                    CollectTag.Cherry => cherrySprite,
                    CollectTag.Enemy => enemySprite
                };
                _objects = GameObject.FindGameObjectsWithTag(tagToCollect.ToString());
                collectathonText.text = $"x{_objects.Length}";
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

        // var text = $"Time: {levelTimer.ToString("F1")}";
        // var text = $"Time: {levelTimer:F1}";
    }
}
