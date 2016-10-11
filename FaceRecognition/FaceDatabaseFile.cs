using Ionic.Zip;
using RealSenseSdkExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecognition {
    /// <summary>
    /// 臉部辨識檔案與名稱對應檔案ZIP壓縮
    /// </summary>
    public static class FaceDatabaseFile {
        /// <summary>
        /// 檔案儲存
        /// </summary>
        /// <param name="file">檔案路徑</param>
        /// <param name="faceData">臉部辨識資料</param>
        /// <param name="userTable">名稱對應資料</param>
        public static void Save(string file, RecognitionFaceData[] faceData,Dictionary<int,string> userTable) {
            FormatData(faceData, userTable);
            using (FileStream outputStream = new FileStream(file, FileMode.Create))
            using (ZipFile zip = new ZipFile()) {
                zip.AlternateEncodingUsage = ZipOption.Always;
                zip.AlternateEncoding = Encoding.UTF8;
                zip.Comment = string.Format(
                    Resource.ZipComment,
                    DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss"),
                    Guid.NewGuid()
                );                
                zip.AddEntry("UserTable.csv", UserTableToCSVBinary(userTable));
                zip.AddEntry("FaceData.bin", faceData.ToBinary());
                zip.Save(outputStream);
            }
        }

        public static byte[] UserTableToCSVBinary(Dictionary<int,string> userTable) {
            using (MemoryStream stream = new MemoryStream()) {
                StreamWriter writer = new StreamWriter(stream,Encoding.UTF8);
                var csvRow = userTable.ToArray().Select(x => $"{x.Key},{x.Value}");
                foreach(var row in csvRow) {
                    writer.WriteLine(row);
                }
                writer.Flush();
                return stream.ToArray();
            }
        }

        public static Dictionary<int,string> CSVBinaryToUserTable(byte[] binary) {
            using(MemoryStream stream = new MemoryStream(binary)) {
                StreamReader reader = new StreamReader(stream);
                string[] lines = reader.ReadToEnd().Split(new string[] { "\r\n" },StringSplitOptions.RemoveEmptyEntries);
                Dictionary<int, string> result = new Dictionary<int, string>();

                foreach(var line in lines) {
                    string[] rawCols = line.Split(new char[] { ',' }, 2);
                    if (rawCols.Length == 2) {
                        result[int.Parse(rawCols[0])] = rawCols[1];
                    }else {
                        result[int.Parse(rawCols[0])] = "";
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// 格式錯誤處理，校正可能重複的Face ID
        /// </summary>
        /// <param name="faceData"></param>
        /// <param name="userTable"></param>
        public static void FormatData(
            RecognitionFaceData[] faceData,
            Dictionary<int,string> userTable) {
            faceData = faceData.OrderBy(x => x.PrimaryKey)
                .Where(x => x.ForeignKey != -1)//清除未用到的圖片
                .Select((x, i) => { x.PrimaryKey = i; return x; })                
                .ToArray();
            
        }

        /// <summary>
        /// 檔案讀取
        /// </summary>
        /// <param name="file">檔案路徑</param>
        /// <param name="faceData">臉部辨識資料</param>
        /// <param name="userTable">名稱對應資料</param>
        public static void Load(string file, ref RecognitionFaceData[] faceData, ref Dictionary<int,string> userTable) {
            using (ZipFile zip = ZipFile.Read(file)) {
                var userTableReader = zip["UserTable.csv"].OpenReader();
                userTable = CSVBinaryToUserTable(
                    StreamToBytes(userTableReader));

                var faceDataReader = zip["FaceData.bin"].OpenReader();
                faceData = RecognitionFaceDataFile.FromBinary(StreamToBytes(faceDataReader));
            }
            FormatData(faceData,userTable);
        }

        public static byte[] StreamToBytes(Stream stream) {
            List<byte> buffer = new List<byte>();
            while (stream.Length != stream.Position) {
                buffer.Add((byte)stream.ReadByte());
            }
            return buffer.ToArray();
        }
    }
}
