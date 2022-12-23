using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ATBS.Notifications;
using UnityEngine.UI;
using TMPro;

public class ExampleTooltip : Tooltip
{
    [SerializeField] private TextMeshProUGUI textElement;
    private Image image;
    private byte transparency = 0;
    private bool changeTransparency = true;
    private void Start()
    {
        image = GetComponent<Image>();
        ChangeTransparency(0);
    }

    private void Update()
    {
        UpdateTransparency();
    }

    public override void Refresh()
    {
        textElement.text = DisplayText;
    }

    public override void NewParent() 
    {
        FireDelay();
    }

    private void FireDelay()
    {
        ChangeTransparency(0);
        StopCoroutine("DelayTransparency");
        StartCoroutine("DelayTransparency", 0.5f);
    }

    public override void Show()
    {
        base.Show();
        if(!NewParentCheck() && !Interactable) FireDelay();
    }

    private void ChangeTransparency(byte transparency)
    {
        if(transparency == 0)
            Interactable = false;
        else
            Interactable = true;

        image.color = new Color32(255, 255, 255, transparency);
        textElement.color = new Color32(0, 0, 0, transparency);
        this.transparency = transparency;
    }

    private IEnumerator DelayTransparency(float seconds)
    {
        changeTransparency = false;
        yield return new WaitForSeconds(seconds);
        changeTransparency = true;
    }

    private void UpdateTransparency()
    {
        if(!changeTransparency) return;
        
        if(IsShowed && transparency != 255)
        {
            transparency += 1;
            ChangeTransparency(transparency);
        }
        else if(!IsShowed && transparency != 0)
        {
            transparency -= 1;
            ChangeTransparency(transparency);
        }
    }
}
