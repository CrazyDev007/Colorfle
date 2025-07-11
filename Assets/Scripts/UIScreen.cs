using UnityEngine;

namespace MB
{
    public abstract class UIScreen : MonoBehaviour
    {
        [SerializeField] private GameObject root;

        public virtual void Show()
        {
            root.SetActive(true);
        }

        public virtual void Hide()
        {
            root.SetActive(false);
        }
    }
}