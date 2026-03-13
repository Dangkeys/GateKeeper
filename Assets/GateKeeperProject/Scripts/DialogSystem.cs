using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    [SerializeField] private DialogSO dialogSO;
    [SerializeField] private TMP_Text headerText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image image;

    void Start()
    {
        UpdateText();
    }

    public void PlayNextDialog()
    {
        dialogSO = dialogSO.dialogSO;
        UpdateText();
    }

    private void UpdateText()
    {
        headerText.text = dialogSO.header;
        descriptionText.text = dialogSO.description;
        if(dialogSO.image != null)
        {
            image.gameObject.SetActive(true);
            image.sprite = dialogSO.image;
        }
        else
        {
            image.gameObject.SetActive(false);
        }
    }
}
