using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SKCell;
public sealed class Goal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FlowManager.instance.OnReachGoal(collision.GetComponent<PlayerModel>());
            SKAudioManager.instance.PlaySound("goal");
            MapManager.instance.PlayGoalPuffEffect(transform.position);
            
            collision.GetComponent<PlayerModel>().OnReachGoal();    
        }
    }
}
