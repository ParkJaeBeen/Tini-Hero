using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // 키보드, 마우스 터치를 이벤트로 오브젝트에 보낼 수 있는 기능을 지원

public class JoyStickTestM : MonoBehaviour , IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image imgJSBg;
    private Vector2 posInput;

    public Image imgJS;

    
    private void Awake()
    {
        imgJSBg = GetComponent<Image>();
        imgJS = transform.GetChild(0).GetComponent<Image>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("begin");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            imgJSBg.rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out posInput))
        {
            posInput.x = posInput.x / (imgJSBg.rectTransform.sizeDelta.x);
            posInput.y = posInput.y / (imgJSBg.rectTransform.sizeDelta.y);
            //Debug.Log(posInput.x.ToString() + "/" + posInput.y.ToString());

            if (posInput.magnitude > 1.0f)
            {
                posInput = posInput.normalized;
            }

            imgJS.rectTransform.anchoredPosition = new Vector2(
                posInput.x * (imgJSBg.rectTransform.sizeDelta.x / 2),
                posInput.y * (imgJSBg.rectTransform.sizeDelta.y / 2));

        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        posInput = Vector2.zero;
        imgJS.rectTransform.anchoredPosition = Vector2.zero;
    }

    public float inputHorizontal()
    {
        if (posInput.x != 0)
            return posInput.x;
        else
            return Input.GetAxis("Horizontal");
    }

    public float inputVertical()
    {
        if (posInput.y != 0)
            return posInput.y;
        else
            return Input.GetAxis("Vertical");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
