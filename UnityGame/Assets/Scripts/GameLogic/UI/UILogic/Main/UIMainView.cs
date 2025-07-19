using System.Collections.Generic;
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
            StarBtnText.text = ConfigManager.Instance.Tables.TbLanguage.Get("Main_BtnTitle_Star").CN;
            Debug.Log("OnStarBtnClick");
        }
    }
}