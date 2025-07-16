using System.Collections;
using CreatGame.UI;
using UnityEditor.VersionControl;

namespace CreatGame
{
    public class GameLogic : Singleton<GameLogic>
    {
        public IEnumerator Start()
        {
            AssetBundle.AssetBundleManager.Instance.Initialize();
            while (true)
            {
                if (AssetBundle.AssetBundleManager.Instance.IsInitializeAsync)
                {
                    break;
                }

                yield return null;
            }

            UIManager.Instance.OpenView<UI.UIMainView>(UILayer.Main);
        }
    }
}

