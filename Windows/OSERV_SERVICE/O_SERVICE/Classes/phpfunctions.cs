using System;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Security.AccessControl;
using System.Reflection;
using System.Collections.Generic;

namespace OSERV_BASE.Classes
{
    /// <summary>
    /// PHP Functions for c# by nom1@IRIS
    /// </summary>
    public static class PHP
    {
        /*
         
         "PHP FUNCTIONS" FOR C# by nom1@IRIS
         
         
          functions included:
          -------------------
         * strstr
         * stristr
         * trim
         * substr
         * file_get_contents
         * file_put_contents
         * str_replace
         * md5
         * strtoupper
         * strtolower
         * explode
         * implode
         
         extra functions:
         ----------------
         * alert [a JavaScript function]
         * get_version
         * ToString [overrided]
         */

        /// <summary>
        /// Returns part of haystack string from the first occurrence of needle to the end of haystack. (case-sensitive)
        /// </summary>
        /// <param name="haystack">The input string.</param>
        /// <param name="needle">The search string.</param>
        /// <returns>Returns the portion of string, or empty if needle is not found.</returns>
        public static string strstr(string haystack, string needle)
        {
            string returner = "";
            int hstrt = haystack.IndexOf(needle);
            int hlen = (haystack.Length - hstrt);
            try
            {
                returner = haystack.Substring(hstrt, hlen);
                return returner;
            }
            catch
            {
                return "";
            }

        }

        /// <summary>
        /// Returns part of haystack string from the first occurrence of needle to the end of haystack. (case-insensitive)
        /// </summary>
        /// <param name="haystack">The input string.</param>
        /// <param name="needle">The search string.</param>
        /// <returns>Returns the portion of string, or empty if needle is not found.</returns>
        public static string stristr(string haystack, string needle)
        {
            string returner = "";
            int hstrt = haystack.ToLower().IndexOf(needle.ToLower());
            int hlen = (haystack.Length - hstrt);
            try
            {
                returner = haystack.Substring(hstrt, hlen);
                return returner;
            }
            catch
            {
                return "";
            }

        }
        /// <summary>
        /// Strip whitespace (or other characters) from the beginning and end of a string
        /// </summary>
        /// <param name="str">The string that will be trimmed.</param>
        public static string trim(string str)
        {
            return str.Trim();
        }

        /// <summary>
        /// Sends an alert message as INFO in CLI
        /// </summary>
        /// <param name="str">Content</param>
        public static void alert(string str)
        {
            Dbg.LogEvent(str);
        }

