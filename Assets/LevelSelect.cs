using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    public void LoadLevel(int level)
    {
        FlowManager.instance.LoadLevel(level);
    }
}
