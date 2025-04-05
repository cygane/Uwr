using System;
using System.Collections.Generic;

namespace Shapes
{
    public class ShapeFactory
    {
        private readonly List<IShapeFactoryWorker> _workers = new();

        public void RegisterWorker(IShapeFactoryWorker worker)
        {
            _workers.Add(worker);
        }

        public IShape CreateShape(string shapeName, params object[] parameters)
        {
            foreach (var worker in _workers)
            {
                if (worker.CanCreate(shapeName))
                {
                    return worker.CreateShape(parameters);
                }
            }

            throw new InvalidOperationException($"Shape '{shapeName}' not supported.");
        }
    }
}
