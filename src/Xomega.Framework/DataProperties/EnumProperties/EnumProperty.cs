﻿// Copyright (c) 2020 Xomega.Net. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xomega.Framework.Lookup;

namespace Xomega.Framework.Properties
{
    /// <summary>
    /// A data property that has enumerated set of possible values
    /// that come from a lookup table of the specified type.
    /// Internally the values are stored as objects of type <see cref="Header"/>,
    /// which can store ID, text and a number of additional attributes for the value.
    /// When a value is being set to the property it tries to resolve it to a Header
    /// by looking it up in the lookup table for the property, which is obtained
    /// from a lookup cache of a given type.
    /// </summary>
    public class EnumProperty : DataProperty<Header>
    {
        /// <summary>
        /// The type of cache to use to obtain the lookup table.
        /// By default the <see cref="LookupCache.Global"/> cache is used, which is shared across the whole application.
        /// </summary>
        public string CacheType { get; set; }

        /// <summary>
        /// Enumeration type, which is the type of a lookup table in the cache to be used for the property.
        /// This field should be initialized externally by either a subclass property
        /// or from within the data object.
        /// </summary>
        public string EnumType { get; set; }

        /// <summary>
        /// The string format that is used to obtain the key field from the Header.
        /// The default value points to the header ID (see <see cref="Header.FieldId"/>),
        /// but it can be customized to point to another unique field or a combination of fields
        /// in the header, e.g. a custom attribute that stores a unique abbreviation.
        /// </summary>
        public string KeyFormat { get; set; }

        /// <summary>
        /// The string format for a header field or combination of fields that is used
        /// to display the header as a string. The default value is to display the header text
        /// (see <see cref="Header.FieldText"/>).
        /// </summary>
        public string DisplayFormat { get; set; }

        /// <summary>
        ///  Constructs an EnumProperty with a given name and adds it to the parent data object under this name.
        /// </summary>
        /// <param name="parent">The parent data object to add the property to if applicable.</param>
        /// <param name="name">The property name that should be unique within the parent data object.</param>
        public EnumProperty(DataObject parent, string name)
            : base(parent, name)
        {
            CacheType = LookupCache.Global;
            KeyFormat = Header.FieldId;
            DisplayFormat = Header.FieldText;
            ItemsProvider = GetItems;
            AsyncItemsProvider = GetItemsAsync;
            FilterFunc = IsAllowed;
            SortField = DefaultSortField;
        }

        /// <summary>
        /// Overrides the base method to construct a list of string values for the Transport format.
        /// </summary>
        /// <param name="format">The format to create a new list for.</param>
        /// <returns>A new list for the given format.</returns>
        protected override IList CreateList(ValueFormat format)
        {
            return format == ValueFormat.Transport ? new List<string>() : base.CreateList(format);
        }

        /// <summary>
        /// Gets the lookup table for the property. The default implementation uses the <see cref="EnumType"/>
        /// to find the lookup table in the lookup cache specified by the <see cref="CacheType"/>.
        /// </summary>
        /// <param name="cachedOnly">True to return only the cached lookup table, False to try to load the it, if it's not cached.</param>
        /// <returns>The lookup table to be used for the property.</returns>
        protected virtual LookupTable GetLookupTable(bool cachedOnly = false)
        {
            LookupCache cache = LocalCacheLoader?.LocalCache ?? LookupCache.Get(parent?.ServiceProvider ?? DI.DefaultServiceProvider, CacheType);
            string tableType = LocalCacheLoader?.TableType ?? EnumType;
            return cache?.GetLookupTable(tableType, cachedOnly);
        }

        /// <summary>
        /// Gets the lookup table for the property. The default implementation uses the <see cref="EnumType"/>
        /// to find the lookup table in the lookup cache specified by the <see cref="CacheType"/>.
        /// </summary>
        /// <param name="token">Cancellation token.</param>
        /// <returns>The lookup table to be used for the property.</returns>
        protected async virtual Task<LookupTable> GetLookupTableAsync(CancellationToken token = default)
        {
            LookupCache cache = LocalCacheLoader?.LocalCache ?? LookupCache.Get(parent?.ServiceProvider ?? DI.DefaultServiceProvider, CacheType);
            string tableType = LocalCacheLoader?.TableType ?? EnumType;
            return await cache?.GetLookupTableAsync(tableType, token);
        }

