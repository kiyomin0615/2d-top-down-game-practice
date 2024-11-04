using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Button : UI_Popup
{
    enum Buttons
    {
        ClickButton
    }

    enum Texts
    {
        ClickText,
        ScoreText
    }

    enum Images
    {
        ItemIcon
    }

    enum GameObjects
    {
        TestObject
    }

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));

        // temp
        GameObject clickButton = Get<Button>((int)Buttons.ClickButton).gameObject;
        AddUIEventHandler(clickButton, OnButtonClicked, Definition.UIEvent.Click);

        GameObject itemIcon = Get<Image>((int)Images.ItemIcon).gameObject;
        AddUIEventHandler(itemIcon, (PointerEventData eventData) => { itemIcon.transform.position = eventData.position; }, Definition.UIEvent.Drag);
    }

    int score = 0;

    public void OnButtonClicked(PointerEventData eventData)
    {
        score++;
        Get<TextMeshProUGUI>((int)Texts.ScoreText).text = $"SCORE : {score}";
    }
}
