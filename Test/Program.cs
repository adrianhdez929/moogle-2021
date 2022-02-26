using MoogleEngine.Utils;

namespace Test 
{
    public class Program
    {
        public static void Main(string[] args)
        {
            TestFileLoad();
        }

        #region Utils related Test Cases
        public static void TestFileLoad() {
            string[] filesLoaded = MoogleEngine.Utils.Utils.LoadFiles();

            if (!filesLoaded.Contains("./Content/Perros.txt"))
                throw new FileLoadException("Must contain Perros.txt");
        }
        #endregion

        #region Matrix related Test Cases
        #endregion
        
        #region Moogle related Test Cases
        #endregion
    }
}