        /// <summary>
        /// Returns the portion of string specified by the start and length parameters. 
        /// </summary>
        /// <param name="str">The input string. Must be one character or longer. </param>
        /// <param name="start">If start is non-negative, the returned string will start at the start'th position in string, counting from zero.</param>
        /// <param name="lenght">If length is given and is positive, the string returned will contain at most length characters beginning from start (depending on the length of string).</param>
        /// <returns>Returns the extracted part of string, or empty on failure or an empty string.</returns>
        public static string substr(string str, int start, int lenght)
        {
            try
            {
                if (str != "")
                {
                    return str.Substring(start, lenght);
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// Reads an entire file into an string List
        /// </summary>
        /// <param name="filename">Name of the file to read</param>
        /// <returns>A string list, line seperated</returns>
        public static List<string> file(string filename)
        {
            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                List<string> returner = new List<string>();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(filename))
                {
                    String line = string.Empty;
                    // Read and display lines from the file until the end of
                    // the file is reached.
                    while ((line = sr.ReadLine()) != null)
                    {
                        returner.Add(line);
                    }
                    sr.Close();
                    return returner;
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                alert("The file could not be read: " + e.Message);
                return new List<string>();
            }
        }

        /// <summary>
        /// Reads entire file into a string (Lines separated by \n)
        /// </summary>
        /// <param name="filename">Name of the file to read.</param>
        /// <returns>File contents as string, or messagebox with exception and return empty on failure.</returns>
        public static string file_get_contents(string filename)
        {
            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (System.IO.StreamReader sr = new System.IO.StreamReader(filename))
                {
                    String line = string.Empty;
                    String returner = "";
                    // Read and display lines from the file until the end of
                    // the file is reached.
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (returner != "")
                        {
                            returner += "\n";
                        }
                        returner += line;
                    }
                    sr.Close();
                    return returner;
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                alert("The file could not be read: " + e.Message);
                return "";
            }
        }


        /// <summary>
        /// Write a string to a file. If filename does not exist, the file is created. Otherwise, the existing file is overwritten.
        /// </summary>
        /// <param name="filename">Path to the file where to write the data.</param>
        /// <param name="content">The data to write</param>
        /// <returns>Returns true on success, or false and a messagebox with the error message on failure</returns>
        public static bool file_put_contents(string filename, string content)
        {
            try
            {
                System.IO.File.WriteAllText(filename, content);
                return true;
            }
            catch (Exception e)
            {
                alert("The file could note be written: " + e.Message);
                return false;
            }
        }

        /// <summary>
        /// Replace all occurrences of the search string with the replacement string
        /// </summary>
        /// <param name="find">The value being searched for.</param>
        /// <param name="replace">The replacement value that replaces found search value.</param>
        /// <param name="str">The string or array being searched and replaced on.</param>
        /// <returns>Returns a string with the replaced values.</returns>
        public static string str_replace(string find, string replace, string str)
        {
            return str.Replace(find, replace);
        }

        /// <summary>
        /// Creates a md5 hash of a string
        /// </summary>
        /// <param name="str_to_md5">The string to generate the md5 hash from.</param>
        /// <returns>MD5 hash (uppercase)</returns>
        public static string md5(string str_to_md5)
        {
            // Instantiate MD5CryptoServiceProvider, get bytes for original string and compute hash (encoded strin)g
            Byte[] originalBytes = ASCIIEncoding.Default.GetBytes(str_to_md5);
            Byte[] encodedBytes = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(originalBytes);
            // Convert encoded bytes back to a 'readable' string
            return trim(str_replace("-", "", BitConverter.ToString(encodedBytes)));
        }

        /// <summary>
        /// Converts a string to lowercase.
        /// </summary>
        /// <param name="str">String to convert to lowercase.</param>
        /// <returns>Lowercase string</returns>
        public static string strtolower(string str)
        {
            return str.ToLower();
        }

        /// <summary>
        /// Converts a string to uppercase.
        /// </summary>
        /// <param name="str">String to convert to uppercase</param>
        /// <returns>Uppercase string</returns>
        public static string strtoupper(string str)
        {
            return str.ToUpper();
        }

        /// <summary>
        /// Returns an array of strings, each of which is a substring of string formed by splitting it on boundaries formed by the string delimiter. 
        /// </summary>
        /// <param name="delimiter">The boundary string.</param>
        /// <param name="str">The input string.</param>
        /// <returns>Returns a string-array of strings created by splittin the string parameter on boundaries formed by the delimiter</returns>
        public static string[] explode(string delimiter, string str)
        {
            return System.Text.RegularExpressions.Regex.Split(str, delimiter);
        }

        /// <summary>
        /// Join array elements with a glue string.
        /// </summary>
        /// <param name="glue">Glue string.</param>
        /// <param name="pieces">The array of strings to implode.</param>
        /// <returns>Returns a string containing a string representation of all the array elements in the same order, with the glue string between each element.</returns>
        public static string implode(string glue, string[] pieces)
        {
            return string.Join(glue, pieces);
        }
        public static string implode(string glue, List<string> pieces)
        {
            return string.Join(glue, pieces.ToArray());
        }

        /// <summary>
        /// Returns the current PHPF-library version
        /// </summary>
        /// <returns>String, in format: 1.0.0.0</returns>
        public static string get_version()
        {
            return Application.ProductVersion;
        }

        /// <summary>
        /// Get full assembly name
        /// </summary>
        /// <returns>full name string</returns>
        public static string get_name()
        {
            return Assembly.GetEntryAssembly().FullName;
        }

        /*/// <summary>
        /// Get class credits string
        /// </summary>
        /// <returns>Returns credits</returns>
        public static override string ToString()
        {
            return "PHP-Functions class by nom1@IRIS";
        }*/
    }
}
