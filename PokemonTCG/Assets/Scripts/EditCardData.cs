using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EditCardData : MonoBehaviour
{


    private string editNameData="";
    private int editCountData=0;
    private Sprite editImageData;

    public GameObject editCardNumber;
    public GameObject editCardNameButton;
    public GameObject editCardImage;
    
    public void SetName(string name)
    {
        this.editNameData = name;
    }

    public void SetCount(int count)
    {
        this.editCountData = count;
    }

    public void SetImage(Sprite image)
    {
        this.editImageData = image;
    }

    public string GetName()
    {
        return this.editNameData;
    }

    public int GetCount()
    {
        return this.editCountData;
    }

    public Sprite GetImage()
    {
        return this.editImageData;
    }

    public void FitData()
    {
        editCardNumber.GetComponentInChildren<TextMeshProUGUI>().text = ""+editCountData;
        editCardNameButton.GetComponentInChildren<TextMeshProUGUI>().text = editNameData;
        editCardImage.GetComponentInChildren<Image>().sprite = editImageData;
    }

    public void RemoveCard()
    {
        GameObject deckBuilderManager = GameObject.Find("DeckBuilderManager");
        deckBuilderManager.GetComponent<DeckBuilderManager>().RemoveEditCard(gameObject);
    }
}
