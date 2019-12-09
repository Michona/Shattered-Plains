using UnityEngine;

/** Handles highlighting tiles when character is selected. */
[RequireComponent(typeof(BoardManager))]
public class BoardSelector : MonoBehaviour
{
    [SerializeField] private Material highlightMaterial;

    [SerializeField] private Material idleMaterial;

    private BoardManager _manager;

    private void Awake() {
        _manager = gameObject.GetComponent<BoardManager>();
    }

    public void OnEnable() {
        EventHub.Instance.AddListener<CharacterSelectedEvent>(HighlightTiles);
        EventHub.Instance.AddListener<MovePlayerEvent>(ClearHighlight);
    }

    public void OnDisable() {
        EventHub.Instance.RemoveListener<CharacterSelectedEvent>(HighlightTiles);
        EventHub.Instance.RemoveListener<MovePlayerEvent>(ClearHighlight);
    }


    private void ClearHighlight(MovePlayerEvent e) {
        foreach (Tile tile in _manager.TileList) {
            tile.GetComponent<MeshRenderer>().material = idleMaterial;
        }
    }

    private void HighlightTiles(CharacterSelectedEvent e) {
        //Clear prev highlight
        ClearHighlight(new MovePlayerEvent());
        
        foreach (var tile in _manager.TileList) {
            if (_manager.GetDistanceBetweenTiles(e.TileId, tile.Id) <= e.MoveDistance && !tile.isOccupied) {
                tile.GetComponent<MeshRenderer>().material = highlightMaterial;
            }
        }
    }
}