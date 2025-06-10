using UnityEngine;
using UnityEngine.Assertions;

namespace ProjectPortfolio.Global
{
    public class Asserts
    {
        public static bool IsNull<T>(T p_value, string p_message = "No detailed message")
            where T : class
        {
            if (p_value != null)
#if DEVELOPMENT_BUILD || UNITY_EDITOR
                throw new AssertionException($"Value {p_value} was not null!", p_message);
#else
                Debug.LogError($"Value {p_value} was not null! User msg: {p_message}");
                return false;
#endif
            return true;
        }
        
        public static bool IsNotNull<T>(T p_value, string p_message = "No detailed message")
            where T : class
        {
            if (p_value == null)
#if DEVELOPMENT_BUILD || UNITY_EDITOR
                throw new AssertionException("Value was null!", p_message);
#else
                Debug.LogError($"Value was null! User msg: {p_message}");
                return false;
#endif
            return true;
        }
        
        public static bool IsTrue(bool p_value, string p_message = "No detailed message")
        {
            if (!p_value)
#if DEVELOPMENT_BUILD || UNITY_EDITOR
                throw new AssertionException("Value was false!", p_message);
#else
                Debug.LogError($"Value was false! User msg: {p_message}");
                return false;
#endif
            return true;
        }
        
        public static bool IsFalse(bool p_value, string p_message = "No detailed message")
        {
            if (p_value)
#if DEVELOPMENT_BUILD || UNITY_EDITOR
                throw new AssertionException("Value was true!", p_message);
#else
                Debug.LogError($"Value was true! User msg: {p_message}");
                return false;
#endif
            return true;
        }

    }
}