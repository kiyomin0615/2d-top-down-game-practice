using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class UIManager
{
    int order = 10;

    Stack<UI_Popup> popupStack = new Stack<UI_Popup>();
    UI_Scene scene = null;

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UIRoot");
            if (root == null)
                root = new GameObject() { name = "@UIRoot" };
            return root;
        }
    }

    public void SetCanvas(GameObject ui, bool sort = true)
    {
        Canvas canvas = Utility.GetOrAddComponent<Canvas>(ui);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = order;
            order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    public T OpenSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject ui = Manager.Resource.Instantiate($"UI/Scene/{name}"); // Assets/Resources/Prefabs/UI/Scene/{name}
        T scene = Utility.GetOrAddComponent<T>(ui);
        this.scene = scene;

        ui.transform.SetParent(Root.transform, false);

        return scene;
    }

    public T OpenPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject ui = Manager.Resource.Instantiate($"UI/Popup/{name}"); // Assets/Resources/Prefabs/UI/Popup/{name}
        T popup = Utility.GetOrAddComponent<T>(ui);
        popupStack.Push(popup);

        ui.transform.SetParent(Root.transform, false);

        return popup;
    }

    public T GenerateSubItemUI<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject ui = Manager.Resource.Instantiate($"UI/SubItem/{name}"); // Assets/Resources/Prefabs/UI/SubItem/{name}

        if (parent != null)
            ui.transform.SetParent(parent, false); // worldPositionStays가 true이면, item의 scale이 변경된다

        return Utility.GetOrAddComponent<T>(ui);
    }

    public void ClosePopupUI()
    {
        if (popupStack.Count == 0)
            return;

        UI_Popup popup = popupStack.Pop();
        Manager.Resource.Destroy(popup.gameObject);
        popup = null;
        order--;
    }

    public void ClosePopupUI(UI_Popup popup)
    {
        if (popupStack.Count == 0)
            return;

        if (popupStack.Peek() != popup)
        {
            Debug.Log("Failed to close the popup UI.");
            return;
        }

        ClosePopupUI();
    }

    public void CloseAllPopupUI()
    {
        while (popupStack.Count > 0)
        {
            ClosePopupUI();
        }
    }

    public void Clear()
    {
        scene = null;
        CloseAllPopupUI();
    }
}
