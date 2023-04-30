namespace SchoolSystem.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "SchoolSystem";

        public const string AdministratorRoleName = "Administrator";

        public const int EgnLength = 10;

        public const string EgnErrorMessage = "Егн-то трябва да е дълго 10 символа, ти въведе {0} символа. Моля, опитай отново.";

        public const string ComparePasswordErrorMessage = "Полетата за парола и потвърди парола трябва да съдържат една и съща стойност.";

        public const string FirstNameErrorMessage = "Името трябва да съдържа между {0} и {1} символи.";

        public const string SurnameErrorMessage = "Презимето трябва да съдържа между {0} и {1} символи.";

        public const string LastNameErrorMessage = "Фамилията трябва да съдържа между {0} и {1} символи.";

        public const string RequiredDefaultErrorMessage = "Това поле е задължително.";

        public const string EgnDisplay = "ЕГН:";

        public const string EmailAddressDisplay = "Имейл адрес:";

        public const string FirstNameDisplay = "Име:";

        public const string SurnameDisplay = "Презиме:";

        public const string LastNameDisplay = "Фамилия:";

        public const string PasswordDisplay = "Парола:";

        public const string ConfirmPasswordDisplay = "Потвърди Парола:";

        public const int NameMinLength = 3;

        public const int NameMaxLength = 30;

        public static class Teacher
        {
            public const int MinYearsOfExperience = 1;
            public const int MaxYearsOfExperience = 40;
        }

        public static class SchoolClass
        {
            public const int ClassMinLength = 2;
            public const int ClassMaxLength = 3;
        }
    }
}
