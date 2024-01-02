﻿// <auto-generated />
using System;
using System.Reflection;
using CarvedRock.Data.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;

#pragma warning disable 219, 612, 618
#nullable disable

namespace CarvedRock.Data.CompiledModels
{
    internal partial class ProductEntityType
    {
        public static RuntimeEntityType Create(RuntimeModel model, RuntimeEntityType baseEntityType = null)
        {
            var runtimeEntityType = model.AddEntityType(
                "CarvedRock.Data.Entities.Product",
                typeof(Product),
                baseEntityType);

            var id = runtimeEntityType.AddProperty(
                "Id",
                typeof(int),
                propertyInfo: typeof(Product).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(Product).GetField("<Id>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                valueGenerated: ValueGenerated.OnAdd,
                afterSaveBehavior: PropertySaveBehavior.Throw,
                sentinel: 0);
            id.TypeMapping = IntTypeMapping.Default.Clone(
                comparer: new ValueComparer<int>(
                    (int v1, int v2) => v1 == v2,
                    (int v) => v,
                    (int v) => v),
                keyComparer: new ValueComparer<int>(
                    (int v1, int v2) => v1 == v2,
                    (int v) => v,
                    (int v) => v),
                providerValueComparer: new ValueComparer<int>(
                    (int v1, int v2) => v1 == v2,
                    (int v) => v,
                    (int v) => v),
                mappingInfo: new RelationalTypeMappingInfo(
                    storeTypeName: "INTEGER"));

            var category = runtimeEntityType.AddProperty(
                "Category",
                typeof(string),
                propertyInfo: typeof(Product).GetProperty("Category", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(Product).GetField("<Category>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            category.TypeMapping = SqliteStringTypeMapping.Default;

            var description = runtimeEntityType.AddProperty(
                "Description",
                typeof(string),
                propertyInfo: typeof(Product).GetProperty("Description", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(Product).GetField("<Description>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            description.TypeMapping = SqliteStringTypeMapping.Default;

            var imgUrl = runtimeEntityType.AddProperty(
                "ImgUrl",
                typeof(string),
                propertyInfo: typeof(Product).GetProperty("ImgUrl", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(Product).GetField("<ImgUrl>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            imgUrl.TypeMapping = SqliteStringTypeMapping.Default;

            var name = runtimeEntityType.AddProperty(
                "Name",
                typeof(string),
                propertyInfo: typeof(Product).GetProperty("Name", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(Product).GetField("<Name>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            name.TypeMapping = SqliteStringTypeMapping.Default;

            var price = runtimeEntityType.AddProperty(
                "Price",
                typeof(double),
                propertyInfo: typeof(Product).GetProperty("Price", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(Product).GetField("<Price>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                sentinel: 0.0);
            price.TypeMapping = DoubleTypeMapping.Default.Clone(
                comparer: new ValueComparer<double>(
                    (double v1, double v2) => v1.Equals(v2),
                    (double v) => v.GetHashCode(),
                    (double v) => v),
                keyComparer: new ValueComparer<double>(
                    (double v1, double v2) => v1.Equals(v2),
                    (double v) => v.GetHashCode(),
                    (double v) => v),
                providerValueComparer: new ValueComparer<double>(
                    (double v1, double v2) => v1.Equals(v2),
                    (double v) => v.GetHashCode(),
                    (double v) => v),
                mappingInfo: new RelationalTypeMappingInfo(
                    storeTypeName: "REAL"));

            var ratingId = runtimeEntityType.AddProperty(
                "RatingId",
                typeof(int?),
                nullable: true);
            ratingId.TypeMapping = IntTypeMapping.Default.Clone(
                comparer: new ValueComparer<int?>(
                    (Nullable<int> v1, Nullable<int> v2) => v1.HasValue && v2.HasValue && (int)v1 == (int)v2 || !v1.HasValue && !v2.HasValue,
                    (Nullable<int> v) => v.HasValue ? (int)v : 0,
                    (Nullable<int> v) => v.HasValue ? (Nullable<int>)(int)v : default(Nullable<int>)),
                keyComparer: new ValueComparer<int?>(
                    (Nullable<int> v1, Nullable<int> v2) => v1.HasValue && v2.HasValue && (int)v1 == (int)v2 || !v1.HasValue && !v2.HasValue,
                    (Nullable<int> v) => v.HasValue ? (int)v : 0,
                    (Nullable<int> v) => v.HasValue ? (Nullable<int>)(int)v : default(Nullable<int>)),
                providerValueComparer: new ValueComparer<int?>(
                    (Nullable<int> v1, Nullable<int> v2) => v1.HasValue && v2.HasValue && (int)v1 == (int)v2 || !v1.HasValue && !v2.HasValue,
                    (Nullable<int> v) => v.HasValue ? (int)v : 0,
                    (Nullable<int> v) => v.HasValue ? (Nullable<int>)(int)v : default(Nullable<int>)),
                mappingInfo: new RelationalTypeMappingInfo(
                    storeTypeName: "INTEGER"));

            var key = runtimeEntityType.AddKey(
                new[] { id });
            runtimeEntityType.SetPrimaryKey(key);

            var index = runtimeEntityType.AddIndex(
                new[] { ratingId });

            return runtimeEntityType;
        }

        public static RuntimeForeignKey CreateForeignKey1(RuntimeEntityType declaringEntityType, RuntimeEntityType principalEntityType)
        {
            var runtimeForeignKey = declaringEntityType.AddForeignKey(new[] { declaringEntityType.FindProperty("RatingId") },
                principalEntityType.FindKey(new[] { principalEntityType.FindProperty("Id") }),
                principalEntityType);

            var rating = declaringEntityType.AddNavigation("Rating",
                runtimeForeignKey,
                onDependent: true,
                typeof(ProductRating),
                propertyInfo: typeof(Product).GetProperty("Rating", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(Product).GetField("<Rating>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

            return runtimeForeignKey;
        }

        public static void CreateAnnotations(RuntimeEntityType runtimeEntityType)
        {
            runtimeEntityType.AddAnnotation("Relational:FunctionName", null);
            runtimeEntityType.AddAnnotation("Relational:Schema", null);
            runtimeEntityType.AddAnnotation("Relational:SqlQuery", null);
            runtimeEntityType.AddAnnotation("Relational:TableName", "Products");
            runtimeEntityType.AddAnnotation("Relational:ViewName", null);
            runtimeEntityType.AddAnnotation("Relational:ViewSchema", null);

            Customize(runtimeEntityType);
        }

        static partial void Customize(RuntimeEntityType runtimeEntityType);
    }
}