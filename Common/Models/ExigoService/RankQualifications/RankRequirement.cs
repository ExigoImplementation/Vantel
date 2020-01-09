using Common;
using Common.Api.ExigoWebService;
using System;

namespace ExigoService
{
    public class RankRequirement
    {
        public RankRequirement()
        {

        }
        public RankRequirement(QualificationResponse qualification, IRankRequirementDefinition definition)
        {
            QualificationResponse = qualification;
            RequiredValue = qualification.Required;
            ActualValue = qualification.Actual;
            IsOverridden = (qualification.QualifiesOverride != null) ? Convert.ToBoolean(qualification.QualifiesOverride) : false;
            QualifiedValue = qualification.Qualifies;

            Label = definition.Label;
            RequirementDescription = definition.RequirementDescription;
            QualifiedDescription = definition.QualifiedDescription;
            NotQualifiedDescription = definition.NotQualifiedDescription;
            IsBoolean = definition.Type == RankQualificationType.Boolean;
            Expression = definition.Expression;
        }

        public QualificationResponse QualificationResponse { get; set; }
        public string Expression { get; set; }
        public string RequiredValue { get; set; }
        public string ActualValue { get; set; }
        public bool QualifiedValue { get; set; }
        public bool IsBoolean { get; set; }
        public bool IsOverridden { get; set; }
        public RankRequirementGroup Group { get; set; }
        public int GroupPriority { get; set; }

        public string RequirementDescription { get; set; }
        public string QualifiedDescription { get; set; }
        public string NotQualifiedDescription { get; set; }

        public string Label { get; set; }

        public bool IsQualified
        {
            get
            {
                return QualifiedValue || IsOverridden;
            }
        }
        public string CurrentDescription
        {
            get
            {
                if (this.IsQualified) return QualifiedDescription;
                else return NotQualifiedDescription;
            }
        }
        public decimal RequiredValueAsDecimal
        {
            get
            {
                if (!IsBoolean) return GlobalUtilities.TryParse<decimal>(this.RequiredValue, 0M);
                else return 100M;
            }
        }
        public string FormattedRequiredValueAsDecimal
        {
            get { return this.RequiredValueAsDecimal.ToString("N0"); }
        }
        public decimal ActualValueAsDecimal
        {
            get
            {
                if (!IsBoolean) return GlobalUtilities.TryParse<decimal>(this.ActualValue, 0M);
                else if (IsBoolean && this.IsQualified == true) return 100;
                else if (IsBoolean && this.IsQualified == false) return 0;
                else return 9999;
            }
        }
        public string FormattedActualValueAsDecimal
        {
            get { return this.ActualValueAsDecimal.ToString("N0"); }
        }
        public decimal RequiredToActualAsRatio
        {
            get
            {
                if (this.RequiredValueAsDecimal > 0)
                {
                    return (this.ActualValueAsDecimal / this.RequiredValueAsDecimal);
                }
                else return this.RequiredValueAsDecimal;
            }
        }
        public decimal RequiredToActualAsPercent
        {
            get
            {
                if (this.RequiredToActualAsRatio > 1M) return 100M;
                else return this.RequiredToActualAsRatio * 100;
            }
        }
        public decimal AmountNeededToQualify
        {
            get
            {
                return this.RequiredValueAsDecimal - this.ActualValueAsDecimal;
            }
        }
        public string FormattedAmountNeededToQualify
        {
            get
            {
                return this.AmountNeededToQualify.ToString("N0");
            }
        }
        public decimal ExcessAmountOverRequired
        {
            get
            {
                return this.ActualValueAsDecimal - this.RequiredValueAsDecimal;
            }
        }

        public string BooleanQualificationDescription
        {
            get { return (this.IsBoolean == true && this.ActualValueAsDecimal == this.RequiredValueAsDecimal) ? "Completed" : "Not Completed"; }
        }
        public string PercentageQualificationDescription
        {
            get
            {
                if (this.ActualValueAsDecimal == this.RequiredValueAsDecimal)
                {
                    if (this.AmountNeededToQualify != 0)
                    {
                        return "Complete";
                    }
                    else
                    {
                        return "Not Received";
                    }
                }
                else if (this.ActualValueAsDecimal == 0 && this.RequiredValueAsDecimal == 1)
                {
                    return "Not Complete";
                }
                else
                {
                    return this.ActualValueAsDecimal.ToString("N0") + " / " + this.AmountNeededToQualify.ToString("N0"); ;
                }
            }
        }
    }

    public enum RankRequirementGroup
    {
        None
    }
}