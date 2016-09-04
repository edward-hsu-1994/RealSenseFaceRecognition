using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace DF_FaceTracking.cs {
    public class NameMapping {
        /// <summary>
        /// 唯一識別
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// RealSense Id列表
        /// </summary>
        public List<int> DataIds { get; set; } = new List<int>();

        /// <summary>
        /// 讀取檔案
        /// </summary>
        /// <param name="file">路徑</param>
        /// <returns>名稱對應陣列</returns>
        public static NameMapping[] Load(string file) {
            using (Stream stream = File.Open(file, FileMode.Open)) {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (NameMapping[])binaryFormatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// 自二進制陣列轉換為名稱對應檔案
        /// </summary>
        /// <param name="binary">二進制陣列</param>
        /// <returns>名稱對應陣列</returns>
        public static NameMapping[] FromBinary(byte[] binary) {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            MemoryStream stream = new MemoryStream(binary);
            return (NameMapping[])binaryFormatter.Deserialize(stream);
        }

        /// <summary>
        /// 儲存檔案
        /// </summary>
        /// <param name="file">路徑</param>
        /// <param name="mappingCollection">名稱對應集合</param>
        public static void Save(string file,NameMapping[] mappingCollection) {
            using (Stream stream = File.Open(file, FileMode.Create)) {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, mappingCollection);
            }
        }

        /// <summary>
        /// 將名稱對應陣列轉換為二進制陣列
        /// </summary>
        /// <param name="mappingCollection">名稱對應集合</param>
        /// <returns>二進制陣列</returns>
        public static byte[] ToBinary(NameMapping[] mappingCollection) {
            using (MemoryStream stream = new MemoryStream()) {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, mappingCollection);
                return stream.ToArray();
            }
        }
    }
}
