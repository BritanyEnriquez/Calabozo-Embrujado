using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class Microphone_move : MonoBehaviour
{

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();
    private int movex;
    private int movey;
    public float speed;

    void Start()
    {
        actions.Add("izquierda", Izquierda);
        actions.Add("derecha", Derecha);
        actions.Add("arriba", Arriba);
        actions.Add("abajo", Abajo);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();

    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }

    private void Izquierda()
    {
        //transform.Translate(-1, 0, 0);
        movex = -1;
        movey = 0;
    }

    private void Derecha()
    {
        // transform.Translate(1, 0, 0);
        movex = 1;
        movey = 0;
    }

    private void Arriba()
    {
        //transform.Translate(0, 1, 0);
        movey = 1;
        movex = 0;
    }

    private void Abajo()
    {
        //  transform.Translate(0, -1, 0);
        movex = 0;
        movey = -1;
    }

    private void Update()
    {
        Vector3 move = new Vector3(movex, movey, 0f);
        transform.Translate(move * Time.deltaTime * speed);
    }

    
}
