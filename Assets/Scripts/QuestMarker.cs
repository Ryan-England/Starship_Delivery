using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestMarker : MonoBehaviour
{
    public Sprite icon;
    public Image image;
    public bool IsQuest;

    public Vector2 position {
        get { return new Vector2(transform.position.x, transform.position.y);}
    }
    public void SetIcon(Image targetImage)
    {
        if (icon != null && targetImage != null)
        {
            targetImage.sprite = icon;
        }
    }
}
