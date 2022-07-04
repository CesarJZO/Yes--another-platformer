using UnityEditor.U2D.Sprites;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public int coins;
    public int gems;
    public int lives;

    public Transform spawnPoint;
    public PlayerController player;

    public float timeToRespawn;
    public float timer;

    public bool gameOver;
    public bool levelFinished;

    private InputAction _start;

    private void Awake()
    {
        _start = player.playerInput.actions[ActionName.Start.ToString()];
    }

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
                player.Respawn();
                timer = 0;
            }
            else
            {
                gameOver = true;
            }
        }

        if (gameOver || levelFinished)
            if (_start.WasPressedThisFrame())
            {
                // Load main menu
            }
    }

    public void FinishLevel()
    {
        levelFinished = true;
        player.rb.velocity = Vector2.zero;
    }
}
