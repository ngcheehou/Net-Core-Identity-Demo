namespace WebApp2.Constant
{
    public class PageNameDictionary
    : Dictionary<string, string>
    {
        private static object _lock = new object();
        private static PageNameDictionary? _instance;

        private PageNameDictionary() { }

        public static PageNameDictionary Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new PageNameDictionary();
                        }
                    }
                }

                return _instance;
            }
        }
    }
}
