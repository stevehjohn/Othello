namespace Othello.Engine.Bitboards;

public struct Planes
{
    public Planes() { }

    public Plane Black;

    public Plane White;

    public bool this[int cell] => Black[cell] | White[cell];
}