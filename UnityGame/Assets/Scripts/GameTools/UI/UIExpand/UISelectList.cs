using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace CreatGame.UI
{
    public delegate void OnSelectCallback(int index);
    /// <summary>
    /// 选中列表
    /// </summary>
    public class UISelectList : MonoBehaviour
    {
        public OnSelectCallback onSelectCallback;
        private List<Button> m_buttonList = new List<Button>();
        private void Awake()
        {
            m_buttonList = new List<Button>();
        }
        /// <summary>
        /// 
        /// </summary>
        private void Start()
        {
            var childCount= transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                var button = transform.GetChild(i).GetComponent<Button>();
                if (button != null)
                {
                    m_buttonList.Add(button);
                    button.onClick.AddListener(() => { OnClick(button); });
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="btn"></param>
        private void OnClick(Button btn)
        {
            onSelectCallback?.Invoke(m_buttonList.IndexOf(btn));
        }

        public int Count => m_buttonList.Count;

        public Transform this[int index]
        {
            get
            {
                if (index < 0 || index >= m_buttonList.Count)
                {
                    return null;
                }
                return m_buttonList[index].transform;
            }
        }
        /// <summary>
        /// 重置按下状态
        /// </summary>
        public void ResetSelect()
        {
            foreach (var button in m_buttonList)
            {
                button.GetComponent<UISelected>().Selected = false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public void SetSelect(int index)
        {
            for (var i = 0; i < m_buttonList.Count; i++)
            {
                var component = m_buttonList[i].GetComponent<UISelected>();
                if (component == null) continue;
                component.Selected = index == i;   
            }
        }
    }
}