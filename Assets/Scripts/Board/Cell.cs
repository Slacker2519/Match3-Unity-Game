using System;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int BoardX { get; private set; }

    public int BoardY { get; private set; }

    public Item Item { get; private set; }

    public Cell NeighbourUp { get; set; }

    public Cell NeighbourRight { get; set; }

    public Cell NeighbourBottom { get; set; }

    public Cell NeighbourLeft { get; set; }


    public bool IsEmpty => Item == null;

    public void Setup(int cellX, int cellY)
    {
        this.BoardX = cellX;
        this.BoardY = cellY;
    }

    public bool IsNeighbour(Cell other)
    {
        return BoardX == other.BoardX && Mathf.Abs(BoardY - other.BoardY) == 1 ||
            BoardY == other.BoardY && Mathf.Abs(BoardX - other.BoardX) == 1;
    }

    public List<NormalItem.eNormalType> GetNeighbourItemType()
    {
        List<NormalItem.eNormalType> list = new List<NormalItem.eNormalType>();

        if (NeighbourUp != null)
        {
            if (NeighbourUp.Item != null)
            {
                if (NeighbourRight.Item is NormalItem item)
                {
                    list.Add(item.ItemType);
                }
            }
        }
        if (NeighbourRight != null)
        {
            if (NeighbourRight.Item != null)
            {
                if (NeighbourRight.Item is NormalItem item)
                {
                    list.Add(item.ItemType);
                }
            }
        }
        if (NeighbourBottom != null)
        {
            if (NeighbourBottom.Item != null)
            {
                if (NeighbourBottom.Item is NormalItem item)
                {
                    list.Add(item.ItemType);
                }
            }
        }
        if (NeighbourLeft != null)
        {
            if (NeighbourLeft.Item != null)
            {
                if (NeighbourLeft.Item is NormalItem item)
                {
                    list.Add(item.ItemType);
                }
            }
        }

        return list;
    }

    public void Free()
    {
        Item = null;
    }

    public void Assign(Item item)
    {
        Item = item;
        Item.SetCell(this);
    }

    public void ApplyItemPosition(bool withAppearAnimation)
    {
        Item.SetViewPosition(this.transform.position);

        if (withAppearAnimation)
        {
            Item.ShowAppearAnimation();
        }
    }

    internal void Clear()
    {
        if (Item != null)
        {
            Item.Clear(PoolManager.Instance);
            Item = null;
        }
    }

    internal bool IsSameType(Cell other)
    {
        return Item != null && other.Item != null && Item.IsSameType(other.Item);
    }

    internal void ExplodeItem()
    {
        if (Item == null) return;

        if (Item is NormalItem normalItem)
        {
            Board.OnDespawnItem?.Invoke(normalItem.ItemType);
        }

        Item.ExplodeView(PoolManager.Instance);
        Item = null;
    }

    internal void AnimateItemForHint()
    {
        Item.AnimateForHint();
    }

    internal void StopHintAnimation()
    {
        Item.StopAnimateForHint();
    }

    internal void ApplyItemMoveToPosition()
    {
        Item.AnimationMoveToPosition();
    }
}
