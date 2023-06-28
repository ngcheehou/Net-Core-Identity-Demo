namespace WebApp1.Constant
{
    public class SpecialActionDictionary : Dictionary<string, string>
    {
        private static object _lock = new object();
        private static SpecialActionDictionary? _instance;

        private SpecialActionDictionary() { }

        public static SpecialActionDictionary Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new SpecialActionDictionary();
                        }
                    }
                }

                return _instance;
            }
        }
    }
}
