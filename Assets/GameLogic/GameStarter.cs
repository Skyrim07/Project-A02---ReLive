using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using SKCell;
public sealed class GameStarter : MonoBehaviour
{
    [SerializeField] Animator creditAnim;
    [SerializeField] RawImage cutsceneRI;
    [SerializeField] GameObject effects;
    private void Start()
    {
        SKAudioManager.instance.PlayIdentifiableSound("main", "1", true, 1);
    }
    public void StartGame()
    {
        SKAudioManager.instance.PlaySound("click");
        PlayCutscene();
    }

    public void QuitGame()
    {
        SKAudioManager.instance.PlaySound("click");
        Application.Quit();
    }

    public void SetCreditPanel(bool on)
    {
        SKAudioManager.instance.PlaySound("click");
        creditAnim.SetBool("Appear", on);
    }
    public void PlayCutscene()
    {
        effects.SetActive(false);
        SKAudioManager.instance.StopIdentifiableSound("1", 2f);
        cutsceneRI.gameObject.SetActive(true);
        VideoClip clip = CommonReference.instance.cutsceneClip;
        SKVideoManager.PlayVideoInUI("cg", cutsceneRI);
        //CommonUtils.InvokeAction((float)clip.length + 0f, () =>
        //{
        //    UnityEngine.SceneManagement.SceneManager.LoadScene("Level00");
        //});
        CommonUtils.InvokeAction(0.2f, () =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Level00");
        });
    }
}
