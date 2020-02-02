using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayBtn : MonoBehaviour
{

    public void LoadScene(string SceneName)
    {
        DifficultyController.difficulty = 0;
        SceneManager.LoadScene(SceneName);
    }
}
