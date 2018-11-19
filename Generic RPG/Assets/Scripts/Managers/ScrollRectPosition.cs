using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ScrollRectPosition : MonoBehaviour
{

    RectTransform scrollRectTransform;
    RectTransform contentPanel;
    RectTransform selectedRectTransform;
    GameObject lastSelected;

    void Start()
    {
        scrollRectTransform = GetComponent<RectTransform>();
        contentPanel = GetComponent<ScrollRect>().content;
    }

    void Update()
    {
        // Get the currently selected UI element from the event system.
        GameObject selected = EventSystem.current.currentSelectedGameObject;

        if (selected == GameObject.Find("Hab_1"))
        {
            GameObject.Find("PanelHabilidades").transform.Find("Scrollbar").gameObject.GetComponent<Scrollbar>().value = 1;
            return;
        }
        GameObject[] PrimerosObjetos = new GameObject[]
        {
            GameObject.Find("Obj_1"),
            GameObject.Find("Obj_2"),
            GameObject.Find("Obj_3"),
            GameObject.Find("Obj_4")
        };
        if (PrimerosObjetos[0] != null)
        {
            foreach (GameObject obj in PrimerosObjetos)
            {
                if (selected == obj)
                {
                    GameObject.Find("Inventario").transform.Find("Scrollbar").gameObject.GetComponent<Scrollbar>().value = 1;
                    return;
                }
            }
        }
        if (selected == null)
        {
            return;
        }
        if (selected.transform.parent != contentPanel.transform)
        {
            return;
        }
        if (selected == lastSelected)
        {
            return;
        }
        selectedRectTransform = selected.GetComponent<RectTransform>();
        float selectedPositionY = Mathf.Abs(selectedRectTransform.anchoredPosition.y) + selectedRectTransform.rect.height;
        float scrollViewMinY = contentPanel.anchoredPosition.y;
        float scrollViewMaxY = contentPanel.anchoredPosition.y + scrollRectTransform.rect.height;
        if (selectedPositionY > scrollViewMaxY)
        {
            float newY = selectedPositionY - scrollRectTransform.rect.height;
            contentPanel.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x, newY);
        }
        else if (Mathf.Abs(selectedRectTransform.anchoredPosition.y) < scrollViewMinY)
        {
            contentPanel.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x, Mathf.Abs(selectedRectTransform.anchoredPosition.y));
        }

        lastSelected = selected;
    }
}
