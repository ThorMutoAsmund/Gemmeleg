using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Gemmeleg
{
    public interface ICanBeDriven
    {
        void Enter(GameObject player);
        void Exit();
    }
}
