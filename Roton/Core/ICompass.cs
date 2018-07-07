namespace Roton.Core
{
    public interface ICompass
    {
        IXyPair GetCardinalVector(int index);
        IXyPair GetConveyorVector(int index);
        IXyPair Rnd();
        IXyPair RndP(IXyPair vector);
        IXyPair Seek(IXyPair location);        
    }
}