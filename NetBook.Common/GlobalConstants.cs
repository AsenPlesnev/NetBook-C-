namespace NetBook.Common
{
    public static class GlobalConstants
    {
        public const string AdministratorRoleName = "Administrator";

        public const string TeacherRoleName = "Teacher";

        public const string PinRegex = "^\\d{10}$";

        public const string PinRegexError = "ЕГН е точно 10 цифри";

        public const int PinLength = 10;

        public const string SchoolYearRegex = "^(20[0-4][0-9]|2050)$";

        public const string SchoolYearRegexError = "Въведете година между 2000 и 2050";

        public const string CitizenshipRegex = "^([^!@#$%^&*(),.?\":\\[\\]\\-_–{}|<>0-9]{5,30})$";

        public const string CitizenshipRegexError = "Въведете текст между 5 и 30 символа";

        public const int CitizenshipLength = 30;

        public const string SchoolNameRegex = "^([^!@#$%^&*(),?:{}|<>0-9]{15,100})$";

        public const string SchoolNameRegexError = "Въведете име между 15 и 100 символа";

        public const int SchoolNameLength = 100;

        public const string AddressRegex = "^([^!@#$%^&*()?:\\[\\]\\-_–{}|<>]{10,75})$";

        public const string AddressRegexError = "Въведете адрес между 10 и 75 символа";

        public const int AddressLength = 75;

        public const string PhoneRegexError = "Телефонът е точно 10 цифри";

        public const int PhoneLength = 10;

        public const string WorkloadRegex = "^([1-9]|[1-8][0-9]|9[0-9]|[1-4][0-9]{2}|500)$";

        public const string WorkloadRegexError = "Въведете число между 1 и 500";

        public const string GradeValueRegex = "^([2-6])$";

        public const string GradeValueRegexError = "Въведете число между 2 и 6";

        public const string SubjectNameRegex = "^([^!@#$%^&*(),.?\":\\[\\]\\-_–{}|<>0-9]{10,75})$";

        public const string SubjectNameRegexError = "Въведете текст между 10 и 75 символа";

        public const int SubjectNameLength = 75;

        public const string RemarkTextRegex = "^([^!@#$%^&*(),?\":\\[\\]\\-_–{}|<>0-9]{10,200})$";

        public const string RemarkTextRegexError = "Въведете текст между 10 и 200 символа";

        public const int RemarkTextLength = 200;

        public const string FullNameRegex = "^([^!@#$%^&*(),.?\":\\[\\]\\-_–{}|<>0-9]{8,50})$";

        public const string FullNameRegexError = "Въведете име между 8 и 50 символа";

        public const int FullNameLength = 50;

        public const string PinRegexErrorMessage = "ЕГН може да съдържа само цифри и е точно 10 символа!";

        public const string ConfirmPasswordErrorMessage = "Паролите на съвпадат";

        public const string PasswordLengthErrorMessage = "Паролата е между 6 и 100 символа";

        public const string EmailErrorMessage = "Невалидена електронна поща";

        public const string RequiredFieldError = "Полето е задължително";

        public const string PinMissingErrorMessage = "ЕГН трябва да се попълни задължително";

        public const string RegisterTeacherCode = "1fa79393-6bbe-42b2-b405-b54232c4ff25";

        public const string RegisterAdministratorCode = "8cb1a411-862e-41c0-93b5-f019fb5f6827";
    }
}
