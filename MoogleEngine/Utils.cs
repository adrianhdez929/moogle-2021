using System.IO;

namespace Utils {
    public static class Utils {
        public static FileHandler[] LoadFiles() {
            string[] contentFiles = Directory.GetFiles("./Content");
            FileHandler[] files = new FileHandler[contentFiles.Length];

            for(int i = 0; i < files.Length; i++) {
                FileHandler newFile = new FileHandler(contentFiles[i]);
                files[i] = newFile;
            }

            return files;
        }
    }
}
