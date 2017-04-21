
namespace YoCoachServer.Helpers
{
    public class MessageHelper
    {
        public static string INVALID_DESERIALIZATION = "The values of json were sent with errors or some error ocurred with the deserealization";
        public static string INVALID_CREDENTIALS = "The credentials are wrong.";
        public static string INVALID_INPUT = "The input data coud not be found.";

        public static string MSG_REGISTER_CLIENT = "Você foi cadastrado com sucesso no YoCoach! Para baixar o app pode descarregar ele aqui {0}. O seu usuario é o seu numero de celular e a senha {1}";
    }

    public class NotificationMessage
    {
        public static string NEW_SCHEDULE_TITLE = "criou um novo agendamento! Entrei no app para ter mais detalhes";
        public static string NEW_SCHEDULE_BODY = "Novo agendamento";
    }
}