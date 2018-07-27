using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = Utils.Random;

using System;
using System.Collections;
using System.Collections.Generic;

namespace Utils {
    public struct MinMax<T> {
        public T min;
        public T max;

        public MinMax(T min, T max) {
            this.min = min;
            this.max = max;
        }
    }
}