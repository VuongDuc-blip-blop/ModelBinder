namespace ModelBinder.Core
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string queryString = "id=1&name=John";
                string[] fields = queryString.Split("&");
                Dictionary<string, string> keyValueField = new Dictionary<string, string>();
                foreach (string field in fields)
                {
                    if (field.Length == 2)
                    {
                        keyValueField[field.Split()[0]] = field.Split()[1];
                    }
                }
                User user = new User();

                if (keyValueField.ContainsKey("id") && keyValueField.ContainsKey("name"))
                {
                    user.Id = int.Parse(keyValueField["id"]);
                    user.Name = keyValueField["name"];
                    Console.WriteLine($"User: Id = {user.Id} Name ={user.Name}");

                }
                else
                {
                    Console.WriteLine("Error:Missing required fields in query string");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Error:Invalid format for id");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
