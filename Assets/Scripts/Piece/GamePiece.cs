using System;
using System.Collections;
using System.Collections.Generic;
using BoardMain;
using Enum;
using UnityEngine;
using Vo;

namespace Piece
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
      
      if (BoardRef.SkillType != SkillType.Empty) return;
      
      BoardRef.EnterPiece(this);
    }

    private void OnMouseDown()
    {
      if (!BoardRef.IsGamePiecesClickable) return;

      if (BoardRef.SkillType != SkillType.Empty)
      {
        SkillsProperties();

        return;
      }
      
      BoardRef.PressPiece(this);
    }

    private void OnMouseUp()
    {
      if (!BoardRef.IsGamePiecesClickable) return;

      if (BoardRef.SkillType != SkillType.Empty) return;

      BoardRef.ReleasePiece();
    }

    private void SkillsProperties()
    {
      switch (BoardRef.SkillType)
      {
        case SkillType.Paint:
          Paint();
          break;
        case SkillType.Break:
          Break();
          break;
        case SkillType.Empty:
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
