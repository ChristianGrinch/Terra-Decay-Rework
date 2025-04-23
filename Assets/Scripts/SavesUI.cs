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
    
    public string difficulty;
    public string saveName;
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
        createSaveBtn.onClick.AddListener(() => GameManager.Instance.CreateSave(saveName));
        deleteBtn.onClick.AddListener(() =>
        {
            if (GameManager.Instance.activeSaveName == "") return;
            SaveSystem.DeleteSave(GameManager.Instance.activeSaveName);
            
            playBtn.gameObject.SetActive(false);
            deleteBtn.gameObject.SetActive(false);
            setDefaultBtn.gameObject.SetActive(false);
            
            InitializeSaveButtons();
        });
        playBtn.onClick.AddListener(() => GameManager.Instance.LoadSave(GameManager.Instance.activeSaveName));
        setDefaultBtn.onClick.AddListener(() =>
        {
            defaultSVFName = GameManager.Instance.activeSaveName;
            StartUI.Instance.playBtn.GetComponentInChildren<TMP_Text>().text = $"Play '{defaultSVFName}'";
            GameManager.Instance.SaveSettings();
        });
        InitializeSaveButtons();
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
    public void InitializeSaveButtons()
    {
        foreach (Transform save in contentPanel.transform)
        {
            Destroy(save.gameObject);
        }
        List<string> saves = SaveSystem.FindSaves();
        if (saves == null) return;

        foreach (var save in saves)
        {
            CreateSaveButton(save);
        }
    }

    public void CreateSaveButton(string name)
    {
        instantiatedSavePrefab = Instantiate(savePrefab, contentPanel.transform);
        instantiatedSavePrefab.GetComponentInChildren<TMP_Text>().text = name;
        instantiatedSavePrefab.GetComponentInChildren<Button>().onClick.AddListener(() =>
        {
            GameManager.Instance.activeSaveName = name;
            
            playBtn.gameObject.SetActive(true);
            deleteBtn.gameObject.SetActive(true);
            
            playBtnText.text = $"Play '{name}'";
            deleteBtnText.text = $"Delete '{name}'";
            setDefaultBtnText.text = $"Set  '{name}' as default save";
        });
    }
    private void SelectSaveName(string selectedName)
    {
        saveName = selectedName;
        selectedNameText.text = $"Save Name: {saveName}";
    }
    private void SelectDifficulty(int selectedDifficulty)
    {
        switch (selectedDifficulty)
        {
            case 1:
                difficulty = "Easy";
                Destroy(instantiatedDifficultyPrefab);
                instantiatedDifficultyPrefab = Instantiate(easyDifficultyPrefab, difficultyPanel.transform);
                break;
            case 2:
                difficulty = "Medium";
                Destroy(instantiatedDifficultyPrefab);
                instantiatedDifficultyPrefab = Instantiate(mediumDifficultyPrefab, difficultyPanel.transform);
                break;
            case 3:
                difficulty = "Hard";
                Destroy(instantiatedDifficultyPrefab);
                instantiatedDifficultyPrefab = Instantiate(hardDifficultyPrefab, difficultyPanel.transform);
                break;
            case 4:
                difficulty = "Expert";
                Destroy(instantiatedDifficultyPrefab);
                instantiatedDifficultyPrefab = Instantiate(expertDifficultyPrefab, difficultyPanel.transform);
                break;
            default:
                difficulty = "Unknown";
                Debug.LogWarning("Unknown difficulty!");
                Destroy(instantiatedDifficultyPrefab);
                break;
        }
        difficultyText.text = $"Selected difficulty: {difficulty}";
        difficultyTextStep2.text = $"Selected difficulty: {difficulty}";
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