        /// <summary>
        /// A function to determine if the given value is considered to be null for the given format.
        /// Overrides the base implementation to convert value for string formats.
        /// </summary>
        /// <param name="value">The value to check for null.</param>
        /// <param name="format">The value format, for which the null check is performed.</param>
        /// <returns>True if the value is considered to be null for the given format, otherwise false.</returns>
        public override bool IsValueNull(object value, ValueFormat format)
        {
            // If the value is a header with blank id (default format) then the base will return true, which is wrong here.
            if (value is Header) return false;
            // Consider empty string as non-null values if there is a list item with a blank id.
            if (value is string && ((string)value).Trim().Length == 0)
            {
                Header h = ConvertValue(value, ValueFormat.Internal) as Header;
                if (h != null && h.IsValid) return false;
            }
            return base.IsValueNull(value, format);
        }

        /// <summary>
        /// Converts a single value to a given format. For internal format
        /// this method tries to convert the value to a header by looking it up
        /// in the lookup table. For the transport format it uses the header ID.
        /// For DisplayString and EditString formats it displays the header formatted according
        /// to the specified <see cref="DisplayFormat"/> or <see cref="KeyFormat"/> respectively.
        /// </summary>
        /// <param name="value">A single value to convert to the given format.</param>
        /// <param name="format">The value format to convert the value to.</param>
        /// <returns>The value converted to the given format.</returns>
        protected override object ConvertValue(object value, ValueFormat format)
        {
            Header h = value as Header;
            if (format == ValueFormat.Internal)
            {
                if (h != null && h.Type == EnumType) return value;
                string str = Convert.ToString(value);
                if (str != null)
                {
                    string trimmed = str.Trim();
                    if (trimmed.Length > 0)
                        str = trimmed;
                }
                LookupTable tbl = GetLookupTable(true);
                if (tbl != null)
                {
                    h = null;
                    if (KeyFormat != Header.FieldId) h = tbl.LookupByFormat(KeyFormat, str);
                    if (h == null) h = tbl.LookupById(str);
                    if (h != null)
                    {
                        h.DefaultFormat = KeyFormat;
                        return h;
                    }
                }
                return new Header(EnumType, str);
            }
            else if (h != null)
            {
                if (format == ValueFormat.Transport) return h.Id;
                if (format == ValueFormat.EditString) return h.ToString(KeyFormat);
                if (format == ValueFormat.DisplayString) return h.ToString(DisplayFormat);
            }
            return base.ConvertValue(value, format);
        }

        /// <inheritdoc/>
        protected override async Task<object> ConvertValueAsync(object value, ValueFormat format, CancellationToken token = default)
        {
            Header h = value as Header;
            if (format == ValueFormat.Internal)
            {
                if (h != null && h.Type == EnumType) return value;
                string str = Convert.ToString(value);
                if (str != null)
                {
                    string trimmed = str.Trim();
                    if (trimmed.Length > 0)
                        str = trimmed;
                }
                LookupTable tbl = await GetLookupTableAsync(token);
                if (tbl != null)
                {
                    h = null;
                    if (KeyFormat != Header.FieldId) h = tbl.LookupByFormat(KeyFormat, str);
                    if (h == null) h = tbl.LookupById(str);
                    if (h != null)
                    {
                        h.DefaultFormat = KeyFormat;
                        return h;
                    }
                }
                return new Header(EnumType, str);
            }
            return await base.ConvertValueAsync(value, format, token);
        }

        /// <summary>
        /// A function that is used by default as the possible items provider
        /// for the property by getting all possible values from the lookup table
        /// filtered by the specified filter function, if any, and ordered by
        /// the specified SortField function, if any.
        /// </summary>
        /// <param name="input">The user input so far.</param>
        /// <param name="row">The data row context, if any.</param>
        /// <returns>A list of possible values.</returns>
        protected virtual IEnumerable GetItems(object input, DataRow row)
        {
            LookupTable tbl = GetLookupTable();
            if (tbl != null)
            {
                IEnumerable<Header> res = tbl.GetValues(FilterFunc, row);
                if (SortField != null) res = res.OrderBy(SortField);
                foreach (Header h in res)
                {
                    // use key format as default for WPF for editable combo boxes, since there seems to be
                    // no other way to control it on the framework level
                    h.DefaultFormat = KeyFormat;
                }
                return res;
            }
            else return Enumerable.Empty<Header>();
        }

