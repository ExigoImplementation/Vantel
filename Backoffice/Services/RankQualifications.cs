using Common;
using Common.Api.ExigoWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ExigoService;

namespace Backoffice.Services
{
    public static partial class RankQualificationService
    {
        private static IEnumerable<IRankRequirementDefinition> GetRankQualificationDefinitions()
        {
            return new List<IRankRequirementDefinition>
            { 
                Boolean("Customer Type",
                    Expression: @"^MUST HAVE A VALID STATUS - ACTIVE",
                    Description: "You must Active."
                ),

                 Boolean("Customer Type",
                    Expression: @"^1 ACTIVE",
                    Description: "You must be Active."
                ),

                Decimal("Personal Business Volume",
                    Expression: @"^\d+ PERSONAL BUSINESS VOLUME",
                    Description: "You needed at least {{RequiredValueAsDecimal:N0}} personal business volume."
                ),

                Decimal("Group Business Volume",
                    Expression: @"^\d+ GBV \: NO MORE THAN 50\% \(300 GBV\) CAN COME FROM ANY ONE LEG",
                    Description: "You needed at least {{RequiredValueAsDecimal:N0}} group business volume."
                ),

                Decimal("Group Business Volume",
                    Expression: @"^\d+ GBV \: NO MORE THAN 50\% \(1500 GBV\) CAN COME FROM ANY ONE LEG",
                    Description: "You needed at least {{RequiredValueAsDecimal:N0}} group business volume."
                ),

                Decimal("Group Business Volume",
                    Expression: @"^\d+ GBV \: NO MORE THAN 50\% \(7500 GBV\) CAN COME FROM ANY ONE LEG",
                    Description: "You needed at least {{RequiredValueAsDecimal:N0}} group business volume."
                ),

                Decimal("Group Business Volume",
                    Expression: @"^\d+ GBV \: NO MORE THAN 50\% \(100000 GBV\) CAN COME FROM ANY ONE LEG",
                    Description: "You needed at least {{RequiredValueAsDecimal:N0}} group business volume."
                ),

                Decimal("C-1 Distributo Leg",
                    Expression: @"^\d+ C-1 DISTRIBUTOR LEGS",
                    Description: "You needed at least {{RequiredValueAsDecimal:N0}} C-1 Distributor Legs."
                ),

                Decimal("M-1 Distributo Leg",
                    Expression: @"^\d+ M-1 DISTRIBUTOR LEGS",
                    Description: "You needed at least {{RequiredValueAsDecimal:N0}} M-1 Distributor Legs."
                ),

                Decimal("D-1 Distributo Leg",
                    Expression: @"^\d+ D-1 DISTRIBUTOR LEGS",
                    Description: "You needed at least {{RequiredValueAsDecimal:N0}} D-1 Distributor Legs."
                ),

                Decimal("Personally Enrolled",
                    Expression: @"^\d+ PERSONALLY ENROLLED D-1 DISTRIBUTORS",
                    Description: "You needed at least {{RequiredValueAsDecimal:N0}} Personally Enrolled."
                )

            }.ToArray();
        }

        public static GetCustomerRankQualificationsResponse GetCustomerRankQualifications(int customerID, int periodTypeID)
        {
            return GetCustomerRankQualifications(new GetCustomerRankQualificationsRequest
            {
                CustomerID = customerID,
                PeriodTypeID = periodTypeID
            });
        }
        public static GetCustomerRankQualificationsResponse GetCustomerRankQualifications(GetCustomerRankQualificationsRequest request)
        {
            // Get the rank qualifications from the API
            var apiResponse = new GetRankQualificationsResponse();
            try
            {
                var apiRequest = new GetRankQualificationsRequest
                {
                    CustomerID = request.CustomerID,
                    PeriodType = request.PeriodTypeID
                };
                if (request.RankID != null) apiRequest.RankID = (int)request.RankID;

                apiResponse = ExigoDAL.WebService().GetRankQualifications(apiRequest);
            }
            catch(Exception exception)
            {
                return new GetCustomerRankQualificationsResponse()
                {
                    IsUnavailable = exception.Message.ToUpper().Contains("UNAVAILABLE"),
                    ErrorMessage = exception.Message
                };
            }


            // Create the response
            var response = new GetCustomerRankQualificationsResponse()
            {
                TotalPercentComplete = apiResponse.Score,
                IsQualified          = (apiResponse.Qualifies || (apiResponse.QualifiesOverride != null && ((bool)apiResponse.QualifiesOverride) == true)),
                Rank                 = new Rank()
                {
                    RankID           = apiResponse.RankID,
                    RankDescription  = apiResponse.RankDescription
                }
            };
            if (apiResponse.BackRankID != null)
            {
                response.PreviousRank = new Rank()
                {
                    RankID          = (int)apiResponse.BackRankID,
                    RankDescription = apiResponse.BackRankDescription
                };
            }
            if (apiResponse.NextRankID != null)
            {
                response.NextRank = new Rank()
                {
                    RankID          = (int)apiResponse.NextRankID,
                    RankDescription = apiResponse.NextRankDescription
                };
            }


            // Loop through each leg and create our responses
            var legs = new List<RankQualificationLeg>();
            foreach (var qualificationLeg in apiResponse.PayeeQualificationLegs)
            {
                var leg = new RankQualificationLeg();


                // Assemble the requirements
                var results = new List<RankRequirement>();
                var RankQualificationDefinitions = GetRankQualificationDefinitions();
                foreach (var definition in RankQualificationDefinitions)
                {
                    var requirement = GetRequirement(qualificationLeg, definition);
                    if (requirement != null)
                    {
                        requirement.RequirementDescription  = GlobalUtilities.MergeFields(requirement.RequirementDescription, requirement, "UNABLE TO MERGE FIELDS");                        

                        results.Add(requirement);
                    }
                }

                // Clean up nulls
                results.RemoveAll(c => string.IsNullOrEmpty(c.RequiredValue));
                leg.Requirements = results;


                legs.Add(leg);
            }
            response.QualificationLegs = legs;


            return response;
        }

        #region Helper Methods
        private static BooleanRankRequirementDefinition Boolean(string label, string Description = "", string Expression = "", string Qualified = "", string NotQualified = "")
        {
            return new BooleanRankRequirementDefinition
            {
                Label                   = label,
                Expression              = Expression,
                RequirementDescription  = Description,
                QualifiedDescription    = Qualified,
                NotQualifiedDescription = NotQualified
            };
        }
        private static DecimalRankRequirementDefinition Decimal(string label, string Description = "", string Expression = "", string Qualified = "", string NotQualified = "")
        {
            return new DecimalRankRequirementDefinition
            {
                Label                   = label,
                Expression              = Expression,
                RequirementDescription  = Description,
                QualifiedDescription    = Qualified,
                NotQualifiedDescription = NotQualified
            };
        }
        private static RankRequirement GetRequirement(QualificationResponse[] qualifications, IRankRequirementDefinition definition)
        {
            Regex regex = new Regex(definition.Expression);
            QualificationResponse qualification = null;

            qualification = (from q in qualifications
                             let description = q.QualificationDescription.ToUpper()
                             let matches = regex.Matches(description)
                             where matches.Count > 0
                             select q).FirstOrDefault();

            if (qualification != null)
            {
                return new RankRequirement(qualification, definition);
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}