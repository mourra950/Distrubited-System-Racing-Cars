using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class texttest : MonoBehaviour
{
    public TMP_InputField input;
    List<string> chat = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n"); chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n"); chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n"); chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n"); chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n"); chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n"); chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n"); chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n"); chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n"); chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n"); chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n"); chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n"); chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n");
        chat.Add("ahmed mohamed \n");
        string allchat = string.Empty;
        foreach (var chater in chat)
        {
            allchat += chater;
        }
        input.text = allchat;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
