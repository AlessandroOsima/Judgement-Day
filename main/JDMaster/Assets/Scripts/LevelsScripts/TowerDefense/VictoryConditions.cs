using UnityEngine;
using System.Collections;

public class VictoryConditions : MonoBehaviour
{

    string nextLevel = "TowerDefense";
    bool startCounting = false;
    float timer = 0;
    // Use this for initialization
    void Start()
    {
        GlobalManager.globalManager.onEndGame += endGame;
    }

    void endGame(EndGameState endGameState)
    {
        if (endGameState == EndGameState.Defeat)
        {
            Debug.Log(endGameState);
            LevelGUI.levelGUI.writeMessage("DEFEAT ", new Vector3(Screen.width / 2, Screen.height / 2, 0f), new Vector3(1.4f, 1.4f, 1), 0f, 0);
        }
        else
        {
            Debug.Log(endGameState);
            LevelGUI.levelGUI.writeMessage("VICTORY", new Vector3(Screen.width / 2, Screen.height / 2, 0f), new Vector3(1.4f, 1.4f, 1), 0f, 0);
        }

        startCounting = true;

        if (Application.loadedLevelName == "TowerDefense")
            nextLevel = "SplashScreen";
    }

    // Update is called once per frame
    void Update()
    {
        if (startCounting)
        {
            timer += Time.deltaTime;
            if (timer >= 10f)
            {
                Application.LoadLevel(nextLevel);
            }
        }
    }
}
