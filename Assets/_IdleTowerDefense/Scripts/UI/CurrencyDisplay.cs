using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CurrencyDisplay : MonoBehaviour
{
    [SerializeField] private float updateInterval = 0.5f;
    [SerializeField] private CurrencyDisplayElement currencyDisplayElementPrefab;
    [SerializeField] private Transform currencyContainer;

    private float timeSinceLastUpdate = 0;
    private Dictionary<CurrencyTypes, CurrencyDisplayElement> CurrencyDisplayElements = new();

    private void Start()
    {
        foreach (var currencyType in GameManager.Instance.Currency.Keys.ToList())
        {
            CurrencyDisplayElement currencyDisplayElement = Instantiate(currencyDisplayElementPrefab, currencyContainer);
            CurrencyDisplayElements.Add(currencyType, currencyDisplayElement);
            currencyDisplayElement.TextObject.text = $"<b>{currencyType.ToString()}</b>: {GameManager.Instance.Currency[currencyType].ToString()}";
        }
    }

    private void Update()
    {
        timeSinceLastUpdate += Time.deltaTime;
        if (!(timeSinceLastUpdate > updateInterval)) return;

        foreach (KeyValuePair<CurrencyTypes, CurrencyDisplayElement> entry in CurrencyDisplayElements)
        {
            entry.Value.TextObject.text = $"<b>{entry.Key.ToString()}</b>: {GameManager.Instance.Currency[entry.Key].ToString()}";
        }
    }
}