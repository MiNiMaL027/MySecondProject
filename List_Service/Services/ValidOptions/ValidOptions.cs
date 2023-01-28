namespace List_Service.Services.ValidOptions
{
    public class ValidOptions
    {
        /// <summary>
        ///  Will check if the name meets the rules(<=3 and >=20)
        /// </summary>
        /// <param name="name"></param>
        /// <returns>True or False</returns>
        public static bool ValidName(string name)
        {
            if (name.Length <= 3)
                return false;
            if (name.Length >= 20)
                return false;
            return true;
        }
    }
}
