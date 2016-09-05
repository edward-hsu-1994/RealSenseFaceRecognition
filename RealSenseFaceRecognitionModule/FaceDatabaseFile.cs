using Ionic.Zip;
using RealSenseSdkExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DF_FaceTracking.cs {
    /// <summary>
    /// 臉部辨識檔案與名稱對應檔案ZIP壓縮
    /// </summary>
    public static class FaceDatabaseFile {
        /// <summary>
        /// 檔案儲存
        /// </summary>
        /// <param name="file">檔案路徑</param>
        /// <param name="list">臉部辨識資料</param>
        /// <param name="mapping">名稱對應資料</param>
        public static void Save(string file,List<RecognitionFaceData> list, List<NameMapping> mapping) {
            using (FileStream outputStream = new FileStream(file, FileMode.Create))
            using (ZipFile zip = new ZipFile()) {
                zip.Comment =
                    "RealSenseFaceRecognition Database Files\r\n" +
                    "UTC Time: " + DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss") + "\r\n" +
                    "Id: " + Guid.NewGuid();

                zip.AddEntry("NameMapping.bin", NameMapping.ToBinary(mapping.ToArray()));
                zip.AddEntry("FaceData.bin", list.ToArray().ToBinary());
                zip.Save(outputStream);
            }
        }

        /// <summary>
        /// 檔案讀取
        /// </summary>
        /// <param name="file">檔案路徑</param>
        /// <param name="list">臉部辨識資料</param>
        /// <param name="mapping">名稱對應資料</param>
        public static void Load(string file,ref List<RecognitionFaceData> list,ref List<NameMapping> mapping) {
            using (ZipFile zip = ZipFile.Read(file)) {
                var nameMappingReader = zip["NameMapping.bin"].OpenReader();
                mapping = NameMapping.FromBinary(StreamToBytes(nameMappingReader)).ToList();

                var faceDataReader = zip["FaceData.bin"].OpenReader();

                list = RecognitionFaceDataFile.FromBinary(StreamToBytes(faceDataReader)).ToList();
            }
        }

        private static byte[] StreamToBytes(Stream stream) {
            List<byte> buffer = new List<byte>();
            while(stream.Length != stream.Position) {
                buffer.Add((byte)stream.ReadByte());
            }
            return buffer.ToArray();
        }
    }
}
