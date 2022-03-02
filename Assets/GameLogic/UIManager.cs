using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

using SKCell;
public sealed class UIManager : MonoSingleton<UIManager>
{
    public bool inUI = false;
    private string[] levelNames = new string[] { "--- Level 1 ---", "--- Level 2 ---", "--- Level 3 ---", "--- Level 4 ---" };

    public Text levelText, convText, moveTextL, moveTextR;
    [SerializeField] Image convBox, tutImage;
    [SerializeField] RawImage cutsceneRI;
    [SerializeField] Animator menuPanel, helpPanel, cover, moveTextCover, convAnim, convTextAnim, startAnim;
    [SerializeField] GameObject levelSelect;

    private Color moveTextColorL, moveTextColorR;
    public Color moveTextColorEmpty;
    private Conversation curConv;
    private int curConvSentenceid;

    public Conversation[] conversations = new Conversation[]
    {
        new Conversation(new List<Sentence>()
        {
            new Sentence(Speaker.Boy, "H-Hey!  I am terribly sorry for asking you this… but I was wondering if you could pose for my photography. You look wonderful and I simply just adore this statue. "),
            new Sentence(Speaker.Girl, "Oh, me? Thank you and for sure. My dad loved this statue too, he said it symbolized a forbidden romance, and I’ve always pitied the lost couple. "),
            new Sentence(Speaker.Boy, "I agree. Well let me get my equipment set up and we’ll be ready to go."),
        }),
        new Conversation(new List<Sentence>()
        {
            new Sentence(Speaker.Boy, "Wait, isn’t that the girl from the museum?"),
            new Sentence(Speaker.Boy, "Hey! What are you doing here? And by the way, I love that book, was looking forward to buying it."),
            new Sentence(Speaker.Girl, "OH HEY!!! How have you been? It’s so weird we ran into each other again in this big city. "),
            new Sentence(Speaker.Girl, "Norwegian Wood is one of my favorites too. Although it’s the last one…"),
            new Sentence(Speaker.Boy, "No worries! But…ummm…what about I borrow the book after you finish reading it. Like at dinner or whatever."),
            new Sentence(Speaker.Girl, " Ha! A date it is. "),
        }),
        new Conversation(new List<Sentence>()
        {
            new Sentence(Speaker.Girl, "Never knew you were into such films. I love Audrey Hepburn. She’s my dad’s celebrity crush haha."),
            new Sentence(Speaker.Boy, "You kidding? I would never miss a reshow of Breakfast at Tiffany’s."),
            new Sentence(Speaker.Girl, "You are just full of surprises. Come on, let’s go in before the movie starts."),
        }),
        new Conversation(new List<Sentence>()
        {
            new Sentence(Speaker.Boy, "Let’s take a rest here at this village. I am so out of breath."),
            new Sentence(Speaker.Girl, "Ugh fine…But the peak is close! "),
            new Sentence(Speaker.Boy, "Maybe we can spend the night in this village?"),
            new Sentence(Speaker.Girl, "You know what? Sure, I can use a few hours of sleep."),
        }),
        new Conversation(new List<Sentence>()
        {
            new Sentence(Speaker.Boy, "These adventures have been fun for the past few months right?"),
            new Sentence(Speaker.Girl, "Of course! I never ever had a bucket list until I met you."),
            new Sentence(Speaker.Boy, "Right…So…I guess I was wondering if you wanted to meet again in the art museum when we first met next week? And then we can grab dinner together."),
            new Sentence(Speaker.Girl, "Sounds like a plan!"),
        }),
        new Conversation(new List<Sentence>()
        {
            new Sentence(Speaker.Boy, "Hey are you almost there? I’m right beside the statue of Apollo and Daphne."),
            new Sentence(Speaker.Girl, " Yep yep. I’ll be there in about 5 minutes max? I’ll see you so-......."),
            new Sentence(Speaker.Boy, "Hello? HELLO? HELLO? ARE YOU THERE?"),
            new Sentence(Speaker.Boy, "*Beep beep beep beep*"),
        }),
    };

