using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SKCell;
public sealed class FlowManager : MonoSingleton<FlowManager>
{
    public int currentLevel = 0;
    public int currentGoalCount = 0;
    public int currentMoveCountL, currentMoveCountR;

    private void Start()
    {
        InitializeLevel(currentLevel);
        SKAudioManager.instance.PlayIdentifiableSound("BGM","bgm",true,1);
    }
    public void InitializeLevel(int level)
    {
        currentLevel = level;
        currentGoalCount = 0;
        currentMoveCountL = currentMoveCountR = MapData.levelMoveLimit[level];
        CommonReference.instance.Initialize();
        UIManager.instance.Initialize(level);
        MapManager.instance.LoadMap(level);
    }

    public void OnReachGoal(PlayerModel pm)
    {
        currentGoalCount++;

        if (currentGoalCount == 2)
            CompleteLevel();
    }
    public void ReloadLevel()
    {
        LoadLevel(currentLevel);
    }

    public void LoadLevel(int level)
    {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Level" + level.ToString("d2"));
            CommonUtils.InvokeAction(0.2f, () =>
            {
                InitializeLevel(level);
            });
    }
    public void CompleteLevel()
    {
        UIManager.instance.SetCover(true);
        if (currentLevel == 0)
            CommonUtils.InvokeAction(1f, () => {
                Application.Quit();
                //LoadLevel(++currentLevel);
            });
        if (currentLevel == 1)
            CommonUtils.InvokeAction(1f, () => {
                UIManager.instance.PlayCutscene();
            });
    }

}
