using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckInstance : MonoBehaviour
{
    List<string> deckInfo = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SelectButton()
    {
        GetComponent<Outline>().enabled = true;
    }

    public void SetDeckInfo(List<string> deckInfo)
    {
        this.deckInfo = deckInfo;
    }

    public List<string> GetDeckInfo()
    {
        return this.deckInfo;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
