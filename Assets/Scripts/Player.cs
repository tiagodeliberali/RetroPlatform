using UnityEngine;

namespace RetroPlatform
{
    public class Player : MonoBehaviour
    {
        PlayerCore _core;
        public PlayerCore Core
        {
            get
            {
                if (_core == null) _core = new PlayerCore();
                return _core;
            }
            set
            {
                _core = value;
            }
        }
    }
}
