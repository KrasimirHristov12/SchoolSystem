using System.Data;

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
            public const string StudentDisplay = "Ученик:";
            public const string StudentClassDisplay = "Класове:";
        }

        public static class SchoolClass
        {
            public const int ClassMinLength = 2;
            public const int ClassMaxLength = 3;
        }

        public static class Subject
        {
            public const int SubjectMinLength = 5;
            public const int SubjectMaxLength = 50;
            public const string SubjectNameDisplay = "Избери предмет:";
            public const string SubjectSuccessfullyAdded = "Предметът беше успешно добавен във вашия списък.";
        }

        public static class Grade
        {
            public const string GradeDisplay = "Оценка:";

            public const double GradeMinimum = 2.0;

            public const double GradeMaximum = 6.0;

            public const string PoorDisplay = "Слаб (2)";

            public const string FairDisplay = "Среден (3)";

            public const string GoodDisplay = "Добър (4)";

            public const string VeryGoodDisplay = "Много добър (5)";

            public const string ExcellentDisplay = "Отличен (6)";

            public const string ScalePattern = "[0-9]+[-][0-9]+";

            public const string PoorPattern = "[0][-][0-9]+";

            public const string OralReason = "Устно изпитване";

            public const string QuizReason = "Тест/писмено изпитване";

            public const string ParticipationReason = "Участие";

            public const string OtherReason = "Друго";

            public const string ReasonDisplay = "Причина:";

            public const string ApiControllerRoute = "api/grades";

            public const string GetFilteredGrades = "GetFilteredGrades";
        }

        public static class Question
        {
            public const string TitleDisplay = "Условие на въпроса:";

            public const string PointsDisplay = "Точки:";

            public const string MinimumPointsAsString = "1";

            public const string MaximumPointsAsString = "30";

            public const int TitleMaxLength = 500;

            public const string RadioDisplay = "С един верен отговор";

            public const string CheckboxDisplay = "С повече от един верен отговор";

            public const string TextDisplay = "Със свободен отговор";

            public const string PointsPropertyName = "Points";
        }

        public static class Answer
        {
            public const int ContentMaxLength = 200;
        }

        public static class Quiz
        {
            public const string EntryLevelDisplay = "Входно ниво";

            public const string MidLevelDisplay = "Междинен тест";

            public const string PlacementDisplay = "Изходно ниво";

            public const string NameDisplay = "Име на теста:";

            public const string TypeDisplay = "Тип на теста:";

            public const string DateTakenDisplay = "Дата и час на провеждане:";

            public const string QuestionTitleDisplay = "Условие на въпроса";

            public const string DurationDisplay = "Продължителност (в минути):";

            public const int NameMinLength = 10;

            public const int NameMaxLength = 100;

            public const string DurationMinNumberAsString = "10";

            public const string DurationMaxNumberAsString = "180";
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

            public const string StudentDoesNotExist = "Няма такъв ученик в системата.";

            public const string StudentCannotBeHeadTeacher = "Ученикът не може да е класен ръководител.";

            public const string StudentCannotHaveYearsOfExperience = "Ученикът не може да има стаж.";

            public const string TeacherShouldHaveClassIfHeadTeacher = "Изберете класа на който сте класен ръководител.";

            public const string TeacherShouldBeHeadTeacherIfHaveClass = "Тъй като сте избрали класа на който си класен ръководител, трябва да натиснете отметката.";

            public const string TeacherShouldHaveYearsOfExperience = "Моля въведете годините стаж, които имате.";

            public const string TeachersShouldNotBelongToClass = "Не може да сте в клас, защото сте казали, че сте учител.";

            public const string TeacherAlreadyHeadForThisClass = "Има регистриран класен ръководител за този клас в системата.";

            public const string ClassDoesNotExist = "Такъв клас не съществува.";

            public const string SubjectAlreadyExistsInTeacherCollection = "Вече имате този предмет в списъка с предметите, които преподавате.";

            public const string GradeErrorMessage = "Оценката трябва да е между 2.00 и 6.00";

            public const string ClassShouldBeUnique = "Избрали сте един клас повече от веднъж.";

            public const string ClassAlreadyInTeacherList = "Вече преподавате на този клас.";

            public const string StudentNotInClass = "Ученикът не е в този клас.";

            public const string SubjectDoesNotExist = "Предметът не съществува в системата.";

            public const string SubjectNotInTeacherList = "Текущо логнатият учител не преподава този предмет";

            public const string QuizNameLengthErrorMessage = "Името на теста трябва да съдържа между {2} и {1} символи.";

            public const string InvalidDurationOfQuiz = "Продължителността трябва да бъде между 10 минути и 180 минути.";

            public const string QuizAlreadyTaken = "Вече си предал теста. Нямаш право на повече от един опит.";

            public const string QuizDoesNotExist = "Тестът, който се опитваш да достъпиш не съществува.";

            public const string QuizDue = "Не си направил теста, когато е трябвало. Имаш служебна двойка.";

            public const string QuizNotStartedYetHours = "Изпитът започва след около {0} час/а.";

            public const string QuizNotStartedYetMinutes = "Изпитът започва след около {0} минути/а";

            public const string NoAnswerSelected = "Изберете отговор на въпроса.";

            public const string CheckAtLeastOneAnswer = "Изберете поне един от тиковете.";

            public const string AtMostOneSelectionPossibleWhenRadio = "Може да избереш само 1 от опциите.";

            public const string PointsErrorMessage = "Точките трябва да са между 1 и 30.";

            public const string ScaleIncorrectFormat = "Невалиден формат.";

            public const string PoorMinimumShouldBeZero = "Двойката трябва да започва от 0 точки.";

            public const string MinValueShouldBeOneGreaterThanMaxValueOfPrev = "Минималната стойност трябва да бъде с 1 по-голяма от максималната стойност на предишния елемент.";

            public const string MaxShouldBeGreaterThanMin = "Максималната стойност трябва да е по-голяма от минималната стойност.";

            public const string MinValueShouldBeZero = "Минималната стойност трябва да бъде 0.";

            public const string MaxValueOfLastShouldBeEqualToTotalPoints = "Максималната стойност трябва да бъде равна на общия брой точки.";
        }
    }
}
