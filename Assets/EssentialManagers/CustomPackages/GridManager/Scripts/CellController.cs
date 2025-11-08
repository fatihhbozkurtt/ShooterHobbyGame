using System.Collections.Generic;
using EssentialManagers.Scripts.Managers;
using UnityEngine;

namespace EssentialManagers.CustomPackages.GridManager.Scripts
{
    public class CellController : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Transform cellGround;

        [SerializeField] private BaseCellOccupant occupierObjectPrefab;

        [Header("Debug")] public bool isPickable;
        public bool isOccupied;
        [SerializeField] private BaseCellOccupant spawnedOccupierObj;
        [SerializeField] Vector2Int coordinates;
        public List<CellController> neighbours;

        private void Start()
        {
            name = coordinates.ToString();

            neighbours = GetNeighbors();
        }

        public void Initialize(Vector2Int initCoords)
        {
            coordinates = initCoords;

            BaseCellOccupant clone = Instantiate(occupierObjectPrefab, transform.position, Quaternion.identity, transform);
            spawnedOccupierObj = clone;
            SetOccupied(spawnedOccupierObj);
        }

        private void OnMouseDown()
        {
            if (!GameManager.instance.isLevelActive) return;
            if (!isOccupied) return;

            if (spawnedOccupierObj) spawnedOccupierObj.gameObject.SetActive(false);
        }

        #region GETTERS & SETTERS

        public void SetOccupied(BaseCellOccupant newOccupier)
        {
            spawnedOccupierObj = newOccupier;
            isOccupied = true;
        }

        public void SetFree()
        {
            spawnedOccupierObj = null;
            isOccupied = false;
        }

        public BaseCellOccupant GetOccupierObject()
        {
            return spawnedOccupierObj;
        }

        public Vector2Int GetCoordinates()
        {
            return coordinates;
        }

        private List<CellController> GetNeighbors()
        {
            List<CellController> gridCells = GridManager.instance.gridPlan;
            List<CellController> neighbors = new();

            // Direction vectors for 8 directions (including diagonals)
            int[] dx = { 1, 1, 0, -1, -1, -1, 0, 1 };
            int[] dz = { 0, 1, 1, 1, 0, -1, -1, -1 };

            for (int i = 0; i < dx.Length; i++)
            {
                Vector2Int neighborCoordinates = coordinates + new Vector2Int(dx[i], dz[i]);
                CellController neighbor = gridCells.Find(cell => cell.coordinates == neighborCoordinates);

                if (neighbor != null)
                {
                    neighbors.Add(neighbor);
                }
            }

            return neighbors;
        }

        #endregion
    }
}