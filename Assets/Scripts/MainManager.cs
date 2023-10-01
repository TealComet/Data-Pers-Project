using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainManager : MonoBehaviour
{
    public Brick brickPrefab;
    public int lineCount = 6;
    public Rigidbody ball;

    public TextMeshProUGUI bestScoreText;

    public TextMeshProUGUI scoreText;

    private int bestScore;

    private int score;

    private TextMeshProUGUI mainPlayerNameText;

    public GameObject gameOverText;

    public GameObject pausePanel;
    
    private bool isGameStarted = false;
    
    private bool isGameOver = false;
    
    // Start is called before the first frame update
    void Start()
    {
        // Set Time.timeScale to 1
        Time.timeScale = 1;

        // Generate bricks
        BrickGeneration(); 

        // Initialize score
        score = 0;

        // Name Passing
        mainPlayerNameText = GameObject.Find("Main Player Text").GetComponent<TextMeshProUGUI>();
        mainPlayerNameText = StartManager.Instance.startPlayerNameText;

        Debug.Log($"Main Player Name = {mainPlayerNameText.text}.");
    }

    private void BrickGeneration()
    {
        const float step = 0.7f;
        int perLine = Mathf.FloorToInt(4 / step);
        
        int[] pointCountArray = new [] {1, 1, 2, 2, 3, 3};
        for (int i = 0; i < lineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(brickPrefab, position, Quaternion.identity);

                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(UpdateScore);
            }
        }
    }

    void UpdateScore(int point)
    {
        // Add points to score
        score += point;

        // Change scoreText to reflect current score value
        scoreText.text = $"Score : {score}";
    }

    private void Update()
    {
        GameState();
        Pause();
    }

    private void GameState()
    {
        if(!isGameStarted)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                isGameStarted = true;

                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0).normalized;

                ball.transform.SetParent(null);
                ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }

        else if(isGameOver)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                GoToStartScene();
            }
        }
    }

    private void Pause()
    {
        // If P is pressed and the game is not over and Time.timeScale != 0, show pausePanel and set Time.timeScale to 0
        if(Input.GetKeyDown(KeyCode.P) && !isGameOver && Time.timeScale != 0)
        {
            // Show Pause Panel
            pausePanel.SetActive(true);

            // Set Time.timeScale to 0
            Time.timeScale = 0;
        }

        // Else if P is pressed and the game is not over and Time.timeScale == 0, hide pausePanel and set Time.timeScale to 1
        else if(Input.GetKeyDown(KeyCode.P) && !isGameOver && Time.timeScale == 0)
        {
            // Hide Pause Panel
            pausePanel.SetActive(false);

            // Set Time.timeScale to 1
            Time.timeScale = 1;
        }
    }

    public void GameOver()
    {
        // Switch Game State
        isGameOver = true;

        // Display Game Over Text
        gameOverText.SetActive(true);

        // Check if bestScore is surpassed
        BestScore();

        // Set Time.timeScale to 0
        Time.timeScale = 0;
    }

    private void BestScore()
    {
        // Update Best Score Value And text
        if(score > StartManager.Instance.bestScoreValue)
        {
            // Assign to bestScore the new high score
            bestScore = score;

            // Display the best player's name and score
            bestScoreText.text = $"Best Score - {mainPlayerNameText.text} : {bestScore}"; 

            // Update StartManager's Best Player Data
            StartManager.Instance.bestPlayerName = mainPlayerNameText.text;
            Debug.Log($"Best Player = {StartManager.Instance.bestPlayerName}.");

            // Update StartManager's Best Score Data
            StartManager.Instance.bestScoreValue = bestScore;
            Debug.Log($"Best Score = {StartManager.Instance.bestScoreValue}.");

            // Save the new data
            StartManager.Instance.SaveBestPlayerAndBestScore();
        }
    }

    // Back To Start Menu Button Method
    public void GoToStartScene()
    {
        // Go back to the Start Scene
        SceneManager.LoadScene("Start Menu");
    }
}
