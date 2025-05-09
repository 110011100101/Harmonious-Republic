using Godot;

namespace Nocturne.Core.Inferface
{
    internal interface IContainer<in TInput, out KOutput>
    {
        void Add(IUnit unit); // 添加内容
        void Clear(IUnit unit); // 移除内容
        KOutput WorkFlow(TInput input);
    }
}