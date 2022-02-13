using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardCollectionInstance : MonoBehaviour
{
    

    public void ClickCard()
    {
        GameObject deckBuilderManager = GameObject.Find("DeckBuilderManager");

        Sprite image = GetComponent<Image>().sprite;
        string name = GetComponentInChildren<TextMeshProUGUI>().text;

        deckBuilderManager.GetComponent<DeckBuilderManager>().AddEditCard(name,image);
        
    }
}
