namespace ModelBinder.Core
{
    public class Program
    {
        static void Main(string[] args)
        {
            string queryString = "id=1?name=John";
            string[] fields = queryString.Split("?");
            Dictionary<string, string> keyValueField = new Dictionary<string, string>();
            foreach (string field in fields)
            {
                keyValueField[field.Split()[0]] = field.Split()[1];
            }
            User user = new User();
            user.Id = int.Parse(keyValueField["id"]);
            user.Name = keyValueField["name"];

        }
    }
}
