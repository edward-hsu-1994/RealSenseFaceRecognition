using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace DF_FaceTracking.cs {
    public class NameMappingFile {
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
        /// <returns>名稱對應集合</returns>
        public static List<NameMappingFile> Load(string file) {
            using (Stream stream = File.Open(file, FileMode.Open)) {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (List<NameMappingFile>)binaryFormatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// 儲存檔案
        /// </summary>
        /// <param name="file">路徑</param>
        /// <param name="mappingCollection">名稱對應集合</param>
        public static void Save(string file,List<NameMappingFile> mappingCollection) {
            using (Stream stream = File.Open(file, FileMode.Create)) {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, mappingCollection);
            }
        }
    }
}
