using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceRecognition {
    public class FaceRecognitionEventArgs {
        public PXCMFaceData.Face[] Faces { get; set; }
        public PXCMFaceData.RecognitionData FirstRecognition { get; set; }
        public PXCMFaceData Output { get; set; }
        public Bitmap Image { get; set; }
    }
}
