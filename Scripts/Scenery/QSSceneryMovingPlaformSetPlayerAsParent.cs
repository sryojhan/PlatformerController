using UnityEngine;


namespace QuickStart
{

    public class QSSceneryMovingPlaformSetPlayerAsParent : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            collision.transform.SetParent(transform);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.activeInHierarchy) 
                collision.transform.SetParent(null);
        }
    }
}
