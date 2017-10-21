using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace firstdnet
{
    public class FileList
    {
        static string[] ExtArray = { ".mp4", ".mkv", ".mov", ".flv", ".mpg", ".avi", ".wmv",
                              ".mp3", ".ac3", ".wav", ".wma", ".flac"};
        /// <summary>
        /// ファイルリスト次ファイル検索
        /// </summary>
        /// <param name="fileList">ファイルリスト</param>
        /// <param name="fileName">もととなるファイル</param>
        /// <returns></returns>
        public static string GetNextFile(List<string> fileList, string fileName, int beforeNext = 1)
        {
            var res = fileList.FindIndex(file =>
                file == fileName
            ) + beforeNext;
            if (res < fileList.Count && res >= 0)
            {
                return(fileList[res]);
            }
            return string.Empty;
        }

        /// <summary>
        /// ファイル一覧取得
        /// </summary>
        /// <remarks>
        /// ディレクトリから、ファイル一覧を作成する
        /// </remarks>
        /// <param name="fileName">基となるファイル</param>
        public static async Task<List<string>> GetFileListAsync(string fileName)
        {
            // 検索ディレクトリ＝カレントディレクトリ
            string currDir = Path.GetDirectoryName(fileName);
            // EnumerateFilesは単一のパターンのみしか指定できないため、
            // とりあえず全部取得し、Whereで絞り込む
            string searchPattern = "*";
            // ファイルリストの取得
            // LINQでファイルの絞り込み(LINQなら複雑な処理を記述しなくて済む)
            // さらに、別途ソートしていたのをLINQでナチュラルソートするように変更
            // List化も別途から一緒にするように変更した
            return await Task.Run(() =>
            {
                return Directory.EnumerateFiles(currDir, searchPattern)
                .Where(file =>
                {
                    // 拡張子の取得
                    string ext = Path.GetExtension(file).ToLower();
                    bool rtc = false;
                    if (ExtArray.Contains(ext))
                    {
                        rtc = true;
                    }
                    return rtc;
                }).OrderBy(x => x, new StrNatComparer()).ToList();
            });
        }
    }
}