        /// <summary>
        /// A function that is used by default as the possible items provider
        /// for the property by getting all possible values from the lookup table
        /// filtered by the specified filter function, if any, and ordered by
        /// the specified SortField function, if any.
        /// </summary>
        /// <param name="input">The user input so far.</param>
        /// <param name="row">The data row context, if any.</param>
        /// <param name="token">Cancellation token.</param>
        /// <returns>A list of possible values.</returns>
        protected async virtual Task<IEnumerable> GetItemsAsync(object input, DataRow row, CancellationToken token = default)
        {
            LookupTable tbl = await GetLookupTableAsync(token);
            if (tbl != null)
            {
                IEnumerable<Header> res = tbl.GetValues(FilterFunc, row);
                if (SortField != null) res = res.OrderBy(SortField);
                foreach (Header h in res)
                {
                    // use key format as default for WPF for editable combo boxes, since there seems to be
                    // no other way to control it on the framework level
                    h.DefaultFormat = KeyFormat;
                }
                return res;
            }
            else return Enumerable.Empty<Header>();
        }

        /// <summary>
        /// Checks if the given header is an allowed possible value.
        /// By default all active headers are allowed.
        /// This is the default filter function.
        /// </summary>
        /// <param name="h">The header to check.</param>
        /// <param name="row">The data row context, if any.</param>
        /// <returns>True if the header value is allowed, false otherwise.</returns>
        public virtual bool IsAllowed(Header h, DataRow row = null)
        {
            return h != null && h.IsActive && MatchesCascadingProperties(h, row);
        }

        /// <summary>
        /// A function to filter allowed items.
        /// By default only active items are allowed.
        /// </summary>
        public Func<Header, DataRow, bool> FilterFunc;

        /// <summary>
        /// A function that extracts an item field to be used for sorting.
        /// By default items are sorted by their display string.
        /// </summary>
        public Func<Header, object> SortField;

        /// <summary>
        /// Returns a value or a combination of values from the given header
        /// that should be used for sorting headers between each other.
        /// This is a default SortField function that sorts headers by their display string.
        /// </summary>
        /// <param name="h">The header from which to get the sort field.</param>
        /// <returns>A value or a combination of values from the given header
        /// that should be used for sorting headers between each other.</returns>
        public object DefaultSortField(Header h)
        {
            object cval = h;
            if (ValueConverter == null || !ValueConverter(ref cval, ValueFormat.DisplayString))
                cval = ConvertValue(cval, ValueFormat.DisplayString);
            return cval;
        }

        #region Local Cache Loader

        /// <summary>
        /// Cache loader with a local cache to use as a list of possible values for this property.
        /// </summary>
        public LocalLookupCacheLoader LocalCacheLoader { get; set; }

        /// <summary>
        /// A list of source data properties for the values of the local cache loader input parameters.
        /// </summary>
        protected Dictionary<string, DataProperty> cacheLoaderSources = new Dictionary<string, DataProperty>();

        /// <summary>
        /// Sets a source data property for the specified input parameter of the local cache loader.
        /// The value of the input parameter will come from the source property, and the cache will be reloaded
        /// whenever the value of the source property changes.
        /// </summary>
        /// <param name="parameter">The name of the input parameter of the local cache loader.</param>
        /// <param name="sourceProperty">The source data property for the parameter's value.</param>
        public void SetCacheLoaderParameters(string parameter, DataProperty sourceProperty)
        {
            if (cacheLoaderSources.ContainsKey(parameter))
            {
                cacheLoaderSources[parameter].AsyncChange -= CacheLoaderParameterChange;
                cacheLoaderSources.Remove(parameter);
            }
            if (sourceProperty != null)
            {
                cacheLoaderSources[parameter] = sourceProperty;
                sourceProperty.AsyncChange += CacheLoaderParameterChange;
            }
        }

        /// <summary>
        /// Listens to the changes in values of the source parameter properties,
        /// reloads the local cache with the new parameters, clears any current values
        /// that are no longer match valid for the new cache. Also fires an event
        /// to notify the listeners that the list of possible values changed.
        /// </summary>
        /// <param name="property">The source parameter property that was changed.</param>
        /// <param name="e">Event arguments that describe the property change.</param>
        /// <param name="token">Cancellation token.</param>
        private async Task CacheLoaderParameterChange(object property, PropertyChangeEventArgs e, CancellationToken token)
        {
            if (!e.Change.IncludesValue() || LocalCacheLoader == null || Equals(e.OldValue, e.NewValue)) return;

            var newParams = new Dictionary<string, object>();
            foreach (string key in cacheLoaderSources.Keys)
                newParams[key] = cacheLoaderSources[key].TransportValue;
            await LocalCacheLoader.SetParametersAsync(newParams, token);
            await ClearInvalidValues(e.Row, token);

            FirePropertyChange(new PropertyChangeEventArgs(PropertyChange.Items, null, null, e.Row));
        }

