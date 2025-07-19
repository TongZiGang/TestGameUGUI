using UnityEngine;
using UnityEngine.UI;

namespace CreatGame.UI
{
    public class MainItem : MonoBehaviour
    {
        public Text text;
        
        void ScrollCellIndex (int idx) 
        {
            string name = "Cell " + idx.ToString ();
            if (text != null) 
            {
                text.text = name;
            }
            gameObject.name = name;
        }
    }
}