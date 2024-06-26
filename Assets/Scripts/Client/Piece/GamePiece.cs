using System;
using System.Collections.Generic;
using Client.BoardMain;
using Client.Enum;
using Client.Vo;
using UnityEngine;

namespace Client.Piece
{
  public class GamePiece : MonoBehaviour
  {
    public int Score;
    
    [SerializeField]
    private List<SpecialObject> _specialObjectsSprites;
    
    private readonly Dictionary<PieceType, Sprite> _specialObjectSpriteDictionary = new();
  
    private int _x;
    public int X
    {
      get => _x;
      set
      {
        if (IsMovable())
        {
          _x = value;
        }
      }
    }

    private int _y;
    public int Y
    {
      get => _y;
      set
      {
        if (IsMovable())
        {
          _y = value;
        }
      }
    }

    public PieceType PieceType { get; private set; }

    public Board BoardRef { get; private set; }

    public MovablePiece MovableComponent { get; private set; }
  
    public ColorPiece ColorComponent { get; private set; }
  
    public ClearablePiece ClearableComponent { get; private set; }

    private void Awake()
    {
      MovableComponent = gameObject.GetComponent<MovablePiece>();
      ColorComponent = gameObject.GetComponent<ColorPiece>();
      ClearableComponent = gameObject.GetComponent<ClearablePiece>();
      
      for (int i = 0; i < _specialObjectsSprites.Count; i++)
      {
        if (!_specialObjectSpriteDictionary.ContainsKey(_specialObjectsSprites[i].PieceType))
        {
          _specialObjectSpriteDictionary.Add(_specialObjectsSprites[i].PieceType, _specialObjectsSprites[i].Sprite);
        }
      }
    }

    public void Init(int x, int y, Board board, PieceType type)
    {
      X = x;
      Y = y;
      BoardRef = board;
      PieceType = type;
    }

    public void SetPieceTypeInitial(PieceType pieceType, ColorType colorType)
    {
      if (!_specialObjectSpriteDictionary.TryGetValue(pieceType, out Sprite value)) return;

      ColorComponent.SetSprite(value, colorType);
      PieceType = pieceType;
    }
    
    private void OnMouseEnter()
    {
      if (!BoardRef.IsGamePiecesClickable) return;
      
      if (BoardRef.SkillKey != SkillKey.Empty) return;
      
      BoardRef.EnterPiece(this);
    }

    private void OnMouseDown()
    {
      if (!BoardRef.IsGamePiecesClickable) return;

      if (BoardRef.SkillKey != SkillKey.Empty)
      {
        SkillsProperties();

        return;
      }
      
      BoardRef.PressPiece(this);
    }

    private void OnMouseUp()
    {
      if (!BoardRef.IsGamePiecesClickable) return;

      if (BoardRef.SkillKey != SkillKey.Empty) return;

      BoardRef.ReleasePiece();
    }

    private void SkillsProperties()
    {
      switch (BoardRef.SkillKey)
      {
        case SkillKey.Paint:
          Paint();
          break;
        case SkillKey.Break:
          Break();
          break;
        case SkillKey.Empty:
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private void Paint()
    {
      if (PieceType != PieceType.Normal)
        return;
      
      BoardRef.PaintPiece(this);
    }

    private void Break()
    {
      if (PieceType != PieceType.Normal)
        return;
      
      BoardRef.BreakPiece(X, Y);
    }

    public bool IsMovable()
    {
      return MovableComponent != null;
    }

    public bool IsColored()
    {
      return ColorComponent != null;
    }

    public bool IsClearable()
    {
      return ClearableComponent != null;
    }
  }
}
