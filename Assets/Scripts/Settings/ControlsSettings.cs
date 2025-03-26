using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlsSettings : MonoBehaviour
{
    public static ControlsSettings Instance {get; private set;}
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    [Header("Controls")]
    public Dictionary<Keys, KeyCode> controlKeys;
    public GameObject controlPanelContent;
    public GameObject controlObjectPrefab;
    public TMP_Text keysFunctionText;
    public TMP_Text keyUsedText;

    private void Start()
    {
        InitializeControls();
    }
    public Dictionary<Keys, KeyCode> ReturnDefaultControlKeys()
    {
        return new Dictionary<Keys, KeyCode> {
            {Keys.GoBack, KeyCode.Escape},
        };
    }
    // Thanks to stackoverflow for this extension method!
    // https://stackoverflow.com/questions/5796383/insert-spaces-between-words-on-a-camel-cased-token
    private static string SplitCamelCase(string str) 
    {
        return Regex.Replace( 
            Regex.Replace( 
                str, 
                @"(\P{Ll})(\P{Ll}\p{Ll})", 
                "$1 $2" 
            ), 
            @"(\p{Ll})(\P{Ll})", 
            "$1 $2" 
        );
    }
    private void InitializeControls()
    {
        controlKeys = ReturnDefaultControlKeys();
        foreach (var control in controlKeys)
        {
            string key = SplitCamelCase(control.Key.ToString());
            string keyCode = control.Value.ToString();
            
            GameObject controlPanel = Instantiate(controlObjectPrefab, controlPanelContent.transform);
            keysFunctionText = controlPanel.GetComponentInChildren<TMP_Text>();
            Button button = controlPanel.GetComponentInChildren<Button>();
            keyUsedText = button.gameObject.GetComponentInChildren<TMP_Text>();
            button.onClick.AddListener(() =>
            {
                keyUsedText.text = "Waiting for input...";
                StartCoroutine(WaitForKey(control.Key));
            });
            
            keysFunctionText.text = key;
            keyUsedText.text = keyCode;
        }
    }
    // Completely made with ChatGPT. Thanks!
    private IEnumerator WaitForKey(Keys controlKey)
    {
        bool keyDetected = false;
        while (!keyDetected)
        {
            yield return null;
            foreach (KeyCode keyDown in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyDown))
                {
                    controlKeys[controlKey] = keyDown;
                    keyUsedText.text = keyDown.ToString(); // Update UI
                    keyDetected = true;
                    SettingsUI.Instance.didChangeSetting = true;
                    break;
                }
            }
        }
    }
}