namespace CreatGame
{
    public class GameLogic : Singleton<GameLogic>
    {
        public void Start()
        {
            AssetBundle.AssetBundleManager.Instance.Initialize();   
        }
    }
}

