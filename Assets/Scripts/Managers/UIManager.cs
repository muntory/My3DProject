using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    // 현재 씬에 있는 UI들
    Dictionary<string, GameObject> UIList = new Dictionary<string, GameObject>();
    Stack<UIBase> popupStack = new Stack<UIBase>();
    private int sortOrder = 10;

    public void InitCanvas(Canvas canvas)
    {
        if (canvas == null)
            return;

        canvas.overrideSorting = true;

        canvas.sortingOrder = sortOrder;
        ++sortOrder;
    }

    public T CreateUI<T>(Transform parent = null)
    {
        string className = typeof(T).Name;
        string path = GetPath<T>();

        GameObject go;
        if (UIList.ContainsKey(className))
        {
            go = UIList[className];
        }
        else
        {
            go = Resources.Load<GameObject>(path);
            go = Instantiate(go, parent);

            UIList.Add(className, go);
        }
        Debug.Assert(go != null);
        T temp = go.GetComponent<T>();

        Debug.Assert(temp != null);
        return temp;
    }


    public T ShowPopup<T>(string prefabName = null) where T : UIBase
    {
        if (string.IsNullOrEmpty(prefabName))
        {
            prefabName = typeof(T).Name;
        }

        T popup;
        if (!UIList.TryGetValue(prefabName, out GameObject go))
        {
            popup = CreateUI<T>();
        }
        else
        {
            popup = go.GetComponent<T>();
        }

        if (!IsActive<T>())
        {
            popup.gameObject.SetActive(true);
        }

        popupStack.Push(popup);

        return popup;
    }

    public void ClosePopupUI()
    {
        if (popupStack.Count == 0)
            return;

        UIBase popup = popupStack.Pop();
        if (popup.gameObject.activeInHierarchy)
        {
            popup.gameObject.SetActive(false);
        }
        --sortOrder;
    }


    public string GetPath<T>(string prefabName = null)
    {
        if (string.IsNullOrEmpty(prefabName))
        {
            prefabName = typeof(T).Name;
        }

        return $"Prefabs/UI/{prefabName}";
    }


    public bool IsActive<T>()
    {
        string name = typeof(T).Name;

        if (!UIList.ContainsKey(name)) return false;

        return UIList[name].activeInHierarchy;
    }
}
