using Common.Api.ExigoWebService;

namespace ExigoService
{
    public interface IRankRequirementDefinition
    {
        string Expression { get; set; }
        string Label { get; set; }
        RankQualificationType Type { get; set; }
        string RequirementDescription { get; set; }
        string QualifiedDescription { get; set; }
        string NotQualifiedDescription { get; set; }
        QualificationResponse Data { get; set; }
    }

    public enum RankQualificationType
    {
        Boolean = 1,
        Decimal = 2
    }
}