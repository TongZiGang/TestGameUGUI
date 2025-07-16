using UnityEngine;
using UnityEngine.UI;

namespace CreatGame.UI
{
    public class UIMainView : UIViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        public Button StarBtn;
        /// <summary>
        /// 
        /// </summary>
        public Text StarBtnText;
        public override void PreLoad(GameObject view)
        {
            base.PreLoad(view);

            StarBtn = GetGameObject(nameof(StarBtn)).GetComponent<Button>();
            StarBtnText = GetGameObject(nameof(StarBtnText)).GetComponent<Text>();
        }
    }
}
