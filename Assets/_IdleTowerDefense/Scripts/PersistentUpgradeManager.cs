using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersistentUpgradeManager : MonoBehaviour
{
   [SerializeField] private Transform buttonContainer;
   Dictionary<int, int> TestUpgrades = new Dictionary<int, int>();
   
   private void Awake()
   {
      
      for (int i = 0; i < buttonContainer.childCount; i++)
      {
         Button currentButton = buttonContainer.GetChild(i).GetComponent<Button>();
         int currentIndex = i;
         
         TestUpgrades.Add(currentIndex,0);
         currentButton.onClick.AddListener(
            () => {
               UpgradeButtonClickTest(currentIndex);
            });
      }
   }

   private void UpgradeButtonClickTest(int index)
   {
      Debug.Log($"{nameof(PersistentUpgradeManager)}.{nameof(UpgradeButtonClickTest)}() - Index: {index}");
      TestUpgrades[index] += 1;
      ES3.Save("Test", TestUpgrades);
   }
   
   public void ClearSaveData()
   {
      ES3.DeleteFile();
   }
}
