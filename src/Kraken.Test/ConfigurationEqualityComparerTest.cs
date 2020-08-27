using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Kraken.Engine;
using Xunit;

namespace Kraken.Test
{
    public class ConfigurationEqualityComparerTest
    {
        [Theory]
        [MemberData(nameof(GetAllPublicProperties))]
        public void ComparerCoversAllPoperties(PropertyInfo pi)
        {
            var fileConfiguration = new FileConfiguration();
            pi.SetValue(fileConfiguration, SomeNonDefaultValue(pi));
            var comparer = new FileConfigurationEqualityComparer();
            comparer.Equals(new FileConfiguration(), fileConfiguration).Should().BeFalse("Изменение любого свойства должно приводить к неравенству объектов");
        }

        public static IEnumerable<object[]> GetAllPublicProperties =>
            typeof(FileConfiguration)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(x => new object[] {x});
        
        private static object SomeNonDefaultValue(PropertyInfo propertyInfo)
        {
            var propertyType = propertyInfo.PropertyType;
            if (propertyType == typeof(string))
            {
                return "ChangedStringValue";
            }
            if (propertyType == typeof(Guid) || propertyType == typeof(Guid?))
            {
                return Guid.NewGuid();
            }
            if (propertyType == typeof(bool))
            {
                return true;
            }
            if (propertyType == typeof(Dictionary<string, string>))
            {
                return new Dictionary<string, string>() { ["aa"] = "bb" };
            }
            throw new NotSupportedException(propertyType.ToString() + " Add support to comparer and to this test!");
        }
    }
}