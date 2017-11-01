using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiKey.Api.Models
{
    public class KeywordSearchResponse
    {
        public List<Part> Parts { get; set; }
        public Taxonomy Taxonomy { get; set; }
        public List<LimitedCategory> LimitedCategories { get; set; }
        public List<PidVid> FilterValues { get; set; }
        public List<LimitedFilterValue> LimitedFilterValues { get; set; }
        public int Results { get; set; }
    }

    public class StandardPricing
    {
        public int BreakQuantity { get; set; }
        public double UnitPrice { get; set; }
        public double TotalPrice { get; set; }
    }

    public class Category
    {
        public string Id { get; set; }
        public string Text { get; set; }
    }

    public class Family
    {
        public string Id { get; set; }
        public string Text { get; set; }
    }

    public class PidVid
    {
        public int ParameterId { get; set; }
        public string ValueId { get; set; }
        public string Parameter { get; set; }
        public string Value { get; set; }
    }

    public class Series
    {
        public int ParameterId { get; set; }
        public string ValueId { get; set; }
        public string Parameter { get; set; }
        public string Value { get; set; }
    }

    public class Packaging
    {
        public int ParameterId { get; set; }
        public string ValueId { get; set; }
        public string Parameter { get; set; }
        public string Value { get; set; }
    }

    public class ManufacturerName
    {
        public string Id { get; set; }
        public string Text { get; set; }
    }

    public class Part
    {
        public List<StandardPricing> StandardPricing { get; set; }
        public Category Category { get; set; }
        public bool ChipOutpostPart { get; set; }
        public string RohsInfo { get; set; }
        public string LeadStatus { get; set; }
        public Family Family { get; set; }
        public List<PidVid> Parameters { get; set; }
        public string PartUrl { get; set; }
        public string PrimaryDatasheet { get; set; }
        public string PrimaryPhoto { get; set; }
        public string PrimaryVideo { get; set; }
        public List<object> RohsSubs { get; set; }
        public Series Series { get; set; }
        public List<object> SuggestedSubs { get; set; }
        public string ManufacturerLeadWeeks { get; set; }
        public string ManufacturerPageUrl { get; set; }
        public string PartStatus { get; set; }
        public string ManufacturerPartNumber { get; set; }
        public int MinimumOrderQuantity { get; set; }
        public bool NonStock { get; set; }
        public Packaging Packaging { get; set; }
        public int PartId { get; set; }
        public int QuantityOnHand { get; set; }
        public string DigiKeyPartNumber { get; set; }
        public string ProductDescription { get; set; }
        public double UnitPrice { get; set; }
        public ManufacturerName ManufacturerName { get; set; }
        public int ManfacturerPublicQuantity { get; set; }
    }

    public class Child2
    {
        public List<object> Children { get; set; }
        public int PartCount { get; set; }
        public int NewPartCount { get; set; }
        public int ParameterId { get; set; }
        public string ValueId { get; set; }
        public string Parameter { get; set; }
        public string Value { get; set; }
    }

    public class Child
    {
        public List<Child2> Children { get; set; }
        public int PartCount { get; set; }
        public int NewPartCount { get; set; }
        public int ParameterId { get; set; }
        public string ValueId { get; set; }
        public string Parameter { get; set; }
        public string Value { get; set; }
    }

    public class Taxonomy
    {
        public List<Child> Children { get; set; }
        public int PartCount { get; set; }
        public int NewPartCount { get; set; }
        public int ParameterId { get; set; }
        public string ValueId { get; set; }
        public string Parameter { get; set; }
        public string Value { get; set; }
    }

    public class Family2
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int PartCount { get; set; }
    }

    public class LimitedCategory
    {
        public string Id { get; set; }
        public string Category { get; set; }
        public List<Family2> Families { get; set; }
    }

    public class ValueObject
    {
        public string Vid { get; set; }
        public string Value { get; set; }
    }

    public class LimitedFilterValue
    {
        public List<ValueObject> Values { get; set; }
        public int Pid { get; set; }
        public string Parameter { get; set; }
    }
}