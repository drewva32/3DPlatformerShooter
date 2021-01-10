using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetable : IGetShot
{
   Transform TargetPoint { get; }

   event Action Die;
}
