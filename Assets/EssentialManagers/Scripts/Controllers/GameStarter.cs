using EssentialManagers.Scripts.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EssentialManagers.Scripts.Controllers
{
    public class GameStarter : MonoBehaviour, IPointerDownHandler
    {
        bool ready = false;

        private void Start()
        {
            ready = true;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (ready)
            {
                ready = false;
                GameManager.instance.StartGame();
            }
        }
    }
}
