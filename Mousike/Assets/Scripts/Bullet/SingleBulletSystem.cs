using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = Utils.Random;

using System;
using System.Collections;
using System.Collections.Generic;

using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms2D;

namespace Seduq.Mousike {
	public class SingleBulletSystem : JobComponentSystem {
        public struct Data {
            public readonly int Length;
            
        }
	}
}
