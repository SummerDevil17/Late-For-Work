using UnityEngine;
using UnityEngine.UI;

public class ButtonImageFader : MonoBehaviour
{
    [SerializeField] Image imageToFade;
    [SerializeField] Color normalColor;
    [SerializeField] Color fadedColor;

    public void FadeButtonImage() { imageToFade.color = fadedColor; }

    public void ReturnToNormal() { imageToFade.color = normalColor; }
}
