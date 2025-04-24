using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SavesUI : MonoBehaviour
{
    public static SavesUI Instance {get; private set;}
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
    [Header("Buttons")] 
    public Button goBackBtn;
    public Button step1PanelBtn;
    public Button step2PanelBtn;
    public Button createSaveBtn;
    public Button playBtn;
    public Button deleteBtn;
    public Button setDefaultBtn;
    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject savesPanel;
    public GameObject leftPanel;
    public GameObject rightPanel;
    public GameObject step1Panel;
    public GameObject step2Panel;
    public GameObject difficultyPanel;
    [Header("Save Prefab stuff")]
    public GameObject savePrefab;
    public GameObject instantiatedSavePrefab;
    public TMP_Text saveText;
    public GameObject contentPanel;
    [Header("Difficulty Information Prefabs")]
    public GameObject easyDifficultyPrefab;
    public GameObject mediumDifficultyPrefab;
    public GameObject hardDifficultyPrefab;
    public GameObject expertDifficultyPrefab;
    public GameObject instantiatedDifficultyPrefab;
    [Header("Saves Panel")]
    public TMP_Text playBtnText;
    public TMP_Text deleteBtnText;
    public TMP_Text setDefaultBtnText;
    [Header("Warnings")]
    public TMP_Text illegalCharactersWarningText;
    public TMP_Text illegalSaveNameWarningText;
    [Header("Other")]
    public TMP_Dropdown difficultyDropdown;
    public TMP_Text  difficultyText;
    public TMP_Text difficultyTextStep2;
    public TMP_InputField saveNameInput;
    public TMP_Text selectedNameText;
    public TMP_Text defaultSaveText;
    
    private string selectedDifficulty;
    private string inputtedSaveName;
    public string defaultSVFName; // Only a thing so that SettingsData.FetchSettingsData() can be called like every modification to settings
    
    private void Start()
    {
        goBackBtn.onClick.AddListener(() => UIManager.Instance.GoBack());
        step1PanelBtn.onClick.AddListener(() => OpenStepPanel(1));
        step2PanelBtn.onClick.AddListener(() => OpenStepPanel(2));
        difficultyDropdown.onValueChanged.AddListener(diff =>
        {
            SelectDifficulty(diff);
            DisplayCreateSaveBtnCheck();
        });
        saveNameInput.onEndEdit.AddListener(str=>
        {
            SelectSaveName(str);
            DisplayCreateSaveBtnCheck();
        }); 
        createSaveBtn.onClick.AddListener(() => GameManager.Instance.CreateSave(inputtedSaveName));
        deleteBtn.onClick.AddListener(() =>
        {
            if (GameManager.Instance.activeSaveName == "") return;
            SaveSystem.DeleteSave(GameManager.Instance.activeSaveName);
            
            ChangeModifierBtnsState();
            InitializeSaveButtons();
            
            if (GameManager.Instance.activeSaveName == defaultSVFName)
            {
                defaultSVFName = "";
                UpdateDefaultSaveStuff();
                GameManager.Instance.SaveSettings(); // Deleting/changing the default save with NOT save without this
            }
        });
        playBtn.onClick.AddListener(() => GameManager.Instance.LoadSave(GameManager.Instance.activeSaveName));
        setDefaultBtn.onClick.AddListener(() =>
        {
            defaultSVFName = GameManager.Instance.activeSaveName;
            UpdateDefaultSaveStuff();
            GameManager.Instance.SaveSettings();
        });
        InitializeSaveButtons();
    }
    private void ChangeModifierBtnsState()
    {
        playBtn.gameObject.SetActive(!playBtn.gameObject.activeSelf);
        deleteBtn.gameObject.SetActive(!deleteBtn.gameObject.activeSelf);
        setDefaultBtn.gameObject.SetActive(!setDefaultBtn.gameObject.activeSelf);
    }
    private void UpdateDefaultSaveStuff()
    {
        defaultSaveText.text = $"Default save: {defaultSVFName}";
        StartUI.Instance.playBtnText.text = defaultSVFName == "" ? "Play" : $"Play '{defaultSVFName}'";
    }
    private void DisplayCreateSaveBtnCheck()
    {
        if (difficultyDropdown.value != 0)
        {
            // Return statement needed because the warning will show up every time
            // after the user changes the difficulty otherwise
            if (saveNameInput.text == "") return;
            
            if (GameManager.Instance.IsSaveNameValid(saveNameInput.text))
            {
                createSaveBtn.interactable = true;
                illegalCharactersWarningText.gameObject.SetActive(false);
            }
            else
            {
                illegalCharactersWarningText.gameObject.SetActive(true);
            }
        }
        else
        {
            createSaveBtn.interactable = false;
        }
    }
    private void InitializeSaveButtons()
    {
        foreach (Transform save in contentPanel.transform)
        {
            Destroy(save.gameObject);
        }
        List<string> saves = SaveSystem.FindSaves();
        if (saves == null) return;

        foreach (string save in saves)
        {
            CreateSaveButton(save);
        }
    }
    public void CreateSaveButton(string saveName)
    {
        instantiatedSavePrefab = Instantiate(savePrefab, contentPanel.transform);
        instantiatedSavePrefab.GetComponentInChildren<TMP_Text>().text = saveName;
        instantiatedSavePrefab.GetComponentInChildren<Button>().onClick.AddListener(() =>
        {
            GameManager.Instance.activeSaveName = saveName;

            if(!playBtn.gameObject.activeSelf) ChangeModifierBtnsState();
            
            playBtnText.text = $"Play '{saveName}'";
            deleteBtnText.text = $"Delete '{saveName}'";
            setDefaultBtnText.text = $"Set  '{saveName}' as default save";
        });
    }
    private void SelectSaveName(string selectedName)
    {
        inputtedSaveName = selectedName;
        selectedNameText.text = $"Save Name: {inputtedSaveName}";
    }
    private void SelectDifficulty(int inputtedDifficulty)
    {
        switch (inputtedDifficulty)
        {
            case 1:
                selectedDifficulty = "Easy";
                Destroy(instantiatedDifficultyPrefab);
                instantiatedDifficultyPrefab = Instantiate(easyDifficultyPrefab, difficultyPanel.transform);
                break;
            case 2:
                selectedDifficulty = "Medium";
                Destroy(instantiatedDifficultyPrefab);
                instantiatedDifficultyPrefab = Instantiate(mediumDifficultyPrefab, difficultyPanel.transform);
                break;
            case 3:
                selectedDifficulty = "Hard";
                Destroy(instantiatedDifficultyPrefab);
                instantiatedDifficultyPrefab = Instantiate(hardDifficultyPrefab, difficultyPanel.transform);
                break;
            case 4:
                selectedDifficulty = "Expert";
                Destroy(instantiatedDifficultyPrefab);
                instantiatedDifficultyPrefab = Instantiate(expertDifficultyPrefab, difficultyPanel.transform);
                break;
            default:
                selectedDifficulty = "Unknown";
                Debug.LogWarning("Unknown difficulty!");
                Destroy(instantiatedDifficultyPrefab);
                break;
        }
        difficultyText.text = $"Selected difficulty: {selectedDifficulty}";
        difficultyTextStep2.text = $"Selected difficulty: {selectedDifficulty}";
    }
    private void OpenStepPanel(int step)
    {
        GameObject panelToOpen;
        GameObject panelToClose;
        if (step == 1)
        {
            panelToOpen = step1Panel;
            panelToClose = step2Panel;
        }
        else
        {
            panelToOpen = step2Panel;
            panelToClose = step1Panel;
        }
        panelToOpen.SetActive(true);
        panelToClose.SetActive(false);
    }
}