        /// <summary>
        /// Clears values that don't match the current value list
        /// without changing the modification state of the property.
        /// </summary>
        public virtual async Task ClearInvalidValues(DataRow row = null, CancellationToken token = default)
        {
            if (!IsNull(row))
            {
                bool? mod = Modified;
                await SetValueAsync(GetValue(ValueFormat.Transport, row), row, token);
                if (IsMultiValued)
                    await SetValueAsync(GetValues(row).Where(h => h.IsValid).ToList(), row, token);
                else if (!GetValue(row).IsValid)
                    await SetValueAsync(null, row, token);
                Modified = mod; // don't change the modified flag for initial load
            }
        }

        #endregion

        #region Cascading Properties

        /// <summary>
        /// A dictionary that maps additional attributes that each possible value of this property may have
        /// to other properties that could be used to implement cascading restrictions of the possible values
        /// based on the current values of other properties.
        /// </summary>
        protected Dictionary<string, DataProperty> cascadingProperties = new Dictionary<string, DataProperty>();

        /// <summary>
        /// Makes the list of possible values dependent on the current value(s) of another property,
        /// which would be used to filter the list of possible values by the specified attribute.
        /// </summary>
        /// <param name="attribute">The name of the attribute of each possible value
        /// that will be matched against the current value(s) of the given data property.</param>
        /// <param name="prop">The cascading property that the list of possible values
        /// for the current property depends on.</param>
        public void SetCascadingProperty(string attribute, DataProperty prop)
        {
            if (cascadingProperties.ContainsKey(attribute))
            {
                cascadingProperties[attribute].Change -= CascadingPropertyChange;
                cascadingProperties.Remove(attribute);
            }
            if (prop != null)
            {
                cascadingProperties[attribute] = prop;
                prop.Change += CascadingPropertyChange;
            }
        }

        /// <summary>
        /// Listens to the changes in values of the cascading properties and clears any current values
        /// that no longer match the values of the cascading properties. Also fires an event
        /// to notify the listeners that the list of possible values changed.
        /// </summary>
        /// <param name="property">The cascading property that was changed.</param>
        /// <param name="e">Event arguments that describe the property change.</param>
        private void CascadingPropertyChange(object property, PropertyChangeEventArgs e)
        {
            if (!e.Change.IncludesValue() || Equals(e.OldValue, e.NewValue)) return;

            if (!IsNull() && FilterFunc != null)
            {
                if (IsMultiValued) Values = Values.Where(h => FilterFunc(h, e.Row)).ToList();
                else if (!FilterFunc(Value, e.Row)) Value = null;
            }
            FirePropertyChange(new PropertyChangeEventArgs(PropertyChange.Items, null, null, e.Row));
        }

        /// <summary>
        /// True if null cascading value matches only values with attributes set to null.
        /// False if cascading value matches any value.
        /// </summary>
        public bool CascadingMatchNulls { get; set; }

        /// <summary>
        /// True if null attribute value matches any value of the cascading property.
        /// False if null attribute value matches only null value of the cascading property.
        /// </summary>
        public bool NullsMatchAnyCascading { get; set; }

        /// <summary>
        /// The method that determines if a given possible value matches the current values
        /// of all cascading properties using the attribute specified for each property.
        /// Cascading properties with blank values are ignored, i.e. a blank value
        /// is considered to match any value.
        /// This method is used as part of the default filter function <see cref="IsAllowed"/>,
        /// but can also be used separately as part of a custom filter function.
        /// </summary>
        /// <param name="h">The possible value to match against cascading properties.
        /// It should have the same attributes as specified for each cascading property.</param>
        /// <param name="row">The data row context, if any.</param>
        /// <returns>True, if the specified value matches the current value(s) of all cascading properties,
        /// false otherwise.</returns>
        public virtual bool MatchesCascadingProperties(Header h, DataRow row = null)
        {
            foreach (string attr in cascadingProperties.Keys)
            {
                DataProperty p = cascadingProperties[attr];
                object pv = p.GetValue(ValueFormat.Internal, row);
                object hv = p.ResolveValue(h[attr], ValueFormat.Internal);

                if (p.IsNull() && !CascadingMatchNulls) continue;

                IList<object> pvl = pv as IList<object>;

                bool match;
                if (hv is IList<object> hvl)
                    match = (pvl != null) ? pvl.Intersect(hvl).Count() > 0 : hvl.Contains(pv);
                else if (hv != null) match = (pvl != null) ? pvl.Contains(hv) : hv.Equals(pv);
                else if (NullsMatchAnyCascading) match = true;
                else match = pv == null;
                if (!match) return false;
            }
            return true;
        }

        #endregion
    }
}
