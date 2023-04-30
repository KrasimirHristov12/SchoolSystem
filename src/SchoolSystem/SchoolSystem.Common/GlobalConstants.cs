namespace SchoolSystem.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "SchoolSystem";

        public const string AdministratorRoleName = "Administrator";

        public const int EgnLength = 10;

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
