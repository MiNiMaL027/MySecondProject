using List_Domain.Exeptions;

namespace List_Service.Services.ValidOptions
{
    public class ValidOptions
    {
        /// <summary>
        /// Check if the name meets the rules(<=3 and >=20) 
        /// </summary>
        /// <param name="name"></param>
        /// <returns>True or False</returns>
        private static bool ValidName(string name)
        {
            if (name.Length <= 3 && name.Length >= 20)
                return false;
            return true;
        }

        public static string ValidNameCreateModel(string Name)
        {
            if (Name == null)
                throw new ValidationException("Null reference");

            Name = Name.Trim();

            if (!ValidName(Name))
                throw new ValidationException($"{Name} - Not valide");

            return Name;       
        }
    }
}
