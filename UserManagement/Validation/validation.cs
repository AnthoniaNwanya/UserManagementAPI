namespace UserManagement.Validation
{
    public class validation
    {
        public string password { get; set; }
        public bool IsValidPassword(string password)
        {

            HashSet<char> specialCharacters = new HashSet<char>() { '%', '$', '#', '@', '.', '&' };
            if (password.Any(char.IsLower) &&
                 password.Any(char.IsUpper) &&
                 password.Any(char.IsDigit) &&
                password.Any(specialCharacters.Contains))
            {

                return true;
            }
            else
            {
                return false;
            }


        }
    }
}
