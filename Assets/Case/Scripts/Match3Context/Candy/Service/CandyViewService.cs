using UnityEngine;
using System.Collections.Generic;

namespace Case.Match3.Candy
{
    public class CandyViewService
    {
        Dictionary<CandyType, Sprite> candyDictonary;

        public CandyViewService(CandyViewImage candyViewImage)
        {
            candyDictonary = new()
        {
            { CandyType.Type1, candyViewImage.CandyType1 },
            { CandyType.Type2, candyViewImage.CandyType2 },
            { CandyType.Type3, candyViewImage.CandyType3 },
            { CandyType.Type4, candyViewImage.CandyType4 },
            { CandyType.Type5, candyViewImage.CandyType5 },
            { CandyType.Type6, candyViewImage.CandyType6 },
            { CandyType.Type7, candyViewImage.CandyType7 },
        };
        }


        public Sprite GetSprite(CandyType candyType)
        {
            return candyDictonary[candyType];
        }
    }
}