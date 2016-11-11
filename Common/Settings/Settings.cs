using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security;
using Sisyphus.Common;

namespace Sisyphus.Settings
{
    public class SettingsGroupRepresentDictionary : IDictionary<string, object>
    {
        private readonly Dictionary<string, object> _dict = new Dictionary<string, object>();

        public string GroupName { get; private set; }

        public SettingsGroupRepresentDictionary(string groupName, IEnumerable<Type> taskSettings)
        {
            GroupName = groupName;
            var typeMap = taskSettings.ToDictionary(t => t.Name);
            var sections = SettingsIni.GetSectionsByGroupName(groupName);
            foreach (var section in sections)
            {
                var className = SettingsIni.GetClassName(section);
                if (typeMap.ContainsKey(className)) Add(className, Activator.CreateInstance(typeMap[className], groupName));
            }
        }

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        public void Add(KeyValuePair<string, object> item)
        {
            _dict.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _dict.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return _dict.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {

        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return _dict.Remove(item.Key);
        }

        public int Count => _dict.Count;
        public bool IsReadOnly { get; } = false;

        public bool ContainsKey(string key)
        {
            return _dict.ContainsKey(key);
        }

        public void Add(string key, object value)
        {
            _dict.Add(key, value);
        }

        public bool Remove(string key)
        {
            return _dict.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return _dict.TryGetValue(key, out value);
        }

        object IDictionary<string, object>.this[string key] { get { return _dict[key]; } set { _dict[key] = value; } }
        public ICollection<string> Keys => _dict.Keys;
        public ICollection<object> Values => _dict.Values;

    }

