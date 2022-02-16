using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ConvStarter : MonoBehaviour
{
    public int convID = 0;

    private bool active = true;

    [SerializeField] GameObject emoji;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!active)
            return;
        if (collision.CompareTag("Player"))
        {
            UIManager.instance.StartConversation(convID);
            active = false;
            if(emoji)
                emoji.SetActive(false);
        }
    }
}
