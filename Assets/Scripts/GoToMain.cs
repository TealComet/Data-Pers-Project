using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoToMain : MonoBehaviour
{
    private Button startButton;

    // Start is called before the first frame update
    void Start()
    {
        startButton = GetComponent<Button>();
        startButton.onClick.AddListener(() => GoToMainScene());
    }

    private void GoToMainScene()
    {
        SceneManager.LoadScene("Main");
    }
}
