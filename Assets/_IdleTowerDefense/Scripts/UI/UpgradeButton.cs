using DuloGames.UI;
using Michsky.UI.Shift;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Modified from Michsky's ChapterButton
/// </summary>
public class UpgradeButton : MonoBehaviour
{
    [Header("Resources")]
    public Sprite backgroundImage;
    public Button Button;
    public UITooltipShow Tooltip;
    public UIElementSound ElementSound;
    public Image backgroundImageObj;
    public TextMeshProUGUI titleObj;
    public TextMeshProUGUI descriptionObj;
    public Transform statusNone;
    public Transform statusLocked;
    public Transform statusCompleted;

    public string buttonTitle = "My Title";
    [TextArea] public string buttonDescription = "My Description";

    [Header("Settings")]
    public bool useCustomResources = false;

    [Header("Status")]
    public bool enableStatus;

    public StatusItem statusItem;

    

    public enum StatusItem
    {
        None,
        Locked,
        Completed
    }

    void Awake()
    {
        if (useCustomResources == false)
        {
            backgroundImageObj = gameObject.transform.Find("Content/Background")
                .GetComponent<Image>();
            titleObj = gameObject.transform.Find("Content/Texts/Title")
                .GetComponent<TextMeshProUGUI>();
            descriptionObj = gameObject.transform.Find("Content/Texts/Description")
                .GetComponent<TextMeshProUGUI>();

            backgroundImageObj.sprite = backgroundImage;
            titleObj.text = buttonTitle;
            descriptionObj.text = buttonDescription;
        }

        if (enableStatus == true)
        {
            statusNone = gameObject.transform.Find("Content/Texts/Status/None")
                .GetComponent<Transform>();
            statusLocked = gameObject.transform.Find("Content/Texts/Status/Locked")
                .GetComponent<Transform>();
            statusCompleted = gameObject.transform.Find("Content/Texts/Status/Completed")
                .GetComponent<Transform>();

            if (statusItem == StatusItem.None)
            {
                statusNone.gameObject.SetActive(true);
                statusLocked.gameObject.SetActive(false);
                statusCompleted.gameObject.SetActive(false);
            }

            else if (statusItem == StatusItem.Locked)
            {
                statusNone.gameObject.SetActive(false);
                statusLocked.gameObject.SetActive(true);
                statusCompleted.gameObject.SetActive(false);
            }

            else if (statusItem == StatusItem.Completed)
            {
                statusNone.gameObject.SetActive(false);
                statusLocked.gameObject.SetActive(false);
                statusCompleted.gameObject.SetActive(true);
            }
        }
    }
}