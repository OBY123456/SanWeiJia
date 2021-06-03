using UnityEngine;
using DG.Tweening;

public static class ExtensionMethods
{
    public static void Open(this CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }
    public static void Hide(this CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    public static void Open(this CanvasGroup canvasGroup, float Time)
    {
        canvasGroup.DOFade(1, Time);
        canvasGroup.blocksRaycasts = true;
    }
    public static void Hide(this CanvasGroup canvasGroup, float Time)
    {
        canvasGroup.DOFade(0, Time);
        canvasGroup.blocksRaycasts = false;
    }
}
