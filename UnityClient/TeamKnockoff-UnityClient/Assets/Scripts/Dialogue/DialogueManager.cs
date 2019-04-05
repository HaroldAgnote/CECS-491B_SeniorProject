using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using TMPro;
using Assets.Scripts.Campaign;
using Assets.Scripts.Application;
//using UnityEngine.UI;


public class DialogueManager : MonoBehaviour {
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    private List<Dialogue> sentences;
    private int index;
    public float typingSpeed;
    public GameObject startButton;
    public GameObject autoTextButton;
    public GameObject continueButton;
    public GameObject prevButton;

    private bool isAutoText = false;
    private float lastTime;

    private string path;

    // Start is called before the first frame update
    void Start() {
        sentences = new List<Dialogue>();
        path = "Assets/Scripts/Dialogue/Dialogue1.txt";
        ReadFile(path);
        index = 0;
        //InvokeRepeating("DisplaySentence", 3, 3);//2.0f, 2.0f);
        StartDialogue();
        //Debug.Log(dialogueText.text);
        //Debug.Log(sentences[index]);
    }

    void Update() {
        //if (dialogueText.text == sentences[index].sentence) {
        //    continueButton.SetActive(true);
        //}
        if((Time.time > lastTime) && isAutoText) {
            lastTime = Time.time + 3;
            Debug.Log("I'm in time");
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
            Debug.Log("about to return");
            CampaignManager.instance.LoadNextCampaignEvent();
        }

        if (index < sentences.Count - 1) {
            index++;
            StopAllCoroutines();
            StartCoroutine(DisplaySentence());
        }

    }

    public void AutoText() {

        //while (index < sentences.Count) {
        //    //StopAllCoroutines();
        //    //semaphore.WaitOne();
        //    StartCoroutine(DisplaySentence()); //StartCoroutine(AutoDisplaySentence());
        //    index++;
        //    //semaphore.Release();
        //}
        Debug.Log("AutoText clicked");
        isAutoText = true;
        //InvokeRepeating("DisplaySentence", 3, 3);//2.0f, 2.0f);
    }

    //IEnumerator AutoDisplaySentence() {
    //    dialogueText.text = "";
    //    Debug.Log($"Count of queue in Display is {sentences.Count}");

    //    Debug.Log(sentences[index].sentence);
    //    yield return new WaitForSeconds(2);
    //    nameText.text = sentences[index].name;//dialogue.name;
    //    foreach (char letter in sentences[index].sentence) {
    //        dialogueText.text += letter;
    //        yield return new WaitForSeconds(typingSpeed);
    //    }
    //}

    IEnumerator DisplaySentence() {
        dialogueText.text = "";
        Debug.Log(sentences[index].sentence);
        nameText.text = sentences[index].name;//dialogue.name;
        foreach (char letter in sentences[index].sentence) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }


    public void ReadFile(string path) {
        //StreamReader reader = new StreamReader(path);
        //string line;

        //while ((line = reader.ReadLine()) != null) {
        //    string[] parts = line.Split(':');
        //    sentences.Add(new Dialogue(parts[0], parts[1]));
        //}

        string text = SceneLoader.GetParam(SceneLoader.LOAD_DIALOGUE_PARAM);
        //CampaignManager.instance.LoadNextCampaignEvent();
        Debug.Log(text);
        string line;
        using (StringReader reader = new StringReader(text.ToString())) {
            while ((line = reader.ReadLine()) != null) {
                string[] parts = line.Split(':');
                sentences.Add(new Dialogue(parts[0], parts[1]));
            }
        }

        //while ((line = reader.ReadLine()) != null) {
        //    string[] parts = line.Split(':');
        //    sentences.Add(new Dialogue(parts[0], parts[1]));
        //}

    }
}
