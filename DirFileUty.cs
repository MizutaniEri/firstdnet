//
// なぜか標準の.netに存在しないリネームのメソッドがあったので、
// ファイルバージョンとディレクトリバージョンの追加
// Microsoft.VisualBasic.FileIO.FileSystemに似たようなものがあったが、
// 変更後の名前がわからず、未使用に。
// 2011.06.04 追記：Microsoft.VisualBasic.FileIO.FileSystem.RenameFileは
// 大文字小文字変換では使用できないことが判明。結構使えない…
//
using System;
using System.IO;

namespace DirFileUty
{
    public class FileUty
    {
        /// <summary>
        /// ファイル名の変更（移動はしない）
        /// </summary>
        /// <param name="src">変更前ファイル名(相対／絶対パスであること)</param>
        /// <param name="dest">変更後ファイル名（ファイル名のみであること）</param>
        /// <returns>変更後ファイル名</returns>
        public static String Rename(String src, String dest)
        {
            FileInfo fi = new FileInfo(src);
            String dir = Path.GetDirectoryName(fi.FullName);
            String fileDestFullName = Path.Combine(dir, dest);
            File.Move(fi.FullName, fileDestFullName);
            return (fileDestFullName);
        }
    }

    class DirUty
    {
        /// <summary>
        /// ディレクトリ名の変更（移動はしない）
        /// </summary>
        /// <param name="src">変更前ディレクトリ名(相対／絶対パスであること)</param>
        /// <param name="dest">変更後ディレクトリ名（変更するディレクトリ名のみであること）</param>
        /// <returns>変更後ディレクトリ名</returns>
        public static String Rename(String src, String dest)
        {
            FileInfo fi = new FileInfo(src);
            String pearentDir = Path.GetDirectoryName(fi.FullName);
            String destDir = Path.Combine(pearentDir, dest);
            Directory.Move(fi.FullName, destDir);
            return (destDir);
        }
    }
}