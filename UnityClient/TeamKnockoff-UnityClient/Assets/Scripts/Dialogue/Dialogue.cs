using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue {

    public string name { get; set; }
    //[TextArea(1,3)]
    //public string[] sentences;
    public string sentence { get; set; }

    public Dialogue(string n, string s) {
        name = n;
        sentence = s ;
    }

}
