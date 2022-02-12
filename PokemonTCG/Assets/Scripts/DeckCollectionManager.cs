using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckCollectionManager : MonoBehaviour
{

    private int selectedDeck;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        
        GameObject homeObjects = GameObject.Find("HomeObjects");
        //Gets rid of unecessary scenes in hierarchy to avoid creating additively empty scene gameobjects 
        if(homeObjects) SceneManager.UnloadSceneAsync("HomeScene");
        
    }

    public void CreateNew()
    {

    }

    public void Edit()
    {

    }

    public void Import()
    {
        //Optional
    }

    public void Export()
    {
        //Optional
    }

    public void Delete()
    {

    }

    public void GoHome()
    {
        SceneManager.LoadScene("HomeScene", LoadSceneMode.Additive);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
