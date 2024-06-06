namespace App.Constants
{
    public static class ConstValues
    {
        private static IDictionary<string, object> CONST_VALUES = new Dictionary<string, object>();

        public static void PutValue<T>(string key, T value) where T : class
            => CONST_VALUES[key] = value;

        public static T GetValue<T>(string key) where T : class
            => CONST_VALUES[key] as T;
    }
}
