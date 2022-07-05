using mastermind;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public int lives;

    public Transform spawnPoint;
    public PlayerController player;

    public float timeToRespawn;
    public float timer;

    public bool gameOver;
    public bool levelFinished;

    public MenuController menuController;
    
    [Header("UI")]
    public Text lifeText;
    public GameObject levelEndPanel;
    public Text levelEndText;

    private InputAction _start;

    private void Awake()
    {
        _start = player.playerInput.actions[ActionName.Start.ToString()];
    }

    private void Start()
    {
        levelEndPanel.SetActive(false);
        player.transform.position = spawnPoint.position;
    }

    private void Update()
    {
        if (gameOver || levelFinished)
        {
            FinishLevel();
            if (_start.WasPressedThisFrame() || Keyboard.current.escapeKey.wasPressedThisFrame)
                menuController.LoadScene(0);
            return;
        }
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

        lifeText.text = $"x{lives}";
    }

    public void FinishLevel()
    {
        levelEndPanel.SetActive(true);
        if (gameOver)
            levelEndText.text = "GAME OVER";
        if (levelFinished) 
            levelEndText.text = "LEVEL FINISHED";
        
        levelFinished = true;
        var velocity = player.rb.velocity;
        velocity.x = 0;
        player.rb.velocity = velocity;
    }
}
