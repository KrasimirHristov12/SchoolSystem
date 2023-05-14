namespace SchoolSystem.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "SchoolSystem";

        public const string AdministratorRoleName = "Administrator";

        public const int EgnPhoneLength = 10;

        public const string EgnDisplay = "ЕГН:";

        public const string EmailAddressDisplay = "Имейл адрес:";

        public const string FirstNameDisplay = "Име:";

        public const string SurnameDisplay = "Презиме:";

        public const string LastNameDisplay = "Фамилия:";

        public const string PasswordDisplay = "Парола:";

        public const string ConfirmPasswordDisplay = "Потвърди Парола:";

        public const int PasswordMinLength = 6;

        public const int PasswordMaxLength = 50;

        public const int NameMinLength = 3;

        public const int NameMaxLength = 30;

        public const string PhoneNumberDisplay = "Телефонен номер:";

        public const int PhoneNumberMinLength = 10;

        public const int PhoneNumberMaxLength = 13;

        public const string PhoneRegexPattern = "^(\\+359|0)\\s?8(\\d{2}\\s\\d{3}\\d{3}|[789]\\d{7})$";

        public const string EgnRegexPattern = "[0-9]{2}[0,1,2,4][0-9][0-9]{2}[0-9]{4}";

        public const string PasswordRegexPattern = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).+$";

        public static class Teacher
        {
            public const string TeacherRoleName = "Teacher";
            public const int MinYearsOfExperience = 1;
            public const int MaxYearsOfExperience = 40;
            public const string IsTeacherDisplay = "Учител ли сте?";
            public const string TeacherDisplay = "Учител";
            public const string IsClassTeacherDisplay = "Класен ръководител ли сте?";
            public const string TeacherClassNameDisplay = "На кой клас сте класен ръководител?";
            public const string TeacherYearsOfExperienceDisplay = "Стаж (в години)";
        }

        public static class Student
        {
            public const string StudentRoleName = "Student";
            public const string StudentDisplay = "Ученик";
            public const string StudentClassDisplay = "Клас:";
        }

        public static class SchoolClass
        {
            public const int ClassMinLength = 2;
            public const int ClassMaxLength = 3;
        }

        public static class ErrorMessage
        {
            public const string RequiredErrorMessage = "Това поле е задължително.";

            public const string EgnErrorMessage = "Това не е валидно ЕГН.";

            public const string EgnMustBeUnique = "Вече има потребител с това ЕГН";

            public const string PhoneMustBeUnique = "Вече има потребител с този телефонен номер";

            public const string InvalidEmail = "Това не е валиден имейл адрес.";

            public const string EmailAlreadyTaken = "Вече има потребител с този имейл адрес";

            public const string PasswordErrorMessage = "Паролата трябва да бъде между {2} и {1} символи.";

            public const string PasswordRequirementsErrorMessage = "Паролата трябва да съдържа числа, малки и големи букви";

            public const string ComparePasswordErrorMessage = "Полетата за парола и потвърди парола трябва да съдържат една и съща стойност.";

            public const string FirstNameErrorMessage = "Името трябва да съдържа между {2} и {1} символи.";

            public const string SurnameErrorMessage = "Презимето трябва да съдържа между {2} и {1} символи.";

            public const string LastNameErrorMessage = "Фамилията трябва да съдържа между {2} и {1} символи.";

            public const string PhoneNumberErrorMessage = "Въведеното не е валиден български номер.";

            public const string LoginErrorMessage = "Грешен имейл или парола.";

            public const string StudentShouldHaveClass = "Моля въведи кой клас си.";

            public const string StudentCannotBeHeadTeacher = "Ученикът не може да е класен ръководител.";

            public const string StudentCannotHaveYearsOfExperience = "Ученикът не може да има стаж.";

            public const string TeacherShouldHaveClassIfHeadTeacher = "Изберете класа на който сте класен ръководител.";

            public const string TeacherShouldBeHeadTeacherIfHaveClass = "Тъй като сте избрали класа на който си класен ръководител, трябва да натиснете отметката.";

            public const string TeacherShouldHaveYearsOfExperience = "Моля въведете годините стаж, които имате.";

            public const string TeachersShouldNotBelongToClass = "Не може да сте в клас, защото сте казали, че сте учител.";

            public const string TeacherAlreadyHeadForThisClass = "Има регистриран класен ръководител за този клас в системата.";

            public const string ClassDoesNotExist = "Такъв клас не съществува.";
        }
    }
}
