using UnityEngine;

namespace Pieces
{
    public abstract class Soldier : Piece
    {
        [SerializeField] protected float movementDuration;
        [SerializeField] protected AnimationCurve animCurve;
    }
}
