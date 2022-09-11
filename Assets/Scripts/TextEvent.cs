using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TextEvent : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.LogErrorFormat("OnPointerClick {0}",eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.LogErrorFormat("OnPointerEnter {0}",eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.LogErrorFormat("OnPointerExit {0}",eventData);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
