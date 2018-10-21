using UnityEngine;

namespace RetroPlatform
{
    public class CameraController : MonoBehaviour
    {
        public float xOffset = 0f;
        public float yOffset = 0f;
        public Transform player;
        public Transform roof;
        public Transform floor;
        
        void LateUpdate()
        {
            transform.position = new Vector3(player.transform.position.x + xOffset, transform.position.y + yOffset, -10);
            roof.position =  new Vector3(player.transform.position.x, roof.transform.position.y, -10);
            floor.position =  new Vector3(player.transform.position.x, floor.transform.position.y, -10);
        }
    }
}