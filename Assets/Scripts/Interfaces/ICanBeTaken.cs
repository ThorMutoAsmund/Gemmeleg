using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Gemmeleg
{
    public interface ICanBeTaken : IEInteractive
    {
        ICanBeTaken Take(Transform actor);
        void Drop(GameObject player);
    }
}
