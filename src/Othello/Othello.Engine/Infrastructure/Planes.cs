namespace Othello.Engine.Infrastructure;

public struct Planes
{
    public Planes() { }

    public Plane Black { get; private set; } = new();

    public Plane White { get; private set; } = new();

    public bool this[int cell] => Black[cell] | White[cell];
}