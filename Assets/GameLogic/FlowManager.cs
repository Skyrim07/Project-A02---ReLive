using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SKCell;
public sealed class FlowManager : MonoSingleton<FlowManager>
{
    public int maxLevelCount = 3;

    public int currentLevel = -1;
    public int currentGoalCount = 0;
    public int currentMoveCountL, currentMoveCountR;

    private void Start()
    {
        LoadStartScene();
        //InitializeLevel(currentLevel);
        SKAudioManager.instance.PlayIdentifiableSound("BGM","bgm",true,1);
    }
    public void InitializeLevel(int level)
    {
        currentLevel = level;
        if (currentLevel == -1)
        {
            
        }
        else
        {
            currentGoalCount = 0;
            currentMoveCountL = currentMoveCountR = MapData.levelMoveLimit[level];
            CommonReference.instance.Initialize();
            UIManager.instance.Initialize(level);
            MapManager.instance.LoadMap(level);
        }
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
        UIManager.instance.SetCover(true);
        CommonUtils.InvokeAction(0.3f, () =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Level" + level.ToString("d2"));
            CommonUtils.InvokeAction(0.2f, () =>
            {
                UIManager.instance.SetCover(false);
                InitializeLevel(level);
            });
        });
    }

    public void LoadStartScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("NewStart");
    }
    public void CompleteLevel()
    {
        UIManager.instance.SetCover(true);
        if (currentLevel < maxLevelCount-1)
            CommonUtils.InvokeAction(1f, () => {
                LoadLevel(++currentLevel);
            });
        else if (currentLevel==maxLevelCount-1)
        {
            Application.Quit();
        }
        //if (currentLevel == 1)
        //    CommonUtils.InvokeAction(1f, () => {
        //        UIManager.instance.PlayCutscene();
        //    });
    }


    private void Update()
    {
        if (currentLevel >= 0)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                LoadStartScene();
            }
        }
    }
}
