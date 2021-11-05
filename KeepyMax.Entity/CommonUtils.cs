namespace KeepyMax.Model
{
    using System;
    public class CommonUtils
    {
        public const String defaultCadena = "";
        public const int defaultEntero = 0;
        public const Boolean defaultBoolean = false;
        public const Decimal defaultDecimal = 0;
        public const Double defaultDouble = 0;
        public const float defaultFloat = 0;


        public static Object validaCampo(Object objeto, Object omision)
        {
            return (objeto == DBNull.Value ? omision : objeto);
        }
    }
}
