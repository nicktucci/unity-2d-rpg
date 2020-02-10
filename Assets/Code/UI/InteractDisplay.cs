using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractDisplay : MonoBehaviour
{
    [SerializeField]
    TMP_Text text = default;

    private List<IInteractable> queue = new List<IInteractable>();

    private void Start()
    {
        gameObject.SetActive(false);
        GlobalEvents.Get.Subscribe(Events.Global.UI.ActivateInteractable, data =>
        {
            if(queue.Count > 0)
            {
                queue[0].Interact((Unit) data);

            }
            //gameObject.SetActive(true);
        });
        GlobalEvents.Get.Subscribe(Events.Global.UI.QueueInteractable, data =>
        {
            IInteractable obj = (IInteractable)data;

            if (queue.Contains(obj)) queue.Remove(obj);
            queue.Add(obj);

            SetActive(obj);

            gameObject.SetActive(true);
        });
        GlobalEvents.Get.Subscribe(Events.Global.UI.DequeueInteractable, data =>
        {
            IInteractable obj = (IInteractable)data;
            queue.Remove(obj);
            if(queue.Count == 0)
            {
                gameObject.SetActive(false);

            }
        });
    }

    private void SetActive(IInteractable obj)
    {
        text.text = "[E] "+obj.InteractLabel;
    }
}
