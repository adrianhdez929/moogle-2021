using System.IO;

namespace Utils {
    public class FileHandler {
        public FileHandler(string location) {
            StreamReader reader = new StreamReader(location);
            
            this.Location = location;
            this.Content = reader.ReadToEnd();
        }

        public string Location { get; private set; }

        public string Content { get; private set; }
    }
}
