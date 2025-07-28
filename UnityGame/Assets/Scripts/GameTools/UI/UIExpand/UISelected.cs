using UnityEngine;

namespace CreatGame.UI
{
    public class UISelected : MonoBehaviour
    {
        public int index = -1;

        /// <summary>
        /// 选中状态
        /// </summary>
        public GameObject selected;

        /// <summary>
        /// 未选中状态
        /// </summary>
        public GameObject unSelected;

        public bool Selected
        {
            get => selected.activeSelf;
            set
            {
                selected?.SetActive(value);
                unSelected?.SetActive(!value);
            }
        }
    }
}