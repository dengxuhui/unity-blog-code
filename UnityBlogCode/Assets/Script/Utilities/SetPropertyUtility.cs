namespace UnityEngine.UI.Extensions
{
    //属性设置
    internal static class SetPropertyUtility
    {
        
        /// <summary>
        /// 设置类
        /// </summary>
        /// <param name="currentValue"></param>
        /// <param name="newValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool SetClass<T>(ref T currentValue, T newValue) where T : class
        {
            if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
            {
                return false;
            }

            currentValue = newValue;
            return true;
        }
    }
}