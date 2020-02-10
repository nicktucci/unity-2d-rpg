using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ResourceBar : MonoBehaviour
{
    [SerializeField]
    private Image fill = default;
    [SerializeField]
    private Image fillHistory = default;

    private float percent { get; set; } = 1;
    private Coroutine fillHistoryAnim;
    private float historyTarget = -1;

    private void UpdateFill(Image image, float percent)
    {
        image.rectTransform.localScale = new Vector3(percent, 1);
    }

    public void Link(AttributeInt attr)
    {
        attr.OnChanged += OnChange;
    }
    public void Unlink(Attribute<int> attr)
    {
        attr.OnChanged -= OnChange;
    }

    private void OnChange(Attribute<int> attr, int value, int oldValue)
    {
        float oldPercent = percent;
        ChangeValue(attr.Value, attr.ValueMax);

        if (historyTarget == percent) return;
        if(fillHistoryAnim != null)
        {
            StopCoroutine(fillHistoryAnim);
        }
        fillHistoryAnim = StartCoroutine(AnimateHistory(oldPercent, percent));
    }

    private void ChangeValue(float value, float valueMax)
    {
        percent = value / valueMax;
        UpdateFill(fill, percent);
    }

    private IEnumerator AnimateHistory(float start, float end)
    {
        historyTarget = end;
        float speed = .5f;
        float percent = start;


        yield return new WaitForSeconds(.4f);

        while(percent != end)
        {
            percent = FloatTowards(percent, end, speed);
         
            UpdateFill(fillHistory, percent);

            yield return new WaitForEndOfFrame();
        }
    }

    private float FloatTowards(float percent, float end, float speed)
    {
        float mult = 1;
        if (end > percent) mult = -1;

        percent -= (speed * Time.deltaTime) * mult;

        if (Mathf.Abs(percent) - Mathf.Abs(end) < 0.01f) percent = end;

        return percent;
    }
}
