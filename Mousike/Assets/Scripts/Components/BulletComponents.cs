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

	public struct ShotComponent : IComponentData {
        public float lifeTime;
        public float radius;
	}

    public struct ShotMovementComponent : IComponentData {
        public float2 position;
        public float2 velocity;
    }

    public struct ShotAccelComponent : IComponentData {
        public float2 acceleration;
        public float drag;
    }

    public struct FragShotComponent : IComponentData {
        public uint fragCount;
        public ShotComponent frag;
        public float delay;
    }

    public struct LineShotComponent : IComponentData {
        public float2 origin;
        public float startDistance;
        public float range;
        public float width;
        public float fadeIn;
    }
}
