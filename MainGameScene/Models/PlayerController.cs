//using System.Numerics;
using UnityEngine;

namespace MainGameScene.Model
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] Player player;

        // void Update()
        // {
        //     Vector3 direction = new Vector3(1.0f, 0.0f, 1.0f);
        //     MoveToDirection(direction);
        // }

        public void MoveToDirection(Vector2 direction)
        {
            player.fps.RunAxis = direction;
        }
    }
}