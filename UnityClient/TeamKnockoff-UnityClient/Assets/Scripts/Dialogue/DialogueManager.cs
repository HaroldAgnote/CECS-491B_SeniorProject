using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using TMPro;
using Assets.Scripts.Campaign;
using Assets.Scripts.Application;
using Assets.Scripts.View;
//using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public GameView gameView;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    private List<Dialogue> sentences;
    private int index;
    public float typingSpeed;

    public Button skipButton;
    public Button autoTextButton;
    public Button continueButton;
    public Button prevButton;

    private bool isAutoText = false;
    private float lastTime;

    private string path;

    public bool HasDialogue;

    // Start is called before the first frame update
    void Start() {
        sentences = new List<Dialogue>();
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// loads dialogue from text into Dialogue array
    /// </summary>
    /// <param name="text"> text to be placed into Dialogue array </param>
    public void LoadDialogue(string text) {
        sentences = new List<Dialogue>();
        this.gameObject.SetActive(true);

        dialogueText.text = "";

        HasDialogue = true;
        isAutoText = false;

        gameView.topPanel.SetActive(false);
        gameView.bottomPanel.SetActive(false);
        gameView.gameOverScreen.gameObject.SetActive(false);
        gameView.tileSelector.gameObject.SetActive(false);
        gameView.mCamera.LockMoveCamera();
        gameView.mCamera.LockZoomCamera();

        var reader = new StringReader(text);
        string line = string.Empty;
        do {
            line = reader.ReadLine();
            if (line != null) {
                string[] parts = line.Split(':');
                sentences.Add(new Dialogue(parts[0], parts[1]));
            }

        } while (line != null);
        
        index = 0;
        //InvokeRepeating("DisplaySentence", 3, 3);//2.0f, 2.0f);
        Debug.Log("HELLO");

        skipButton.onClick.AddListener(SkipDialogue);
        continueButton.onClick.AddListener(NextSentence);
        prevButton.onClick.AddListener(PrevSentence);
        autoTextButton.onClick.AddListener(AutoText);

        StartDialogue();
    }

    /// <summary>
    /// called every frame during Dialogue Manager's activity
    /// Right now, it is used to check if AutoText is enabled
    /// </summary>
    void Update() {
        //if (dialogueText.text == sentences[index].sentence) {
        //    continueButton.SetActive(true);
        //}
        if((Time.time > lastTime) && isAutoText && HasDialogue) {
            lastTime = Time.time + 3;
            // Debug.Log("I'm in time");
            NextSentence();
        }

    }

    /// <summary>
    /// starts dialogue
    /// </summary>
    public void StartDialogue() {  
        StopAllCoroutines();
        StartCoroutine(DisplaySentence());
    }

    /// <summary>
    /// goes to the previous sentence of dialogue array
    /// </summary>
    public void PrevSentence() {
        if(index > 0) {
            index--;
            StopAllCoroutines();
            StartCoroutine(DisplaySentence());
        }
    }

    /// <summary>
    /// goes to the next sentence of dialogue array
    /// </summary>
    public void NextSentence() {
        //continueButton.SetActive(false);
        if (index == sentences.Count - 1) {
            // Debug.Log("about to return");
            // CampaignManager.instance.LoadNextCampaignEvent();
            HasDialogue = false;
            isAutoText = false;
            sentences.Clear();
            this.gameObject.SetActive(false);

            gameView.topPanel.SetActive(true);
            gameView.bottomPanel.SetActive(true);
            gameView.tileSelector.gameObject.SetActive(true);
            gameView.mCamera.UnlockMoveCamera();
            gameView.mCamera.UnlockZoomCamera();
        }

        if (index < sentences.Count - 1) {
            index++;
            StopAllCoroutines();
            StartCoroutine(DisplaySentence());
        }

    }

    /// <summary>
    /// enables autoText
    /// TODO: does disable Autotext work?
    /// </summary>
    public void AutoText() {
        Debug.Log("AutoText clicked");
        isAutoText = true;
    }

    /// <summary>
    /// displays sentence in a character by character
    /// needs to return IEnumerator because of asynch in c#
    /// </summary>
    /// <returns></returns>
    IEnumerator DisplaySentence() {
        dialogueText.text = "";
        Debug.Log(sentences[index].sentence);
        nameText.text = sentences[index].name;//dialogue.name;
        foreach (char letter in sentences[index].sentence) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
    
    /// <summary>
    /// skips dialogue scene
    /// </summary>
    public void SkipDialogue() {
        index = sentences.Count - 1;
        NextSentence();
    }
}
