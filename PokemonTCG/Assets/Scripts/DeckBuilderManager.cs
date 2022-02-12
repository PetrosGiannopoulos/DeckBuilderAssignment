using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeckBuilderManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        GameObject deckCollectionObjects = GameObject.Find("DeckCollectionObjects");

        if (deckCollectionObjects) SceneManager.UnloadSceneAsync("DeckCollectionScene");
        //Destroy(deckCollectionObjects);

        //Debug.Log($"PersistentPath: {Application.persistentDataPath}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
