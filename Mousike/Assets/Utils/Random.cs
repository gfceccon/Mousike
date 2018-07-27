using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using RandomUnity = UnityEngine.Random;

using System;
using System.Collections;
using System.Collections.Generic;

namespace Utils {
    public static class Random {
        private static int seed;

        /// <summary>
        /// Init a Unity state
        /// </summary>
        /// <param name="seed">The Unity RNG seed</param>
        public static void Init(int seed) {
            Random.seed = seed;
            RandomUnity.InitState(Random.seed);
        }

        /// <summary>
        /// Reset to previous set seed
        /// </summary>
        public static void Reset() {
            RandomUnity.InitState(Random.seed);
        }

        /// <summary>
        /// Flip a coin
        /// </summary>
        /// <returns>Return 1 if head, 0 if tail</returns>
        public static int Coin() {
            return RandomUnity.value > 0.5f ? 1 : 0;
        }

        /// <summary>
        /// Return a positive or negative integer
        /// </summary>
        /// <returns>Return 1 or -1</returns>
        public static int Sign() {
            return RandomUnity.value > 0.5f ? 1 : -1;
        }

        #region Unity Random Functions
        //
        // Summary:
        //     ///
        //     Returns a random point inside a circle with radius 1 (Read Only).
        //     ///
        public static Vector2 insideUnitCircle {
            get {
                return RandomUnity.insideUnitCircle;
            }
        }
        //
        // Summary:
        //     ///
        //     Returns a random point inside a sphere with radius 1 (Read Only).
        //     ///
        public static Vector3 insideUnitSphere {
            get {
                return RandomUnity.insideUnitSphere;
            }
        }
        //
        // Summary:
        //     ///
        //     Returns a random point on the surface of a sphere with radius 1 (Read Only).
        //     ///
        public static Vector3 onUnitSphere {
            get {
                return RandomUnity.onUnitSphere;
            }
        }
        //
        // Summary:
        //     ///
        //     Returns a random rotation (Read Only).
        //     ///
        public static Quaternion rotation {
            get {
                return RandomUnity.rotation;
            }
        }
        //
        // Summary:
        //     ///
        //     Returns a random rotation with uniform distribution (Read Only).
        //     ///
        public static Quaternion rotationUniform {
            get {
                return RandomUnity.rotationUniform;
            }
        }
        //
        // Summary:
        //     ///
        //     Gets/Sets the full internal state of the random number generator.
        //     ///
        public static RandomUnity.State state {
            get {
                return RandomUnity.state;
            }

            set {
                RandomUnity.state = value;
            }
        }
        //
        // Summary:
        //     ///
        //     Returns a random number between 0.0 [inclusive] and 1.0 [inclusive] (Read Only).
        //     ///
        public static float value {
            get {
                return RandomUnity.value;
            }
        }

