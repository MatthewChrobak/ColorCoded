namespace CSharp
{
    public static class Extensions
    {
        public static string SafeSubstring(this string input, int index, int length) {
            // Make sure that the starting index is non-negative.
            if (index < 0) {
                index = 0;
            }

            // If the length is bigger than the input length, try one less length.
            // Otherwise, return the substring.
            if (index + length > input.Length) {
                return input.SafeSubstring(index, length - 1);
            } else {
                return input.Substring(index, length);
            }
        }
    }
}
