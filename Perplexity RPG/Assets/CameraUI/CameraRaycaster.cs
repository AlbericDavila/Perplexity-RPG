using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using RPG.Characters;

namespace RPG.CameraUI
{
	public class CameraRaycaster : MonoBehaviour
	{
		// INSPECTOR PROPERTIES RENDERED BY CUSTOM EDITOR SCRIPT
		[SerializeField] Texture2D walkCursor = null;
        [SerializeField] Texture2D enemyCursor = null;
        [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

		const int POTENTIALLY_WALKABLE_LAYER = 8;
	    float maxRaycastDepth = 100f; // Hard coded value

        Rect currentScreenRect;

        // Setup delegates for broadcasting layer changes to other classes
        public delegate void OnMouseOverEnemy(EnemyAI enemy);
        public event OnMouseOverEnemy onMouseOverEnemy;

        public delegate void OnMouseOverTerrain(Vector3 destination);
		public event OnMouseOverTerrain onMouseOverPotentiallyWalkable;

        void Update()
		{
            currentScreenRect = new Rect(0, 0, Screen.width, Screen.height);

            // Check if pointer is over an interactable UI element
            if (EventSystem.current.IsPointerOverGameObject ()) 
			{
				// TODO add UI interaction
			} 
			else 
			{
                performRaycast();
            }
		}

        void performRaycast()
        {
            if (currentScreenRect.Contains(Input.mousePosition))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (raycastForEnemy(ray)) { return; }
                if (RaycastForPotentiallyWalkable(ray)) { return; }
            }
        }

		bool raycastForEnemy(Ray ray)
		{
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo, maxRaycastDepth);
            var gameObjectHit = hitInfo.collider.gameObject;
            var enemyHit = gameObjectHit.GetComponent<EnemyAI>();
            if (enemyHit)
            {
                Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverEnemy(enemyHit);
                return true;
            }
			return false;
		}

		private bool RaycastForPotentiallyWalkable(Ray ray)
		{
			RaycastHit hitInfo;
			LayerMask potentiallyWalkableLayer = 1 << POTENTIALLY_WALKABLE_LAYER;
			bool potentiallyWalkableHit = Physics.Raycast(ray, out hitInfo, maxRaycastDepth, potentiallyWalkableLayer);
			if (potentiallyWalkableHit)
			{
				Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
				onMouseOverPotentiallyWalkable(hitInfo.point);
				return true;
			}
 			return false;
		}

	}
}