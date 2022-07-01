using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int coins;
    public int lives;

    public Transform spawnPoint;
    public PlayerController player;

    public float timeToRespawn;
    public float timer;

    public bool gameOver;
    
    private void Start()
    {
        player.transform.position = spawnPoint.position;
    }

    private void Update()
    {
        if (player.isAlive || gameOver) return;
        if (timer <= timeToRespawn)
        {
            timer += Time.deltaTime;
        }
        else
        {
            if (lives > 0)
            {
                lives--;
                player.transform.position = spawnPoint.position;
                player.isAlive = true;
                timer = 0;
            }
            else
            {
                Debug.Log("Game Over");
                gameOver = true;
            }
        }
    }

    public void FinishLevel() => Debug.Log("You Win!");
}
