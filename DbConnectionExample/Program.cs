using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DbConnectionExample;
using System.Data;
using System.Net.NetworkInformation;
using System.IO;

namespace DbConnectionExample
{
    delegate void myDel(string _text);
    public class myConnectToMSSQLDB
    {
        public SqlConnection connection;
        public myConnectToMSSQLDB()
        {
            string conStr = @"Data Source=(localdb)\MSSQLLocalDB;" + /* Имя сервера */
                        "Initial Catalog=master;" + /* БД подключения*/
                        "Integrated Security=True;" + /* Использование уч.записи Windows */
                        "Connect Timeout=30;" + /* Таймаут в секундах*/
                        "Encrypt=False;" + /* Поддержка шифрования, работает в паре со сл.параметром */
                        "TrustServerCertificate=False;" + /* Только при подключении к экземпляру SQL Server с допустимым сертификатом. Если ключевому слову TrustServerCertificate присвоено значение true, то транспортный уровень будет использовать протокол SSL для шифрования канала и не пойдет по цепочке сертификатов для проверки доверия. */
                        "ApplicationIntent=ReadWrite;" + /* Режим подключения*/
                        "MultiSubnetFailover=False;"; /* true - поддержка уровня доступности: оптимизирует работу для пользователей одной подсети*/
            var myConn = new SqlConnection(conStr);
            try
            {
                myConn.Open();
                Console.WriteLine($"Установлено соединение с параметрами {conStr}");
            }
            catch
            {
                Console.WriteLine($"Не удалось установить соединение с параметрами {conStr}");
            }
            finally
            {
                //myConn.Close();
                connection = myConn;
                Console.WriteLine($"Закрыто соединение с параметрами {conStr}");

            }

        }

    }

}
internal class Program
{
    static void Print(string _text)
    {
        Console.WriteLine(_text);
    }
    static void WriteToFile(string _text)
    {
        string _name = "output.txt";
        var sw = new StreamWriter(_name, true);
        sw.WriteLine(_text);
        sw.Close();
        //using (var sw01 = new StreamWriter(_name))
        //{
        //    sw01.WriteLine(_text, true);
        //}

    }
    static void WriteToDB(string _text)
    {
        var connect = new myConnectToMSSQLDB();
        string _cmd = "use test;\n";
        _cmd += @"insert into [outpyt](myText) ";
        _cmd += @"values (N'";
        _cmd += _text;
        _cmd += "');";

        /* Конструетор по умолчанию */
        SqlCommand sqlCommand = new SqlCommand();
        sqlCommand.Connection = connect.connection;
        sqlCommand.CommandText = _cmd;

        /* Использование нестатического метода SqlConnection */
        //var sqlCommand = myConn.CreateCommand();
        //sqlCommand.CommandText = _cmd;

        /* Использование перегрузки конструктора с 2-мя параметрами */
        //SqlCommand sqlCommand = new SqlCommand(_cmd, myConn);

        var dataReader = sqlCommand.ExecuteReader();
        Console.WriteLine(_cmd);
        while (dataReader.Read())
        {
            int row = dataReader.FieldCount; // Вспомогательная переменная, количество возвращённых столбцов
            for (int i = 0; i < row; i++)
            {
                Console.Write("  " + dataReader[i].ToString());
            }
            Console.WriteLine();
        }
    }
    static void Main(string[] args)
    {
        myDel putText;
        putText = WriteToDB;
        putText += Print;
        putText += WriteToFile;
        putText.Invoke("TEST003");
        Console.ReadKey();
    }
}

//string conStr = @"Data Source=ATLAS_NOTE;" + /* Имя сервера */
//            "Initial Catalog=master;" +  /* БД подключения*/
//            "User ID = sa;" + /* Имя пользователя БД */
//            "Password = 1234;" + /* Его пароль */
//            "Connect Timeout=30;" + /* Таймаут в секундах*/
//            "Encrypt=False;" + /* Поддержка шифрования, работает в паре со сл.параметром */
//            "TrustServerCertificate=False;" + /* Только при подключении к экземпляру SQL Server с допустимым сертификатом. Если ключевому слову TrustServerCertificate присвоено значение true, то транспортный уровень будет использовать протокол SSL для шифрования канала и не пойдет по цепочке сертификатов для проверки доверия. */
//            "ApplicationIntent=ReadWrite;" + /* Режим подключения*/
//            "MultiSubnetFailover=False;"; /* true - поддержка уровня доступности: оптимизирует работу для пользователей одной подсети*/