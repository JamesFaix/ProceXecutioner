using System;
using System.Collections.Generic;
using System.Text;

namespace ProceXecutioner {

    /// <summary>Extension methods for common .NET types. </summary>
    static class ExtensionMethods {

        /// <summary>Splits a string, using the given delimiter and options. </summary>
        /// <param name="str">The string.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="options">The options.</param>
        /// <returns>Array of strings.</returns>
        /// <exception cref="ArgumentNullException">str and delimiter cannot be null.</exception>
        public static string[] Split(this string str, string delimiter, StringSplitOptions options = StringSplitOptions.None) {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));
            return str.Split(new[] { delimiter }, options);
        }

        /// <summary>Returns a string containing a string representation of each object in the 
        /// given sequence, separated by the given delimiter. </summary>
        /// <param name="sequence">The sequence.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <exception cref="ArgumentNullException">sequence and delimiter cannot be null.</exception>
        /// <exception cref="ArgumentException">Sequence cannot contain nulls.</exception>
        public static string ToDelimitedString(this IEnumerable<string> sequence, string delimiter) {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));
            if (delimiter == null) throw new ArgumentNullException(nameof(delimiter));

            var sb = new StringBuilder();
            bool skip = true; //Skips inserting delimiter before first word in sequence

            foreach (var word in sequence) {
                if (word == null) throw new ArgumentException("Sequence cannot contain nulls.", "sequence");

                if (skip) { skip = false; }
                else { sb.Append(delimiter); }

                sb.Append(word);
            }
            return sb.ToString();
        }

        /// <summary>Creates a new HashSet containing the unique elements from the given sequence.
        /// This will only keep the first of any duplicate elements. </summary>
        /// <typeparam name="T">Type of sequence.</typeparam>
        /// <param name="sequence">The sequence.</param>
        /// <exception cref="ArgumentNullException">sequence cannot be null.</exception>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> sequence) {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));
            return new HashSet<T>(sequence);
        }
    }
}
