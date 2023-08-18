
using UnityEngine;
namespace Helpers
{
    public static class Helper
    {
        private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0f,45f,0f));

        public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
        
        public static void TrimActionName(string targetSubstring,string originalString)
        {
            int startIndex = originalString.IndexOf(targetSubstring);
        
            if (startIndex != -1)
            {
                originalString = originalString.Substring(startIndex, targetSubstring.Length);
            }
            else
            {
                Debug.LogWarning("Target substring not found in the original string.");
            }

            Debug.Log("Resulting string: " + originalString);
        }
    }
 
}