    protected override void Awake()
    {
        base.Awake();
        moveTextColorL = moveTextL.color;
        moveTextColorR = moveTextR.color;
    }
    public void Initialize(int level)
    {
        inUI = false;

        CommonUtils.SetActiveEfficiently(tutImage.gameObject, level == 0);
        startAnim.Play("StartAnim");
        SetCover(false);
        SetLevelText(level);

        moveTextL.color = moveTextColorL;
        moveTextR.color = moveTextColorR;
        UpdateMoveText();
    }
    public void StartConversation(int id)
    {
        inUI = true;

        curConv = conversations[id];
        curConvSentenceid = 0;
        convText.text = curConv.sentences[curConvSentenceid].content;
        convBox.sprite = CommonReference.instance.convBoxes[(int)curConv.sentences[curConvSentenceid].speaker];

        convAnim.Appear();
    }

    public void NextSentence()
    {
        SKAudioManager.instance.PlaySound("click");
        if (curConvSentenceid == curConv.sentences.Count - 2)
        {
            if (curConv == conversations[0])
            {
                SKAudioManager.instance.PlaySound("cam");
            }
            if (curConv == conversations[5])
            {
                SKAudioManager.instance.PlaySound("phone");
            }
        }

        if (curConvSentenceid == curConv.sentences.Count - 1)
        {
            convAnim.Disappear();
            inUI = false;
            return;
        }
        convTextAnim.Pop();
        curConvSentenceid++;
        convText.text = curConv.sentences[curConvSentenceid].content;
        convBox.sprite = CommonReference.instance.convBoxes[(int)curConv.sentences[curConvSentenceid].speaker];
    }
    public void SetLevelText(int level)
    {
        levelText.text = levelNames[level];
    }

    public void UpdateMoveText()
    {
        moveTextL.text = FlowManager.instance.currentMoveCountL.ToString();
        moveTextR.text = FlowManager.instance.currentMoveCountR.ToString();
    }

    public void SetMenuPanel(bool on)
    {
        SKAudioManager.instance.PlaySound("click");
        menuPanel.SetBool("Appear", on);
        inUI = on;
    }
    public void SetHelpPanel(bool on)
    {
        SKAudioManager.instance.PlaySound("click");
        helpPanel.SetBool("Appear", on);
    }
    public void SetCover(bool on)
    {
        cover.SetBool("Appear", on);
    }
    public void SetMoveTextCover(bool on)
    {
        moveTextCover.SetBool("Appear", on);
    }
    public void ReturnToMenu()
    {
        SKAudioManager.instance.PlaySound("click");
        SetCover(true);
        CommonUtils.InvokeAction(1f, () => { UnityEngine.SceneManagement.SceneManager.LoadScene("start"); });
    }
    public void PlayCutscene()
    {
        SKAudioManager.instance.StopIdentifiableSound("bgm", 2f);
       cutsceneRI.gameObject.SetActive(true);
        VideoClip clip = CommonReference.instance.endClip;
        SKVideoManager.PlayVideoInUI("end", cutsceneRI);
        CommonUtils.InvokeAction((float)clip.length, () =>
        {
            ReturnToMenu();
        });
    }

    public void OnMoveDepleted()
    {
        if (FlowManager.instance.currentMoveCountL <= 0)
        {
            moveTextL.color = moveTextColorEmpty;
            moveTextR.color = Color.white;
        }
        if (FlowManager.instance.currentMoveCountR <= 0)
        {
            moveTextR.color = moveTextColorEmpty;
            moveTextL.color = Color.white;
        }

        inUI = true;
        SetMoveTextCover(true);
        CommonUtils.InvokeAction(1f, () =>
        {
            FlowManager.instance.ReloadLevel();
        });
    }
}

public class Conversation
{
    public List<Sentence> sentences;  
    public Conversation(List<Sentence> sentences)
    {
        this.sentences = sentences;
    }
}
public class Sentence
{
    public Speaker speaker;
    public string content;

    public Sentence(Speaker speaker, string content)
    {
        this.speaker = speaker;
        this.content = content;
    }
}
public enum Speaker
{
    Boy,
    Girl,
    Together
}