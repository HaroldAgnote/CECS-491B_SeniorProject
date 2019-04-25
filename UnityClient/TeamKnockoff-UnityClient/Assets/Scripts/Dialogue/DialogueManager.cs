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

    public void LoadDialogue(string text) {
        sentences = new List<Dialogue>();
        this.gameObject.SetActive(true);

        HasDialogue = true;

        gameView.topPanel.SetActive(false);
        gameView.bottomPanel.SetActive(false);
        gameView.gameOverScreen.gameObject.SetActive(false);
        gameView.tileSelector.gameObject.SetActive(false);
        gameView.mCamera.LockMoveCamera();
        gameView.mCamera.LockZoomCamera();

        string line;
        using (StringReader reader = new StringReader(text.ToString())) {
            while ((line = reader.ReadLine()) != null) {
                string[] parts = line.Split(':');
                sentences.Add(new Dialogue(parts[0], parts[1]));
            }
        }

        index = 0;
        //InvokeRepeating("DisplaySentence", 3, 3);//2.0f, 2.0f);
        Debug.Log("HELLO");

        skipButton.onClick.AddListener(SkipDialogue);
        continueButton.onClick.AddListener(NextSentence);
        prevButton.onClick.AddListener(PrevSentence);
        autoTextButton.onClick.AddListener(AutoText);

        StartDialogue();
    }

    void Update() {
        //if (dialogueText.text == sentences[index].sentence) {
        //    continueButton.SetActive(true);
        //}
        if((Time.time > lastTime) && isAutoText) {
            lastTime = Time.time + 3;
            // Debug.Log("I'm in time");
            NextSentence();
        }

    }

    public void StartDialogue() {  
        StopAllCoroutines();
        StartCoroutine(DisplaySentence());
    }

    public void PrevSentence() {
        if(index > 0) {
            index--;
            StopAllCoroutines();
            StartCoroutine(DisplaySentence());
        }
    }
    public void NextSentence() {
        //continueButton.SetActive(false);
        if (index == sentences.Count - 1) {
            // Debug.Log("about to return");
            // CampaignManager.instance.LoadNextCampaignEvent();
            HasDialogue = false;
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

    public void AutoText() {
        Debug.Log("AutoText clicked");
        isAutoText = true;
    }

    IEnumerator DisplaySentence() {
        dialogueText.text = "";
        Debug.Log(sentences[index].sentence);
        nameText.text = sentences[index].name;//dialogue.name;
        foreach (char letter in sentences[index].sentence) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
    
    public void SkipDialogue() {
        index = sentences.Count - 1;
        NextSentence();
    }
}
