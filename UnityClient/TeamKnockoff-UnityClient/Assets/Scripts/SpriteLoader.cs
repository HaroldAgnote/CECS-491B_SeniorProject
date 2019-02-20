using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLoader : MonoBehaviour
{
    private Rect buttonPos;
    private int spriteVersion = 0;
    private SpriteRenderer spriteR;
    private Sprite[] sprites;
    public Texture2D texture;

    void Start() {
        buttonPos = new Rect(10.0f, 10.0f, 150.0f, 50.0f);
        spriteR = gameObject.GetComponent<SpriteRenderer>();

        string resourcePath = $"Textures/{texture.name}";

        sprites = Resources.LoadAll<Sprite>(resourcePath);
        Debug.Log($"Number of Sprites: {sprites.Length}");
    }

    void OnGUI() {
        if (GUI.Button(buttonPos, "Choose next sprite")) {
            spriteVersion += 1;
            if (spriteVersion > sprites.Length)
                spriteVersion = 0;
            spriteR.sprite = sprites[spriteVersion];
        }
    }
}
