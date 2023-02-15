namespace List_Service.Services.ValidOptions
{
    public class ValidOptions
    {
        /// <summary>
        /// Check if the name meets the rules(<=3 and >=20) 
        /// </summary>
        /// <param name="name"></param>
        /// <returns>True or False</returns>
        public static bool ValidName(string name)
        {
            if (name.Length <= 3 && name.Length >= 20)
                return false;
            return true;
        }
    }
}
