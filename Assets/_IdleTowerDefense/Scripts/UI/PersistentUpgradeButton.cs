using System.Collections;
using System.Collections.Generic;
using Michsky.UI.Shift;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PersistentUpgradeButton : MonoBehaviour
{
    public Button Button;
    public Image Thumbnail;
    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI DescriptionText;
    public TextMeshProUGUI GameModeText;
    public TextMeshProUGUI UpgradeAmountText;
    public Image CurrencyIcon;
    public TextMeshProUGUI CostText;
    public UIElementSound ElementSound;
    public PersistentUpgradeBase TargetUpgrade;
}
