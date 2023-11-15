using System.Net;
using System.Net.Mail;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Networking;

public class Emailer : MonoBehaviour
{
    /*[SerializeField]*/
    private string _sourceEmail = "petralexia.92@gmail.com"; //"admin@adoo.ru";
    /*[SerializeField]*/ private string _targetEmail = "petralexia.92@gmail.com";
    /*[SerializeField]*/
    private string _host =  "smtp.gmail.com"; //"smtp.ht-systems.ru";
    /*[SerializeField]*/
    private int _port = 587;//25;

    /*[SerializeField]*/
    private string login = "petralexia.92@gmail.com"; //"admin@adoo.ru";//
                                             /*[SerializeField]*/
    private string password = "z0wc4RMI"; //"Kjnlkmpok___111222333";

    [SerializeField] private TMP_InputField message;


    public void SendEmail()
    {
        string email = _targetEmail;
        string subject = MyEscapeURL("Обращение из приложения <Наследие>");
        string body = MyEscapeURL(message.text);
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);

        return;

        try
        {
            int debugCount = 0;
            //Debug.Log($"{debugCount++}");
            MailMessage mail = new MailMessage();
            //Debug.Log($"{debugCount++}");
            mail.From = new MailAddress(_sourceEmail); // Адрес отправителя
            //Debug.Log($"{debugCount++}");
            mail.To.Add(new MailAddress(_targetEmail)); // Адрес получателя
            //Debug.Log($"{debugCount++}");
            mail.Subject = "Обращение из приложения <Наследие>";
            //Debug.Log($"{debugCount++}");
            mail.Body = message.text;
            //Debug.Log($"{debugCount++}");
            SmtpClient client = new SmtpClient(_host, _port);
            //Debug.Log($"{debugCount++}");
            //client.Host = _host;
            //Debug.Log($"{debugCount++}");
            //client.Port = _port; // Обратите внимание что порт 587
            //Debug.Log($"{debugCount++}");
            client.Credentials = new NetworkCredential(login, password) as ICredentialsByHost; // Ваши логин и пароль
            client.EnableSsl = true;
            //ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate,
            //    X509Chain chain, SslPolicyErrors sslPolicyErrors)
            //{
            //    Debug.Log($"email succes");
            //    return true;
            //};

            client.Send(mail);
            Debug.Log($"{debugCount++}");
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
  
    string MyEscapeURL(string url)
    {
        return UnityWebRequest.EscapeURL(url).Replace("+", "%20");
    }
}
