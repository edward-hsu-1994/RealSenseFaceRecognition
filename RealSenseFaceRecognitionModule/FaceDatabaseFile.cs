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
            FormatData(list, mapping);
            using (FileStream outputStream = new FileStream(file, FileMode.Create))
            using (ZipFile zip = new ZipFile()) {
                zip.Comment =
                    "RealSenseFaceRecognition Database Files\r\n" +
                    "UTC Time: " + DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss") + "\r\n" +
                    "Guid: " + Guid.NewGuid();

                zip.AddEntry("NameMapping.bin", NameMapping.ToBinary(mapping.ToArray()));
                zip.AddEntry("FaceData.bin", list.ToArray().ToBinary());
                zip.Save(outputStream);
            }
        }

        /// <summary>
        /// 格式錯誤處理，校正可能重複的Face ID
        /// </summary>
        /// <param name="list"></param>
        /// <param name="mapping"></param>
        public static void FormatData(
            List<RecognitionFaceData> list,
            List<NameMapping> mapping) {


            #region remove not used
            //沒用到的圖像
            foreach (var item in list.ToArray()) {
                if (!mapping.Any(x => x.DataIds.Contains(item.Id))) {
                    list.Remove(item);
                }
            }

            //無用的ID
            foreach(var user in mapping) {
                user.DataIds = user.DataIds
                    .Where(x => list.Select(y => y.Id).Contains(x))
                    .ToList();
            }
            #endregion

            list = list.OrderBy(x => x.Index).Select((x, i) => { x.Index = i; return x; }).ToList();

            List<KeyValuePair<int, int>> oldAndNewId = new List<KeyValuePair<int, int>>();
            for(int i = 0; i < list.Count; i++) {
                var item = list[i];
                int NewID = item.Index + 100;
                oldAndNewId.Add(new KeyValuePair<int, int>(item.Id, NewID));
                item.Id = NewID;
                list[i] = item;
            }
            
            foreach(var rep in oldAndNewId) {
                foreach(var user in mapping) {
                    if (user.DataIds.Remove(rep.Key)) {
                        user.DataIds.Add(rep.Value);
                    }                    
                }
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
            FormatData(list, mapping);
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
