using UnityEngine;

public class CopyTransform : MonoBehaviour
{
    [SerializeField] RectTransform objectToCopy;
    [SerializeField] float amountToFollow = 8f;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (objectToCopy.anchoredPosition == Vector2.zero) rectTransform.anchoredPosition = Vector2.zero;

        rectTransform.anchoredPosition = -amountToFollow * (rectTransform.anchoredPosition - objectToCopy.anchoredPosition).normalized;
    }
}
