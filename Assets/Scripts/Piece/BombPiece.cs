﻿using System.Collections;
using DG.Tweening;
using Enum;
using UnityEngine;

namespace Piece
{
  public class BombPiece : ClearablePiece
  {
    public override bool Clear()
    {
      if (IsStartedAnimation) return true;
      IsStartedAnimation = true;
      
      // _piece.BoardRef.IncreaseDestroyingObjectCount();

      StartCoroutine(BeforeDestroyEffect(0f));
      
      return true;
    }

    public override IEnumerator BeforeDestroyEffect(float time)
    {      
      ExplosionAnimation();

      yield return new WaitForSeconds(time);
      
      _explosionEffect.Kill();
      
      int radius = _piece.PieceType == PieceType.SuperBomb ? 3 : 1;
      _piece.BoardRef.ClearBomb(_piece, radius);
      
      _piece.BoardRef.Fillers();
      
      DestroyAnimation();
    }
  }
}