using System.ComponentModel.DataAnnotations;
using Godot;

namespace Nocturne.Core.Inferface
{
    internal interface IUnit
    {
        Variant ExecuteDynamic(Variant input);
    }

    internal interface IUnit<[MustBeVariant] in TInput, [MustBeVariant] out KOutput> : IUnit
    {
        KOutput Execute(TInput input);

        Variant IUnit.ExecuteDynamic(Variant input)
        {
            return Variant.From(Execute(input.As<TInput>()));
        }
    }
}