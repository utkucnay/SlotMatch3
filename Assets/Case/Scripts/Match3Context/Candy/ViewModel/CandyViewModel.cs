using System;

namespace Case.Match3.Candy
{
    public struct CandyViewModel
    {
        public Guid Id;
        public CandyType CandyType;

        public CandyViewModel(CandyType candyType)
        {
            CandyType = candyType;
            Id = Guid.NewGuid();
        }

        private static Random random = new Random();
        public static CandyViewModel GetRandomViewModel()
        {
            var candyTypeValues = Enum.GetValues(typeof(CandyType));
            var CandyType = (CandyType)candyTypeValues.GetValue(random.Next(candyTypeValues.Length));
            
            var viewModel = new CandyViewModel(CandyType);
            return viewModel;
        }

        public override int GetHashCode()
        {
            return $"{CandyType}".GetHashCode();
        }
    }
}
