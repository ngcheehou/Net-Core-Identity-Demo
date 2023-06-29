namespace WebApp1.Constant
{
    public class DemoUserClaimDictionary : Dictionary<string, string>
    {
        private static object _lock = new object();
        private static DemoUserClaimDictionary? _instance;

        private DemoUserClaimDictionary() { }

        public static DemoUserClaimDictionary Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DemoUserClaimDictionary();
                        }
                    }
                }

                return _instance;
            }
        }
    }
}
