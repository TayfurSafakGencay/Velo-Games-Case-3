using DG.Tweening;
using UnityEngine;

public class ClearablePiece : MonoBehaviour
{
    private bool _isBeingCleared;

    public bool IsBeingCleared => _isBeingCleared;

    protected GamePiece _piece;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _piece = gameObject.GetComponent<GamePiece>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public virtual void Clear()
    {
        _piece.BoardRef.Level.OnPieceCleared(_piece);
        
        _isBeingCleared = true;
        
        transform.DOScale(transform.localScale * 1.5f, 1.25f).SetEase(Ease.OutQuad);

        _spriteRenderer.DOFade(0f, 1.25f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}