        //
        // Summary:
        //     ///
        //     Generates a random color from HSV and alpha ranges.
        //     ///
        //
        // Parameters:
        //   hueMin:
        //     Minimum hue [0..1].
        //
        //   hueMax:
        //     Maximum hue [0..1].
        //
        //   saturationMin:
        //     Minimum saturation [0..1].
        //
        //   saturationMax:
        //     Maximum saturation[0..1].
        //
        //   valueMin:
        //     Minimum value [0..1].
        //
        //   valueMax:
        //     Maximum value [0..1].
        //
        //   alphaMin:
        //     Minimum alpha [0..1].
        //
        //   alphaMax:
        //     Maximum alpha [0..1].
        //
        // Returns:
        //     ///
        //     A random color with HSV and alpha values in the input ranges.
        //     ///
        public static Color ColorHSV() {
            return RandomUnity.ColorHSV();
        }
        //
        // Summary:
        //     ///
        //     Generates a random color from HSV and alpha ranges.
        //     ///
        //
        // Parameters:
        //   hueMin:
        //     Minimum hue [0..1].
        //
        //   hueMax:
        //     Maximum hue [0..1].
        //
        //   saturationMin:
        //     Minimum saturation [0..1].
        //
        //   saturationMax:
        //     Maximum saturation[0..1].
        //
        //   valueMin:
        //     Minimum value [0..1].
        //
        //   valueMax:
        //     Maximum value [0..1].
        //
        //   alphaMin:
        //     Minimum alpha [0..1].
        //
        //   alphaMax:
        //     Maximum alpha [0..1].
        //
        // Returns:
        //     ///
        //     A random color with HSV and alpha values in the input ranges.
        //     ///
        public static Color ColorHSV(float hueMin, float hueMax) {
            return RandomUnity.ColorHSV(hueMin, hueMax);
        }
        //
        // Summary:
        //     ///
        //     Generates a random color from HSV and alpha ranges.
        //     ///
        //
        // Parameters:
        //   hueMin:
        //     Minimum hue [0..1].
        //
        //   hueMax:
        //     Maximum hue [0..1].
        //
        //   saturationMin:
        //     Minimum saturation [0..1].
        //
        //   saturationMax:
        //     Maximum saturation[0..1].
        //
        //   valueMin:
        //     Minimum value [0..1].
        //
        //   valueMax:
        //     Maximum value [0..1].
        //
        //   alphaMin:
        //     Minimum alpha [0..1].
        //
        //   alphaMax:
        //     Maximum alpha [0..1].
        //
        // Returns:
        //     ///
        //     A random color with HSV and alpha values in the input ranges.
        //     ///
        public static Color ColorHSV(float hueMin, float hueMax, float saturationMin, float saturationMax) {
            return RandomUnity.ColorHSV(hueMin, hueMax, saturationMin, saturationMax);
        }
        //
        // Summary:
        //     ///
        //     Generates a random color from HSV and alpha ranges.
        //     ///
        //
        // Parameters:
        //   hueMin:
        //     Minimum hue [0..1].
        //
        //   hueMax:
        //     Maximum hue [0..1].
        //
        //   saturationMin:
        //     Minimum saturation [0..1].
        //
        //   saturationMax:
        //     Maximum saturation[0..1].
        //
        //   valueMin:
        //     Minimum value [0..1].
        //
        //   valueMax:
        //     Maximum value [0..1].
        //
        //   alphaMin:
        //     Minimum alpha [0..1].
        //
        //   alphaMax:
        //     Maximum alpha [0..1].
        //
        // Returns:
        //     ///
        //     A random color with HSV and alpha values in the input ranges.
        //     ///
        public static Color ColorHSV(float hueMin, float hueMax, float saturationMin, float saturationMax, float valueMin, float valueMax) {
            return RandomUnity.ColorHSV(hueMin, hueMax, saturationMin, saturationMax, valueMin, valueMax);
        }
        //
        // Summary:
        //     ///
        //     Generates a random color from HSV and alpha ranges.
        //     ///
        //
        // Parameters:
        //   hueMin:
        //     Minimum hue [0..1].
        //
        //   hueMax:
        //     Maximum hue [0..1].
        //
        //   saturationMin:
        //     Minimum saturation [0..1].
        //
        //   saturationMax:
        //     Maximum saturation[0..1].
        //
        //   valueMin:
        //     Minimum value [0..1].
        //
        //   valueMax:
        //     Maximum value [0..1].
        //
        //   alphaMin:
        //     Minimum alpha [0..1].
        //
        //   alphaMax:
        //     Maximum alpha [0..1].
        //
        // Returns:
        //     ///
        //     A random color with HSV and alpha values in the input ranges.
        //     ///
        public static Color ColorHSV(float hueMin, float hueMax, float saturationMin, float saturationMax, float valueMin, float valueMax, float alphaMin, float alphaMax) {
            return RandomUnity.ColorHSV(hueMin, hueMax, saturationMin, saturationMax, valueMin, valueMax, alphaMin, alphaMax);
        }
        //
        // Summary:
        //     ///
        //     Initializes the random number generator state with a seed.
        //     ///
        //
        // Parameters:
        //   seed:
        //     Seed used to initialize the random number generator.
        public static void InitState(int seed) {
            RandomUnity.InitState(seed);
        }
        //
        // Summary:
        //     ///
        //     Returns a random integer number between min [inclusive] and max [exclusive] (Read
        //     Only).
        //     ///
        //
        // Parameters:
        //   min:
        //
        //   max:
        public static int Range(int min, int max) {
            return RandomUnity.Range(min, max);
        }
        //
        // Summary:
        //     ///
        //     Returns a random float number between and min [inclusive] and max [inclusive]
        //     (Read Only).
        //     ///
        //
        // Parameters:
        //   min:
        //
        //   max:
        public static float Range(float min, float max) {
            return RandomUnity.Range(min, max);
        }
        #endregion
    }
}