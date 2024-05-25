using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //ª∫¥ÊUI
    Dictionary<string ,GameObject> m_UI = new Dictionary<string, GameObject> ();

    Dictionary<string ,Transform> m_UIGroup = new Dictionary<string, Transform> ();

    private Transform m_UIParent;

    private void Awake()
    {
        m_UIParent = this.transform.parent.Find("UI");
    }

   public void SetUIGroup(List<string> group)
   {
        for (int i = 0; i < group.Count; i++)
        {
            GameObject go = new GameObject("Group-" + group[i]);
            go.transform.SetParent(m_UIParent,false);
            m_UIGroup.Add(group[i], go.transform);
        }
   }
    Transform GetUIGroup(string Group)
    {
        if (!m_UIGroup.ContainsKey(Group))
        {
            Debug.LogError("group is not exist");
        }

        return m_UIGroup[Group];
    }
    public void OpenUI(string uiName ,string group, string luaName)
    {
        GameObject ui = null;
        if (m_UI.TryGetValue(uiName,out ui))
        {
            UILogic uILogic = ui.GetComponent<UILogic>();
            uILogic.OnOpen();
            return;
        }
        Manager.Resource.LoadUI(uiName, (Object obj) =>
        {
            ui = (GameObject)Instantiate(obj);
            m_UI.Add(uiName, ui);

            Transform paret = GetUIGroup(group);

            ui.transform.SetParent(paret,false);
            UILogic uILogic = ui.AddComponent<UILogic>();
            uILogic.Init(luaName);
            uILogic.OnOpen();

        });
    }
    
}
