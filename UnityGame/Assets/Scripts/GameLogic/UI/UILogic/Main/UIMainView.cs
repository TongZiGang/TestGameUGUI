using UnityEngine;

namespace CreatGame.UI
{
    public partial class UIMainView
    {
        public override void InitView()
        {
            base.InitView();
            StarBtn.onClick.AddListener(OnStarBtnClick);
        }

        private void OnStarBtnClick()
        {
            
        }
    }
}