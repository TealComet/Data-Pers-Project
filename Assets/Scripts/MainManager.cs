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

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestScoreText;
    public GameObject gameOverText;

    public GameObject pausePanel;
    
    private bool isGameStarted = false;
    private int m_Points;
    
    private bool isGameOver = false;
    
    // Start is called before the first frame update
    void Start()
    {
       BrickGeneration(); 
    }

    private void BrickGeneration()
    {
        const float step = 0.8f;
        int perLine = Mathf.FloorToInt(4 / step);
        
        int[] pointCountArray = new [] {1, 1, 2, 2, 3, 3};
        for (int i = 0; i < lineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(brickPrefab, position, Quaternion.identity);

                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        scoreText.text = $"Score : {m_Points}";
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
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void Pause()
    {
        // If P is pressed and Time.timeScale != 0, show pausePanel and set Time.timeScale to 0
        if(Input.GetKeyDown(KeyCode.P) && Time.timeScale != 0)
        {
            // Show Pause Panel
            pausePanel.SetActive(true);

            // Set Time.timeScale to 0
            Time.timeScale = 0;
        }

        // Else if P is pressed and Time.timeScale == 0, hide pausePanel and set Time.timeScale to 1
        else if(Input.GetKeyDown(KeyCode.P) && Time.timeScale == 0)
        {
            // Hide Pause Panel
            pausePanel.SetActive(false);

            // Set Time.timeScale to 1
            Time.timeScale = 1;
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOverText.SetActive(true);
    }
}
