using UnityEngine;
using UnityEngine.UI;

public class SavesUI : MonoBehaviour
{
    [Header("Buttons")] 
    public Button goBackBtn;
    
    private void Start()
    {
        goBackBtn.onClick.AddListener(() => UIManager.Instance.GoBack());
    }
}
