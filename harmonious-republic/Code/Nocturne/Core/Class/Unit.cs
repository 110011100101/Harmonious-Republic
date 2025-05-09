using Godot;
using Nocturne.Core.Inferface;

namespace Nocturne.Core.Class
{
    public abstract class Unit<[MustBeVariant] TInput, [MustBeVariant] KOutput>: IUnit<TInput, KOutput>
    {
        abstract public KOutput Execute(TInput input);
    }
}