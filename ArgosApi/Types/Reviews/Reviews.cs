using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgosApi.Types.Reviews
{
    public class Includes
    {
    }

    public class TagDimensions
    {
    }

    public class WatchOnTV
    {
        public string Value { get; set; }
        public string ValueLabel { get; set; }
        public string DimensionLabel { get; set; }
        public string Id { get; set; }
    }

    public class Age
    {
        public string Value { get; set; }
        public string ValueLabel { get; set; }
        public string DimensionLabel { get; set; }
        public string Id { get; set; }
    }

    public class UseTV
    {
        public string Value { get; set; }
        public string ValueLabel { get; set; }
        public string DimensionLabel { get; set; }
        public string Id { get; set; }
    }

    public class Gender
    {
        public string Value { get; set; }
        public string ValueLabel { get; set; }
        public string DimensionLabel { get; set; }
        public string Id { get; set; }
    }

    public class ContextDataValues
    {
        public WatchOnTV WatchOnTV { get; set; }
        public Age Age { get; set; }
        public UseTV UseTV { get; set; }
        public Gender Gender { get; set; }
    }

    public class Badges
    {
    }

    public class AdditionalFields
    {
    }

    public class Picturequality
    {
        public int Value { get; set; }
        public object ValueLabel { get; set; }
        public object MaxLabel { get; set; }
        public string Label { get; set; }
        public string Id { get; set; }
        public int ValueRange { get; set; }
        public object MinLabel { get; set; }
        public string DisplayType { get; set; }
    }

    public class Soundquality
    {
        public int Value { get; set; }
        public object ValueLabel { get; set; }
        public object MaxLabel { get; set; }
        public string Label { get; set; }
        public string Id { get; set; }
        public int ValueRange { get; set; }
        public object MinLabel { get; set; }
        public string DisplayType { get; set; }
    }

    public class Connectivity
    {
        public int Value { get; set; }
        public object ValueLabel { get; set; }
        public object MaxLabel { get; set; }
        public string Label { get; set; }
        public string Id { get; set; }
        public int ValueRange { get; set; }
        public object MinLabel { get; set; }
        public string DisplayType { get; set; }
    }

    public class Design
    {
        public int Value { get; set; }
        public object ValueLabel { get; set; }
        public object MaxLabel { get; set; }
        public string Label { get; set; }
        public string Id { get; set; }
        public int ValueRange { get; set; }
        public object MinLabel { get; set; }
        public string DisplayType { get; set; }
    }

    public class SecondaryRatings
    {
        public Picturequality picturequality { get; set; }
        public Soundquality soundquality { get; set; }
        public Connectivity connectivity { get; set; }
        public Design Design { get; set; }
    }

    public class Result
    {
        public TagDimensions TagDimensions { get; set; }
        public List<object> TagDimensionsOrder { get; set; }
        public List<object> AdditionalFieldsOrder { get; set; }
        public object Cons { get; set; }
        public bool IsRecommended { get; set; }
        public bool IsRatingsOnly { get; set; }
        public string UserNickname { get; set; }
        public object Pros { get; set; }
        public List<object> Photos { get; set; }
        public ContextDataValues ContextDataValues { get; set; }
        public List<object> Videos { get; set; }
        public List<string> ContextDataValuesOrder { get; set; }
        public string LastModificationTime { get; set; }
        public string SubmissionId { get; set; }
        public int TotalFeedbackCount { get; set; }
        public int TotalPositiveFeedbackCount { get; set; }
        public List<object> BadgesOrder { get; set; }
        public string UserLocation { get; set; }
        public Badges Badges { get; set; }
        public string AuthorId { get; set; }
        public bool IsFeatured { get; set; }
        public List<object> SecondaryRatingsOrder { get; set; }
        public List<object> ProductRecommendationIds { get; set; }
        public string ProductId { get; set; }
        public string Title { get; set; }
        public AdditionalFields AdditionalFields { get; set; }
        public object CampaignId { get; set; }
        public double? Helpfulness { get; set; }
        public int TotalNegativeFeedbackCount { get; set; }
        public string SubmissionTime { get; set; }
        public double? Rating { get; set; }
        public string ContentLocale { get; set; }
        public int RatingRange { get; set; }
        public int TotalCommentCount { get; set; }
        public string ReviewText { get; set; }
        public string ModerationStatus { get; set; }
        public List<object> ClientResponses { get; set; }
        public string Id { get; set; }
        public List<object> CommentIds { get; set; }
        public SecondaryRatings SecondaryRatings { get; set; }
        public string LastModeratedTime { get; set; }
    }

    public class ArgosReviews
    {
        public Includes Includes { get; set; }
        public bool HasErrors { get; set; }
        public int Offset { get; set; }
        public int TotalResults { get; set; }
        public string Locale { get; set; }
        public List<object> Errors { get; set; }
        public List<Result> Results { get; set; }
        public int Limit { get; set; }
    }
}
