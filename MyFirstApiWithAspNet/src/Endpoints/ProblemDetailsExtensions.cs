namespace MyFirstApiWithAspNet.Endpoints;

public static class ProblemDetailsExtensions
{
    public static Dictionary<String, String[]> ConvertToProblemsDatails(this IReadOnlyCollection<Notification> notifications)
    {
        return notifications
            .GroupBy(g => g.Key)
            .ToDictionary(g => g.Key, g => g.Select(x => x.Message).ToArray());
    }

    public static Dictionary<String, String[]> ConvertToProblemsDatails(this IEnumerable<IdentityError> error)
    {
        var dictionary = new Dictionary<String, String[]>();
        dictionary.Add("Error", error.Select(e => e.Description).ToArray());
        return dictionary;
    }
}