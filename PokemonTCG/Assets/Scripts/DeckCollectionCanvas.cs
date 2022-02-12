using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeckCollectionCanvas : MonoBehaviour
{
    GraphicRaycaster m_RayCaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    // Start is called before the first frame update
    void Start()
    {
        m_RayCaster = GetComponent<GraphicRaycaster>();
        m_EventSystem = GetComponent<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_PointerEventData = new PointerEventData(m_EventSystem);
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            m_RayCaster.Raycast(m_PointerEventData, results);


            bool hitDeck = false;
            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.tag.Equals("DeckIcon"))
                {
                    hitDeck = true;
                    ClearDeckIcons();
                    result.gameObject.GetComponent<DeckInstance>().SelectButton();
                    break;
                }

                if (result.gameObject.GetComponent<Button>() != null) {
                    
                    hitDeck = true;break; 
                }
                //Debug.Log("Hit " + result.gameObject.name);
            }
            if (hitDeck == false) ClearDeckIcons();
        }
    }

    public void ClearDeckIcons()
    {
        GameObject[] deckIcons = GameObject.FindGameObjectsWithTag("DeckIcon");
        foreach (GameObject go in deckIcons) go.GetComponent<Outline>().enabled = false;
    }
}
