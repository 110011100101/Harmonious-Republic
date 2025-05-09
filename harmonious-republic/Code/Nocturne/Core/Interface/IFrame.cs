using Godot;

namespace Nocturne.Core.Inferface
{
    internal interface IFrame
    {
        Variant ExecuteDynamic(Variant input);
    }

    internal interface IFrame<[MustBeVariant] in TInput, [MustBeVariant] out KOutput> : IFrame
    {
        KOutput Main(TInput input);

        Variant IFrame.ExecuteDynamic(Variant input)
        {
            return Variant.From(Main(input.As<TInput>()));
        }
    }
}