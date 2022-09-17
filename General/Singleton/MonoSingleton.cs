using UnityEngine;

namespace General.Singletons
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        public static T I { get; private set; } = null;

        private void Awake()
        {
            if (I == null)
            {
                I = this as T;
                I.OnAwake();
                return;
            }

            Destroy(this);
        }

        protected virtual void OnAwake()
        {
        }

        private void OnDestroy()
        {
            if (I == this)
            {
                I = null;
            }
        }

    }
}