    public class SettingsGroupsCollection : IList<string>
    {
        private readonly List<string> _list = new List<string>();
        public SettingsGroupsCollection(IEnumerable<Type> taskSettings)
        {
            foreach (var taskType in taskSettings)
            {
                var groups = SettingsIni.GetSections(taskType.Name);
                groups.ForEach(x => _list.Add(SettingsIni.GetGroupName(x)));
            }
            _list = _list.Distinct().ToList();
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(string item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(string item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public bool Remove(string item)
        {
            return _list.Remove(item);
        }

        public int Count => _list.Count;
        public bool IsReadOnly { get; } = false;
        public int IndexOf(string item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, string item)
        {
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public string this[int index]
        {
            get { return _list[index]; }
            set { _list[index] = value; }
        }
    }

    public class SettingsRepresentCollection : IList<object>
    {
        private readonly List<object> _list = new List<object>();

        public string SettingsRepresentTypeName { get; private set; }

        public SettingsRepresentCollection(Type settingRepresentType)
        {
            SettingsRepresentTypeName = settingRepresentType.Name;
            SettingsIni.GetSections(SettingsRepresentTypeName).ForEach(t => _list.Add(Activator.CreateInstance(settingRepresentType, t)));
        }

        public IEnumerator<object> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(object item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(object item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(object[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public bool Remove(object item)
        {
            return _list.Remove(item);
        }

        public int Count => _list.Count;
        public bool IsReadOnly { get; } = false;
        public int IndexOf(object item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, object item)
        {
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public object this[int index]
        {
            get { return _list[index]; }
            set { _list[index] = value; }
        }
    }

    public abstract partial class SettingsRepresent
    {
        protected virtual Dictionary<string, Type> ExtFields => new Dictionary<string, Type>();
        private Dictionary<string, object> _extValues;

        public readonly string EmptyValue = string.Empty;

        private string _groupName = "DefaultGroup";

        [Category("Source")]
        [DisplayName("Current group settings name.")]
        public string GroupName
        {
            get { return _groupName; }
            private set { _groupName = value == "" ? "DefaultGroup" : value; }
        }

        [Category("Source")]
        [DisplayName("Current INI section name.")]
        public string SectionName => $"{GetType().Name}.{GroupName}";

        private Dictionary<string, PropertyInfo> _embededProperties;
        private Dictionary<string, PropertyInfo> EmbededProperties => _embededProperties ?? (_embededProperties = GetEmbededProperties());

        private Dictionary<string, PropertyInfo> GetEmbededProperties()
        {
            return GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).ToDictionary(k => k.Name, v => v);
        }

        private IEnumerable<string> GetProperties()
        {
            var properties =
                GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(t => t.Name).ToArray();
            var additionaryFields = ExtFields.Keys.ToArray();
            var resultArray = properties.Union(additionaryFields).Distinct().ToArray();
            return resultArray;
        }

        private bool IsEmbededProperty(string fieldName)
        {
            return EmbededProperties.ContainsKey(fieldName);
        }

        private Type GetPropertyType(string fieldName)
        {
            if (IsEmbededProperty(fieldName))
                return EmbededProperties[fieldName].PropertyType;
            else
                return ExtFields[fieldName];
        }

        public void SetPropertyValue(string fieldName, string value)
        {
            object newValue;
            var propertyType = GetPropertyType(fieldName);
            if (propertyType.IsEnum)
            {
                newValue = Enum.Parse(propertyType, value);
            }
            else
            {
                try
                {
                    newValue = Convert.ChangeType(value, propertyType);
                }
                catch (Exception)
                {
                    return;
                }
            }
            SetPropertyValue(fieldName, newValue);
        }

        public void SetPropertyValue(string fieldName, object value)
        {
            if (IsEmbededProperty(fieldName) && EmbededProperties[fieldName].CanWrite)
                EmbededProperties[fieldName].SetValue(this, value);
            else if (!IsEmbededProperty(fieldName))
                _extValues[fieldName] = value;
        }

        private string GetPropertyValueRepresent(string fieldName)
        {
            var value = Convert.ToString(GetPropertyValue(fieldName));
            return value == "" ? EmptyValue : value;
        }

        private bool PropertyExist(string fieldName)
        {
            return IsEmbededProperty(fieldName) || ExtFields.ContainsKey(fieldName);
        }

        public object GetPropertyValue(string fieldName)
        {
            if (!PropertyExist(fieldName))
                return null;
            return IsEmbededProperty(fieldName) ? EmbededProperties[fieldName].GetValue(this) : _extValues[fieldName];
        }

        public object GetPropertyValue(string fieldName, Type type)
        {
            var currentProperty = GetPropertyValue(fieldName);
            if (currentProperty == null)
                return type == typeof(string) ? string.Empty : Activator.CreateInstance(type);
            else return Convert.ChangeType(currentProperty, type);
        }

        private void Init()
        {
            _extValues = new Dictionary<string, object>();
            foreach (var fieldName in GetProperties())
                if (IsEmbededProperty(fieldName))
                    SetPropertyValue(fieldName, GetType().GetProperty(fieldName).GetValue(this, null));
                else if (GetPropertyType(fieldName).IsValueType)
                    SetPropertyValue(fieldName, Activator.CreateInstance(GetPropertyType(fieldName)));
                else if (!IsEmbededProperty(fieldName))
                    SetPropertyValue(fieldName, "");
            if (!SectionExist(SectionName))
                SetSettings();
            else
                GetSettings();
            if (NotAllSettingsExist)
                SetSettings();
        }
        public SettingsRepresent(string groupName)
        {
            GroupName = groupName;
            Init();
        }
    }

    public abstract partial class SettingsRepresent : SettingsIni
    {
        private void GetSettings(bool create = false)
        {
            var settingsIni = GetSettingsIni();

            if (!create && settingsIni.GetSections().Count(t => t == SectionName) == 0)
                throw new ArgumentException($@"Section {SectionName} not found in saved sections.", SectionName);

            foreach (var field in GetProperties())
            {
                var value = settingsIni.GetValue(field, SectionName, NotSetValue);
                if (value == NotSetValue) NotAllSettingsExist = true;
                else SetPropertyValue(field, value);
            }
        }

        public void SetSettings()
        {
            var settingsIni = GetSettingsIni();
            foreach (var fieldName in GetProperties())
                settingsIni.WriteValue(fieldName, SectionName, GetPropertyValueRepresent(fieldName));
            SaveSettingsIni(settingsIni);
        }

        public static void EditIniFileManulUnderDifferentUser(string userName = "")
        {
            var startInfo = new ProcessStartInfo("settings.exe");
            NetworkCredential credential;
            CommonFunctions.GetCredentialsVistaAndUp(out credential);
            if (credential == null) return;
            if (credential.UserName == "") { credential.UserName = userName.GetUserName(); credential.Domain = userName.GetDomain(); }
            startInfo.Domain = credential.Domain;
            startInfo.UserName = credential.UserName;
            startInfo.Password = new SecureString();
            credential.Password.ToCharArray().ToList().ForEach(p => startInfo.Password.AppendChar(p));
            startInfo.UseShellExecute = false;
            startInfo.Verb = "runas";
            Process.Start(startInfo);
        }
    }
}
