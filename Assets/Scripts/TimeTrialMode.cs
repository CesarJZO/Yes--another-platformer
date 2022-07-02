using UnityEngine;

public class TimeTrialMode : MonoBehaviour
{
    public GameManager manager;
    public float levelTimer;

    private void Update()
    {
        if (manager.gameOver || manager.levelFinished) return;
        if (levelTimer > 0)
        {
            levelTimer -= Time.deltaTime;
        }
        else
        {
            if (levelTimer < 0)
                levelTimer = 0;
            if (!manager.gameOver)
            {
                manager.gameOver = true;
                manager.player.Die();
            }
        }
    }
}
