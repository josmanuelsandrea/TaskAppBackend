namespace TaskApp.Infrastructure.Tools
{
    public static class ResponseMessages
    {
        public static string USER_NOT_FOUND = "Usuario no encontrado";
        public static string USER_ALREADY_EXISTS = "El usuario ya existe";
        public static string USER_FOUND = "Usuario encontrado";

        public static string RESOURCE_NOT_FOUND = "El recurso no fue encontrado";

        public static string AUTH_SUCCESS = "Autenticación correcta";
        public static string UNAUTHORIZED = "Usuario sin permisos";
        public static string NOT_VALID_TOKEN = "Token invalido";

        public static string OPERATION_SUCCESS = "Operación concretada";

        public static string SYSTEM_ERROR = "Ha ocurrido un error interno";
    }
}
