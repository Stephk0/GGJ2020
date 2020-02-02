using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScreen : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        DifficultyController.collectedMaterials = 0;
        Debug.Log("LoadScene -- Difficulty " + DifficultyController.difficulty);
        SceneManager.LoadScene("Game");
    }
}
