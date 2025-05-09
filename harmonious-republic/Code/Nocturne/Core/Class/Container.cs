using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Nocturne.Core.Inferface;

namespace Nocturne.Core.Class
{
    class Container<[MustBeVariant] TInput, [MustBeVariant] KOutput> : IContainer<TInput, KOutput>
    {
        private readonly List<IUnit> units = new List<IUnit>();

        public void Add(IUnit unit)
        {
            if (units.Count == 0)
            {
                Type[] genericArguments = unit.GetType().BaseType.GetGenericArguments();

                if (genericArguments.First() != typeof(TInput))
                {
                    throw new ArgumentException("The TInput of the first unit must match the TInput of the container.");
                }
            }
            else
            {
                // 检查后续unit的TInput是否与前一个unit的KOutput一致
                IUnit lastUnit = units.Last();

                Type[] lastGenericArguments = lastUnit.GetType().BaseType.GetGenericArguments();
                Type[] unitGenericArguments = unit.GetType().BaseType.GetGenericArguments();

                if (lastGenericArguments.Last() != unitGenericArguments.First())
                {
                    throw new ArgumentException("The TInput of the unit must match the KOutput of the previous unit.");
                }
            }

            units.Add(unit);
        }

        public void Clear(IUnit unit)
        {
            units.Clear();
        }

        public KOutput WorkFlow(TInput input)
        {
            if (units.Count == 0)
            {
                throw new Exception("There is no unit in the container.");
            }
            
            // 这里规定了输出类型必须严格遵循框架
            if (units.Last().GetType().BaseType.GetGenericArguments().Last() != typeof(KOutput))
            {
                throw new Exception("Ouput type is not match.");
            }

            Variant result = Variant.From(input);

            // 需要轮流执行
            foreach (IUnit unit in units)
            {
                result = unit.ExecuteDynamic(result);
            }

            return result.As<KOutput>();
        }
    }
}