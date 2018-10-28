using System;
using UnityEngine;

namespace RetroPlatform.Levels
{
    public class CameraController : MonoBehaviour
    {
        public float xOffset = 0f;
        public float yOffset = 0f;
        public Transform player;
        public Transform roof;
        public Transform floor;

        public float minXPosition;
        public float maxXPosition;

        private GameObject[] backgrounds;
        private GameObject[] middlegrounds;

        private void Awake()
        {
            backgrounds = GameObject.FindGameObjectsWithTag("Background");
            middlegrounds = GameObject.FindGameObjectsWithTag("Middleground");
        }

        void LateUpdate()
        {
            float xPosition = Math.Min(Math.Max(player.transform.position.x + xOffset, minXPosition), maxXPosition);
            transform.position = new Vector3(xPosition, transform.position.y + yOffset, -10);
            roof.position = new Vector3(player.transform.position.x, roof.transform.position.y, -10);
            floor.position = new Vector3(player.transform.position.x, floor.transform.position.y, -10);

            SetPosition(backgrounds, 3);
            SetPosition(middlegrounds, 30);
        }

        private void SetPosition(GameObject[] gameObjectArray, float factor)
        {
            for (int i = 0; i < gameObjectArray.Length; i++)
            {
                gameObjectArray[i].transform.position = new Vector3(
                    player.transform.position.x / factor + i * 19 - 10,
                    gameObjectArray[i].transform.position.y,
                    gameObjectArray[i].transform.position.z);
            }
        }
    }
}