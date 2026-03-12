using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BlessingUI : MonoBehaviour
{
    [Header("Core Systems")]
    [SerializeField] private RewardSystem rewardSystem;

    [Header("Stat Card")]
    [SerializeField] private Button statButton;
    [SerializeField] private TextMeshProUGUI statTitleText;
    [SerializeField] private TextMeshProUGUI statDescriptionText;

    [Header("Weapon Card")]
    [SerializeField] private Button weaponButton;
    [SerializeField] private TextMeshProUGUI weaponTitleText;
    [SerializeField] private TextMeshProUGUI weaponDescriptionText;

    [Header("Ammo Card")]
    [SerializeField] private Button ammoButton;
    [SerializeField] private TextMeshProUGUI ammoTitleText;
    [SerializeField] private TextMeshProUGUI ammoDescriptionText;

    private void Start()
    {
        // Tell the buttons to trigger the rewards in the RewardSystem when clicked
        statButton.onClick.AddListener(() => rewardSystem.SelectStatReward());
        weaponButton.onClick.AddListener(() => rewardSystem.SelectWeaponReward());
        ammoButton.onClick.AddListener(() => rewardSystem.SelectAmmoReward());
        gameObject.SetActive(false);
    }

    // RewardSystem will call this to populate the cards
    public void UpdateCardUI(
        string sTitle, string sDesc, 
        string wTitle, string wDesc, 
        string aTitle, string aDesc)
    {
        
        statTitleText.text = sTitle;
        statDescriptionText.text = sDesc;

        weaponTitleText.text = wTitle;
        weaponDescriptionText.text = wDesc;

        ammoTitleText.text = aTitle;
        ammoDescriptionText.text = aDesc;
    }
}