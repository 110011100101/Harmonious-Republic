using System.Collections.Generic;
using Godot;
using Nocturne.Core.Inferface;

namespace Nocturne.Core.Class
{
    class Framework<[MustBeVariant] TInput, [MustBeVariant ]KOutput>(IContainer<TInput, KOutput> container) : IFrame<TInput, KOutput>
    {
        public List<IFrame> InFrame { get; } = new() {}; // 规定了以进入形式链接的框架类型
        public List<IFrame> OutFrame { get; } = new() {}; // 规定了以输出形式链接的框架类型
        private readonly IContainer<TInput, KOutput> _container = container;

        public KOutput Main(TInput input)
        {
            return _container.WorkFlow(input);
        }
    }
}