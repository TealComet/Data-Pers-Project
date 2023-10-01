using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Load the new data
        StartManager.Instance.LoadBestPlayerAndBestScore();
    }
}
