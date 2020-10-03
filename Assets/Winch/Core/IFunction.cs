using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFunction
{
    float Sample(params float[] x);
}