using System;
using UnityEngine;

namespace WSP.VFX
{
    public abstract class VfxObject : MonoBehaviour
    {
        public Action OnFinished { get; set; }

        public abstract void Play();
    }
}