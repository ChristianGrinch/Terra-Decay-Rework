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
    [Header("Other")]
    public TMP_Dropdown difficultyDropdown;
    public TMP_Text  difficultyText;
    public TMP_Text difficultyTextStep2;
    public TMP_InputField saveNameInput;
    public TMP_Text selectedNameText;
    public TMP_Text playBtnText;
    public TMP_Text illegalCharactersWarningText;
    
    public string difficulty;
    public string saveName;
    
    private void Start()
    {
        goBackBtn.onClick.AddListener(() => UIManager.Instance.GoBack());
        step1PanelBtn.onClick.AddListener(() => OpenStepPanel(1));
        step2PanelBtn.onClick.AddListener(() => OpenStepPanel(2));
        difficultyDropdown.onValueChanged.AddListener(SelectDifficulty);
        saveNameInput.onSubmit.AddListener(SelectSaveName);
        createSaveBtn.onClick.AddListener(() => GameManager.Instance.CreateSave(saveName));
        InitializeSaveButtons();
    }

    public void InitializeSaveButtons()
    {
        foreach (var save in SaveSystem.FindSaves())
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
            playBtnText.text = $"Play: {name}";
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
