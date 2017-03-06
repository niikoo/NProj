using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace OSERV_BASE.Classes
{
    public static class DirectoryFunctions
    {
        #region WIN32API FUNCTIONS
        private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct WIN32_FIND_DATA
        {
            public uint dwFileAttributes;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
            public uint nFileSizeHigh;
            public uint nFileSizeLow;
            public uint dwReserved0;
            public uint dwReserved1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string cFileName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
            public string cAlternateFileName;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr FindFirstFile(string lpFileName, out WIN32_FIND_DATA lpFindFileData);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool FindNextFile(IntPtr hFindFile, out WIN32_FIND_DATA lpFindFileData);

        [DllImport("kernel32.dll")]
        private static extern bool FindClose(IntPtr hFindFile);
        #endregion

        /// <summary>
        /// Recursive copy of a directory
        /// </summary>
        /// <param name="source">Source directory</param>
        /// <param name="target">Target directory</param>
        /// <param name="overwrite">Overwrite files? (bool), default: true</param>
        public static void CopyAll(DirectoryInfo source, DirectoryInfo target, bool overwrite = true)
        {
            // Check if the target directory exists, if not, create it.
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it’s new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name),overwrite);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
        /// <summary>
        /// Directory Info
        /// </summary>
        /// <param name="DirectoryPath">Directory Path</param>
        /// <returns>DirectoryInfo</returns>
        public static DirectoryInfo Info(string DirectoryPath)
        {
            return new DirectoryInfo(DirectoryPath);
        }
        /// <summary>
        /// Delete files by RegEx filter (NOT RECURSIVELY)
        /// </summary>
        /// <param name="folder">Folder to search for files by RegEx</param>
        /// <param name="RegEx">RegEx filter to use</param>
        public static void DeleteFilesRegex(DirectoryInfo folder, System.Text.RegularExpressions.Regex RegEx)
        {
            if (!IsDirectoryEmpty(folder.FullName))
            {
                foreach (FileInfo fi in folder.GetFiles())
                {
                    if (RegEx.Match(fi.Name).Success)
                    {
                        System.IO.File.Delete(fi.FullName);
                    }
                }
            }
        }

        public static void DeleteEmptyFilesRecursively(DirectoryInfo root)
        {
            FileInfo[] files = root.GetFiles("*.*", SearchOption.AllDirectories);
            List<FileInfo> emptyFiles = new List<FileInfo>();
            foreach (FileInfo file in files)
            {
                using (var r = new StreamReader(file.OpenRead()))
                {
                    string content = r.ReadToEnd();
                    if (string.IsNullOrEmpty(content))
                    {
                        emptyFiles.Add(file);
                    }
                }
            }
            foreach(FileInfo emptyFile in emptyFiles) {
                emptyFile.Delete();
            }
        }

        /// <summary>
        /// Deletes empty directories inside specified directory, recursively.
        /// </summary>
        /// <param name="root">Root directory (DirectoryInfo)</param>
        public static void DeleteEmptyDirectoriesRecursively(DirectoryInfo root)
        {
            DirectoryInfo[] subDirs = null;
            if (IsDirectoryEmpty(root.FullName))
                root.Delete();
            else
                subDirs = root.GetDirectories();
            if (subDirs == null)
                return;
            foreach (var dirInfo in subDirs)
                DeleteEmptyDirectoriesRecursively(dirInfo);

        }
        /// <summary>
        /// Check if the current directory is empty
        /// </summary>
        /// <param name="path">Path to directory (string)</param>
        /// <returns></returns>
        public static bool IsDirectoryEmpty(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(path);
            }
            if (Directory.Exists(path))
            {
                if (path.EndsWith(Path.DirectorySeparatorChar.ToString()))
                    path += "*";
                else
                    path += Path.DirectorySeparatorChar + "*";
                WIN32_FIND_DATA findData;
                var findHandle = FindFirstFile(path, out findData);

                if (findHandle != INVALID_HANDLE_VALUE)
                {
                    try
                    {
                        bool empty = true;
                        do
                        {
                            if (findData.cFileName != "." && findData.cFileName != "..")
                            {
                                empty = false;
                            }
                        } while (empty && FindNextFile(findHandle, out findData));
                        return empty;
                    }
                    finally
                    {
                        FindClose(findHandle);
                    }
                }
                throw new Exception("Failed to get directory first file",
                                    Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error()));
            }
            else
            {
                throw new DirectoryNotFoundException();

            }
        }
        /// <summary>
        /// System.IO.File.Delete does not support wildcards in the filename. 
        /// So this function calls Directory.GetFiles which does support wildcards and then calls File.Delete on each file that is found
        /// </summary>
        /// <param name="DirectoryPath">Path to the directory where the files are</param>
        /// <param name="namePattern">Filename, with wildcard</param>
        /// <returns></returns>
        public static void DeleteFilesByWildcard(string DirectoryPath, string namePattern) {
            try
            {
                foreach (string f in Directory.GetFiles(DirectoryPath,namePattern))
                {
                    File.Delete(f);
                }
            }
            catch(Exception ex) {
                Dbg.LogEvent(ex.Message, System.Diagnostics.EventLogEntryType.Error, false);
            }
        }
    }

    // http://www.codeproject.com/KB/cs/ScanDirectory.aspx //
    /// <summary>
    /// Defines the action on a directory which triggered the event
    /// </summary>
    public enum ScanDirectoryAction
    {
        /// <summary>
        /// Enter a directory
        /// </summary>
        Enter,

        /// <summary>
        /// Leave a directory
        /// </summary>
        Leave
    }

    #region Event argument definition for ScanDirectory.FileEvent

    /// <summary>
    /// Information about the file in the current directory.
    /// </summary>
    public class FileEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Block the default constructor.
        /// </summary>
        private FileEventArgs() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryEventArgs"/> class.
        /// </summary>
        /// <param name="fileInfo"><see cref="FileInfo"/> object for the current file.</param>
        internal FileEventArgs(FileInfo fileInfo)
        {
            if (fileInfo == null) throw new ArgumentNullException("fileInfo");

            // Get File information 
            _fileInfo = fileInfo;
        }

        #endregion

        #region Properties

        private bool _cancel;
        private FileInfo _fileInfo;

        /// <summary>
        /// Gets the current file information.
        /// </summary>
        /// <value>The <see cref="FileInfo"/> object for the current file.</value>
        public FileInfo Info
        {
            get { return _fileInfo; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to cancel the directory scan.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the scan must be cancelled; otherwise, <see langword="false"/>.
        /// </value>
        public bool Cancel
        {
            get { return _cancel; }
            set { _cancel = value; }
        }

        #endregion
    }

    #endregion

    #region Event argument definition for ScanDirectory.DirectoryEvent

    /// <summary>
    /// Event arguments for the DirectoryEvent
    /// </summary>
    public class DirectoryEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Block the default constructor.
        /// </summary>
        private DirectoryEventArgs() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryEventArgs"/> class.
        /// </summary>
        /// <param name="directory"><see cref="DirectoryInfo"/> object for the current path.</param>
        /// <param name="action">The action.</param>
        internal DirectoryEventArgs(DirectoryInfo directory, ScanDirectoryAction action)
        {
            if (directory == null) throw new ArgumentNullException("directory");

            // Get File information 
            _directoryInfo = directory;
            _action = action;
        }

        #endregion

        #region Properties

        private DirectoryInfo _directoryInfo;
        private ScanDirectoryAction _action;
        private bool _cancel;

        /// <summary>
        /// Gets the current directory information.
        /// </summary>
        /// <value>The <see cref="DirectoryInfo"/> object for the current directory.</value>
        public DirectoryInfo Info
        {
            get { return _directoryInfo; }
        }

        /// <summary>
        /// Gets the current directory action.
        /// </summary>
        /// <value>The <see cref="ScanDirectoryAction"/> action value.</value>
        public ScanDirectoryAction Action
        {
            get { return _action; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to cancel the directory scan.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the scan must be cancelled; otherwise, <see langword="false"/>.
        /// </value>
        public bool Cancel
        {
            get { return _cancel; }
            set { _cancel = value; }
        }

        #endregion
    }

    #endregion

    /// <summary>
    /// Scan directory trees
    /// </summary>
    public class ScanDirectory
    {
        private const string _patternAllFiles = "*.*";

        #region Handling of the FileEvent

        /// <summary>
        /// Definition for the FileEvent.
        ///	</summary>
        public delegate void FileEventHandler(object sender, FileEventArgs e);

        /// <summary>
        /// Event is raised for each file in a directory.
        /// </summary>
        public event FileEventHandler FileEvent;

        /// <summary>
        /// Raises the file event.
        /// </summary>
        /// <param name="fileInfo"><see cref="FileInfo"/> object for the current file.</param>
        private bool RaiseFileEvent(FileInfo fileInfo)
        {
            bool continueScan = true;

            // Create a new argument object for the file event.
            FileEventArgs args = new FileEventArgs(fileInfo);

            // Now raise the event.
            FileEvent(this, args);

            continueScan = !args.Cancel;

            return continueScan;
        }

        #endregion

        #region Handling of the DirectoryEvent

        /// <summary>
        /// Definition for the DirectoryEvent.
        /// </summary>
        public delegate void DirectoryEventHandler(object sender, DirectoryEventArgs e);

        /// <summary>
        /// Event is raised for each directory.
        /// </summary>
        public event DirectoryEventHandler DirectoryEvent;

        /// <summary>
        /// Raises the directory event.
        /// </summary>
        /// <param name="directory"><see cref="DirectoryInfo"/> object for the current path.</param>
        /// <param name="action">The <see cref="ScanDirectoryAction"/> action value.</param>
        /// <returns><see langword="true"/> when the scan is allowed to continue. <see langword="false"/> if otherwise;</returns>
        private bool RaiseDirectoryEvent(DirectoryInfo directory, ScanDirectoryAction action)
        {
            bool continueScan = true;

            // Only do something when the event has been declared.
            if (FileEvent != null)
            {
                // Create a new argument object for the file event.
                DirectoryEventArgs args = new DirectoryEventArgs(directory, action);

                // Now raise the event.
                DirectoryEvent(this, args);

                continueScan = !args.Cancel;
            }
            return continueScan;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Walks the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns><see langword="true"/> when the scan finished without being interupted. <see langword="false"/> if otherwise;</returns>
        public bool WalkDirectory(string path)
        {
            // Validate path argument.
            if (path == null || path.Length == 0) throw new ArgumentNullException("path");

            return this.WalkDirectory(new DirectoryInfo(path));
        }

        /// <summary>
        /// Walks the specified directory.
        /// </summary>
        /// <param name="directory"><see cref="DirectoryInfo"/> object for the current path.</param>
        /// <returns><see langword="true"/> when the scan finished without being interupted. <see langword="false"/> if otherwise;</returns>
        public bool WalkDirectory(DirectoryInfo directory)
        {
            if (directory == null)
            {
                throw new ArgumentNullException("directory");
            }

            return this.WalkDirectories(directory);
        }

        #endregion

        #region Overridable methods

        /// <summary>
        /// Processes the directory.
        /// </summary>
        /// <param name="directoryInfo">The directory info.</param>
        /// <param name="action">The action.</param>
        /// <returns><see langword="true"/> when the scan is allowed to continue. <see langword="false"/> if otherwise;</returns>
        public virtual bool ProcessDirectory(DirectoryInfo directoryInfo, ScanDirectoryAction action)
        {
            if (DirectoryEvent != null)
            {
                return RaiseDirectoryEvent(directoryInfo, action);
            }
            return true;
        }

        /// <summary>
        /// Processes the file.
        /// </summary>
        /// <param name="fileInfo">The file info.</param>
        /// <returns><see langword="true"/> when the scan is allowed to continue. <see langword="false"/> if otherwise;</returns>
        public virtual bool ProcessFile(FileInfo fileInfo)
        {
            // Only do something when the event has been declared.
            if (FileEvent != null)
            {
                RaiseFileEvent(fileInfo);
            }
            return true;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Walks the directory tree starting at the specified directory.
        /// </summary>
        /// <param name="directory"><see cref="DirectoryInfo"/> object for the current directory.</param>
        /// <returns><see langword="true"/> when the scan is allowed to continue. <see langword="false"/> if otherwise;</returns>
        private bool WalkDirectories(DirectoryInfo directory)
        {
            bool continueScan = true;

            if (continueScan = this.ProcessDirectory(directory, ScanDirectoryAction.Enter))
            {
                // Only scan the files in this path when a file event was specified 
                if (this.FileEvent != null)
                {
                    continueScan = WalkFilesInDirectory(directory);
                }

                if (continueScan)
                {
                    DirectoryInfo[] subDirectories = directory.GetDirectories();

                    foreach (DirectoryInfo subDirectory in subDirectories)
                    {
                        // It is possible that users create a recursive directory by mounting a drive
                        // into an existing directory on that same drive. If so, the attributes
                        // will have the ReparsePoint flag active. The directory is then skipped.
                        // See: http://blogs.msdn.com/oldnewthing/archive/2004/12/27/332704.aspx
                        if ((subDirectory.Attributes & FileAttributes.ReparsePoint) != 0)
                        {
                            continue;
                        }

                        if (!(continueScan = this.WalkDirectory(subDirectory)))
                        {
                            break;
                        }
                    }
                }

                if (continueScan)
                {
                    continueScan = this.ProcessDirectory(directory, ScanDirectoryAction.Leave);
                }
            }
            return continueScan;
        }

        /// <summary>
        /// Walks the directory tree starting at the specified path.
        /// </summary>
        /// <param name="directory"><see cref="DirectoryInfo"/> object for the current path.</param>
        /// <returns><see langword="true"/> when the scan was cancelled. <see langword="false"/> if otherwise;</returns>
        private bool WalkFilesInDirectory(DirectoryInfo directory)
        {
            bool continueScan = true;

            // Break up the search pattern in separate patterns
            string[] searchPatterns = _searchPattern.Split(';');

            // Try to find files for each search pattern
            foreach (string searchPattern in searchPatterns)
            {
                if (!continueScan)
                {
                    break;
                }
                // Scan all files in the current path
                foreach (FileInfo file in directory.GetFiles(searchPattern))
                {
                    if (!(continueScan = this.ProcessFile(file)))
                    {
                        break;
                    }
                }
            }
            return continueScan;
        }

        #endregion

        #region Properties

        private string _searchPattern;

        /// <summary>
        /// Gets or sets the search pattern.
        /// </summary>
        /// <example>
        /// You can specify more than one seach pattern
        /// </example>
        /// <value>The search pattern.</value>
        public string SearchPattern
        {
            get { return _searchPattern; }
            set
            {
                // When an empty value is specified, the search pattern will be the default (= *.*)
                if (value == null || value.Trim().Length == 0)
                {
                    _searchPattern = _patternAllFiles;
                }
                else
                {
                    _searchPattern = value;
                    // make sure the pattern does not end with a semi-colon
                    _searchPattern = _searchPattern.TrimEnd(new char[] { ';' });
                }
            }
        }

        #endregion
    }
}
