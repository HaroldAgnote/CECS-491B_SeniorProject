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
    public static DialogueManager instance;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    private List<Dialogue> sentences;
    private int index;
    public float typingSpeed;
    public GameObject autoTextButton;
    public GameObject continueButton;
    public GameObject prevButton;

    private bool isAutoText = false;
    private float lastTime;

    private string path;

    private void Awake() {
        //Check if instance already exists
        if (instance == null) {
            //if not, set instance to this
            instance = this;
        }

        //If instance already exists and it's not this:
        else if (instance != this) {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start() {
        sentences = new List<Dialogue>();

        string text = SceneLoader.GetParam(SceneLoader.LOAD_DIALOGUE_PARAM);
        
        Debug.Log(text);
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
        StartDialogue();
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


        //while ((line = reader.ReadLine()) != null) {
        //    string[] parts = line.Split(':');
        //    sentences.Add(new Dialogue(parts[0], parts[1]));
        //}

    }
}
