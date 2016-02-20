//
// なぜか標準の.netに存在しないリネームのメソッドがあったので、
// ファイルバージョンとディレクトリバージョンの追加
// Microsoft.VisualBasic.FileIO.FileSystemに似たようなものがあったが、
// 変更後の名前がわからず、未使用に。
// 2011.06.04 追記：Microsoft.VisualBasic.FileIO.FileSystem.RenameFileは
// 大文字小文字変換では使用できないことが判明。結構使えない…
//
// リネーム系2メソッドを拡張メソッドに変更した
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Utility
{
    public static class FileUty
    {
        /// <summary>
        /// ファイル名の変更（移動はしない）
        /// </summary>
        /// <remarks>
        /// 拡張メソッドに変更(第一引数にthisを付けただけ)
        /// </remarks>
        /// <param name="src">変更前ファイル名(相対／絶対パスであること)</param>
        /// <param name="dest">変更後ファイル名（ファイル名のみであること）</param>
        /// <returns>変更後ファイル名</returns>
        public static String FileRename(this String src, String dest)
        {
            FileInfo fi = new FileInfo(src);
            String dir = Path.GetDirectoryName(fi.FullName);
            String fileDestFullName = Path.Combine(dir, dest);
            File.Move(fi.FullName, fileDestFullName);
            return (fileDestFullName);
        }
    }

    public static class DirUty
    {
        /// <summary>
        /// ディレクトリ名の変更（移動はしない）
        /// </summary>
        /// <remarks>拡張メソッドに変更(第一引数にthisを付けただけ)</remarks>
        /// <param name="src">変更前ディレクトリ名(相対／絶対パスであること)</param>
        /// <param name="dest">変更後ディレクトリ名（変更するディレクトリ名のみであること）</param>
        /// <returns>変更後ディレクトリ名</returns>
        public static String DirRename(this String src, String dest)
        {
            FileInfo fi = new FileInfo(src);
            String pearentDir = Path.GetDirectoryName(fi.FullName);
            String destDir = Path.Combine(pearentDir, dest);
            Directory.Move(fi.FullName, destDir);
            return (destDir);
        }
    }

    public static class TraceDebug
    {
        public static void WriteLine(this string message,
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0,
            [CallerMemberName] string member = "")
        {
            var s = string.Format("{0}:{1} - {2}: {3}", file, line, member, message);
            Console.WriteLine(s);
        }
    }
}