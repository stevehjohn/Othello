namespace Othello.Engine.Infrastructure;

public readonly struct Planes
{
    public Planes() { }

    public Plane Black { get; } = new();

    public Plane White { get; } = new();

    public bool this[int cell] => Black[cell] | White[cell];
}