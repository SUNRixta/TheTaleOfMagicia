using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;

public class VoiceControl : MonoBehaviour
{
    private KeywordRecognizer keywordRecog;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();

    private float countTime;
    private bool counterActive;

    void Start()
    {
        actions.Add("supra", Up); 
        actions.Add("infra", Down);    
        actions.Add("left latus", Left);    
        actions.Add("right latus", Right);

        keywordRecog = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecog.OnPhraseRecognized += RecognisedSpeech;
    }

    private void RecognisedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }

    private void Up()
    {
        Debug.Log("Recog Stopped");
        transform.Translate(0, 1, 0);
        keywordRecog.Stop();
    }
    private void Down()
    {
        Debug.Log("Recog Stopped");
        transform.Translate(0, -1, 0);
        keywordRecog.Stop();
    }
    private void Left()
    {
        Debug.Log("Recog Stopped");
        transform.Translate(-1, 0, 0);
        keywordRecog.Stop();
    }
    private void Right()
    {
        Debug.Log("Recog Stopped");
        transform.Translate(1, 0, 0);
        keywordRecog.Stop();
    }

    private void startCount()
    {
        counterActive = true;
    }
    private void stopCount()
    {
        countTime = 0;
        counterActive = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            startCount();
            Debug.Log("Recog Start");
            keywordRecog.Start();
        }
        if (counterActive == true)
        {
            countTime += Time.deltaTime;
        }
        if (countTime > 10)
        {
            Debug.Log("Recog Stopped");
            stopCount();
            keywordRecog.Stop();
        }
    }
}

